using Cloo;
using GSI.Coding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CudaTester
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        string clProgramSource = @"
kernel void VectorAdd(
    global  read_only float* a,
    global  read_only float* b,
    global write_only float* c )
{
    int index = get_global_id(0);
    c[index] = a[index] + b[index];
}
";
        private void button1_Click(object sender, EventArgs e)
        {
            CodeTimer timer = new CodeTimer();


            // Create the arrays and fill them with random data.
            int count = 10000;
            int internalN = 10000;
            float[] arrA = new float[count];
            float[] arrB = new float[count];
            float[] arrC = new float[count];

            Random rand = new Random();
            for (int i = 0; i < count; i++)
            {
                arrA[i] = (float)(rand.NextDouble() * 100);
                arrB[i] = (float)(rand.NextDouble() * 100);
            }

            float[] resultNorm = new float[count];
            float[] resultOpenCLDirect = new float[count];
            float[] resultOpenCLTask = new float[count];

            timer.Mark("Prepare");

            for (int i = 0; i < resultNorm.Length; i++)
            {
                for (int j = 0; j < internalN; j++)
                    resultNorm[i] = Convert.ToSingle(Math.Cos(arrA[i]) + Math.Cos(arrB[i]));
            }

            timer.Mark("Do normal");

            resultOpenCLDirect = DoDirectOpenCl(timer,
                count, internalN, arrA, arrB, arrC, resultNorm);

            timer.Mark("Do opencl direct");

            GSI.OpenCL.GpuTask.Run(File.ReadAllText("simple_vector_add.c"), "DoComplex",
                (k) =>
                {
                    k.SetParamter<int>(internalN, true);
                    k.SetBufferParameter<float>(ref arrA, true);
                    k.SetBufferParameter<float>(ref arrB, true);
                    k.SetBufferParameter<float>(ref arrC, false);
                },
                (k) =>
                {
                    resultOpenCLTask = k.GetBufferValue<float>(3);
                }, count);

            timer.Mark("Do opencl task");

            bool ok=true;
            for (int i = 0; i < resultNorm.Length; i++)
            {
                if (resultOpenCLDirect[i] != resultNorm[i] ||
                   resultOpenCLTask[i] != resultNorm[i])
                {
                    ok = false;
                    break;
                }
            }
                //result = DoDirectOpenCl(timer, count, internalN, arrA, arrB, arrC, result);
            timer.Mark("Do compare");

            MessageBox.Show(timer.ToTraceString() + "\n" +
                (ok ? "All ok" : "Result dose not match!!"));
        }

        private static float[] DoDirectOpenCl(CodeTimer timer, int count, int internalN, float[] arrA, float[] arrB, float[] arrC, float[] result)
        {

            ComputePlatform platform = ComputePlatform.Platforms[0];

            ComputeContextPropertyList properties = new ComputeContextPropertyList(platform);

            ComputeContext context = new ComputeContext(platform.Devices,
                properties, null, IntPtr.Zero);

            // Create the input buffers and fill them with data from the arrays.
            // Access modifiers should match those in a kernel.
            // CopyHostPointer means the buffer should be filled with the data provided in the last argument.
            ComputeBuffer<float> a = new ComputeBuffer<float>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, arrA);
            ComputeBuffer<float> b = new ComputeBuffer<float>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, arrB);


            // The output buffer doesn't need any data from the host. Only its size is specified (arrC.Length).
            ComputeBuffer<float> c = new ComputeBuffer<float>(context, ComputeMemoryFlags.WriteOnly, arrC.Length);

            ComputeProgram prog = new ComputeProgram(context, File.ReadAllText("simple_vector_add.c"));
            prog.Build(null, null, null, IntPtr.Zero); // build with no data.

            // create the specific kernal and add resources to it.
            ComputeKernel vectoradd = prog.CreateKernel("DoComplex");
            vectoradd.SetValueArgument<int>(0, internalN);
            vectoradd.SetMemoryArgument(1, a);
            vectoradd.SetMemoryArgument(2, b);
            vectoradd.SetMemoryArgument(3, c);

            timer.Mark("Prepare calculation kernal");


            // Create the event wait list. An event list is not really needed for this example but it is important to see how it works.
            // Note that events (like everything else) consume OpenCL resources and creating a lot of them may slow down execution.
            // For this reason their use should be avoided if possible.
            ComputeEventList eventList = new ComputeEventList();

            // Create the command queue. This is used to control kernel 
            // execution and manage read/write/copy operations.
            ComputeCommandQueue commands = new ComputeCommandQueue(context,
                context.Devices[0], ComputeCommandQueueFlags.None);

            // Sending the command to execute. Here it is important to know the definitions
            // for workitems and workgroups.
            commands.Execute(vectoradd, null, new long[] { count }, null, eventList);

            // reading the data (async without blocking).
            commands.ReadFromBuffer<float>(c, ref result, false, eventList);

            // waits for the command to finish.
            commands.Finish();
            return result;
        }

        private void btnDoTestFFT_Click(object sender, EventArgs e)
        {
            CodeTimer timer = new CodeTimer();

            bool isReverse = false;
            // preparing sin function.
            int internalL = (int)Math.Pow(2, 7);
            int zeropad = 20;
            int sinLength = internalL - zeropad/2;
            int numberOfVectors = 1;
            double[] dataReal = new double[internalL * numberOfVectors];
            double[] dataImg = new double[internalL * numberOfVectors];
            double freq = 5 * 2 * Math.PI;
            double[] source = new double[internalL * numberOfVectors];
            Complex[] nums = new Complex[internalL * numberOfVectors];
            Random r = new Random();
            for (int i = 0; i < numberOfVectors; i++)
            {
                for (int j = zeropad / 2; j < sinLength; j++)
                {
                    int idx=i * internalL + j;
                    source[idx] = Convert.ToSingle(Math.Sin(j * freq / internalL));
                    dataReal[idx] = source[idx];
                    dataImg[idx] = 0;
                    nums[idx] = 
                        new Complex(dataReal[idx], dataImg[idx]);
                }
            }

            int logn = (int)Math.Ceiling(Math.Log(internalL * 1.0, 2));
            uint nn = (uint)Math.Pow(2, Math.Ceiling(Math.Log(internalL * 1.0, 2)));
            double[] workBuffer = new double[nn * 2 + 1];
            for (int i = 0; i < nn; i += 2)
            {
                workBuffer[i] = dataReal[i/2];
                workBuffer[i + 1] = dataImg[i / 2];
            }

            timer.Mark("Prepare sin vector");
            int vecl = (int)Math.Pow(2, logn);

            double[] dofftR = new double[internalL];
            double[] dofftI = new double[internalL];

            double[] doGfftR = new double[vecl];
            double[] doGfftI = new double[vecl];

            double[] dofftR1 = new double[internalL];
            double[] dofftI1 = new double[internalL];

            for (int i = 0; i < internalL; i++)
            {
                dofftR[i] = dataReal[i];
                dofftI[i] = dataImg[i];
                doGfftR[i] = dataReal[i];
                doGfftI[i] = dataImg[i];
                dofftR1[i] = dataReal[i];
                dofftI1[i] = dataImg[i];
            }

            timer.Mark("Prepare buffers.");

            GSI.OpenCL.GpuTask.Run(
                File.ReadAllText("fft.c"),
                "doFFT",
                (k) =>
                {
                    k.SetParamter<uint>(nn, true);
                    k.SetBufferParameter<double>(ref workBuffer, false);
                    k.SetParamter<int>(isReverse ? -1 : 1, true);
                },
                (k) =>
                {
                    workBuffer = k.GetBufferValue<double>(1);
                },
                1);

            timer.Mark("FFT NumericalRcipies GPU complete");

            GSI.OpenCL.GpuTask.Run(
                File.ReadAllText("r2fft.c"),
                "R2FFT",
                (k) =>
                {
                    k.SetParamter<int>(logn, true);
                    k.SetBufferParameter<double>(ref doGfftR, false);
                    k.SetBufferParameter<double>(ref doGfftI, false);
                    k.SetParamter<int>(isReverse ? -1 : 1, true);
                },
                (k) =>
                {
                    doGfftR = k.GetBufferValue<double>(1);
                    doGfftI = k.GetBufferValue<double>(2);
                },
                1);

            timer.Mark("FFT Radix2 GPU complete");

            MathNet.Numerics.IntegralTransforms.Fourier.BluesteinForward(nums,
                MathNet.Numerics.IntegralTransforms.FourierOptions.NoScaling);

            timer.Mark("FFT CPU COmplete");


            SpeedTest.FFT2.DoFFT(dofftR, dofftI);

            timer.Mark("FFT 2 GPU COmplete");

            Fast_Fourier_Transform.FFT.DoFFT(dofftR1, dofftI1);

            timer.Mark("FFT Code project.");

            StreamWriter wr = new StreamWriter("rslt.csv");
            for (int i = 0; i < nn; i += 2)
            {
                dataReal[i / 2] = workBuffer[i];
                dataImg[i / 2] = workBuffer[i + 1];
            }

            timer.Mark("Converted back");

            for (int i = 0; i < internalL; i++)
            {
                wr.Write(i.ToString());
                wr.Write(",");
                wr.Write(source[i].ToString());
                wr.Write(",");
                wr.Write(dataReal[i].ToString());
                wr.Write(",");
                wr.Write(dataImg[i].ToString());
                wr.Write(",");
                wr.Write(nums[i].Real.ToString());
                wr.Write(",");
                wr.Write(nums[i].Imaginary.ToString());
                wr.Write(",");
                wr.Write(dofftR[i].ToString());
                wr.Write(",");
                wr.Write(dofftI[i].ToString());
                wr.Write(",");
                wr.Write(dofftR1[i].ToString());
                wr.Write(",");
                wr.Write(dofftI1[i].ToString());
                wr.Write(",");
                wr.Write(doGfftR[i].ToString());
                wr.Write(",");
                wr.Write(doGfftI[i].ToString());
                wr.WriteLine();
            }
            wr.Close();
            wr.Dispose();
            timer.Mark("Write");
            int nanCount = workBuffer.Where(v => double.IsNaN(v)).Count();
            MessageBox.Show(timer.ToTraceString("LV: " + workBuffer.Last() + " Nans: " + nanCount));
        }

        private void btnTestMulti_Click(object sender, EventArgs e)
        {
            CodeTimer timer = new CodeTimer();
            // preparing sin function.
            int vectorLength = 200;
            int internalVec = 100;
            int sampleNum = 10000;

            double[,] real = new double[sampleNum, vectorLength];
            double[,] imag = new double[sampleNum, vectorLength];
            double[,] source = new double[sampleNum, vectorLength];
            
            for (int s = 0; s < sampleNum; s++)
            {
                double freq = (5 + s) * 2 * Math.PI;
                for (int i = 0; i < internalVec; i++)
                {
                    source[s, i] = Math.Sin(i * freq / vectorLength);
                    real[s, i] = source[s, i];
                    imag[s, i] = 0;
                }
            }

            timer.Mark("Prepare GPU");
            int vectoractual = (int)Math.Pow(2, Math.Ceiling(Math.Log(vectorLength, 2)));
            Complex[][] nums = new Complex[sampleNum][];
            for (int s = 0; s < sampleNum; s++)
            {
                double freq = (5 + s) * 2 * Math.PI;
                nums[s] = new Complex[vectoractual];
                for (int i = 0; i < internalVec; i++)
                {
                    nums[s][i] = new Complex(Math.Sin(i * freq / vectorLength), 0);
                }
            }

            timer.Mark("Prepare complex math.net");

            MathNet.Numerics.IntegralTransforms.FourierOptions options=
                new MathNet.Numerics.IntegralTransforms.FourierOptions();

            for (int s = 0; s < sampleNum; s++)
            {
                MathNet.Numerics.IntegralTransforms.Fourier.Radix2Forward(nums[s], options);
            }

            timer.Mark("Run radix2 cpu");

            // now running the fft. (multiple samples).
            R2FFTOpenCL.Forward(ref real, ref imag);
            timer.Mark("FFT");
            StreamWriter wr = new StreamWriter("multirslt.csv");
            for (int i = 0; i < vectorLength; i++)
            {
                string[] vecs = new string[sampleNum];
                for (int s = 0; s < sampleNum; s++)
                {
                    vecs[s] = string.Join(",", new string[]{
                        source[s,i].ToString(),
                        real[s,i].ToString(),
                        imag[s,i].ToString()
                    });
                }
                wr.WriteLine(string.Join(",", vecs));
            }
            wr.Close();
            wr.Dispose();
            timer.Mark("Write");
            MessageBox.Show(timer.ToTraceString("Samp: " + sampleNum + ", vec:" + vectorLength + " -> " + real.GetLength(1)));
        }
    }
}
