using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CudaTester
{
    public class PrintPrecompiled
    {
        /// <summary>
        /// Radix-2 Step Helper Method
        /// 
        /// </summary>
        /// <param name="samples">Sample vector.</param><param name="exponentSign">Fourier series exponent sign.</param><param name="levelSize">Level Group Size.</param><param name="k">Index inside of the level.</param>
        private static void Radix2Step(Complex[] samples, int exponentSign, int levelSize, int k)
        {
            double num1 = (double)(exponentSign * k) * Math.PI / (double)levelSize;
            Complex complex1 = new Complex(Math.Cos(num1), Math.Sin(num1));
            int num2 = levelSize << 1;
            int index = k;
            while (index < samples.Length)
            {
                Complex complex2 = samples[index];
                Complex complex3 = complex1 * samples[index + levelSize];
                samples[index] = complex2 + complex3;
                samples[index + levelSize] = complex2 - complex3;
                index += num2;
            }
        }


        /// <summary>
        /// Radix-2 generic FFT for power-of-two sized sample vectors.
        /// 
        /// </summary>
        /// <param name="samples">Sample vector, where the FFT is evaluated in place.</param><param name="exponentSign">Fourier series exponent sign.</param><exception cref="T:System.ArgumentException"/>
        internal static void Radix2(Complex[] samples, int exponentSign)
        {
            Radix2Reorder<Complex>(samples);
            int levelSize = 1;
            while (levelSize < samples.Length)
            {
                for (int k = 0; k < levelSize; ++k)
                    Radix2Step(samples, exponentSign, levelSize, k);
                levelSize *= 2;
            }
        }

        /// <summary>
        /// Radix-2 Reorder Helper Method
        /// 
        /// </summary>
        /// <typeparam name="T">Sample type</typeparam><param name="samples">Sample vector</param>
        private static void Radix2Reorder<T>(T[] samples)
        {
            int index1 = 0;
            for (int index2 = 0; index2 < samples.Length - 1; ++index2)
            {
                if (index2 < index1)
                {
                    T obj = samples[index2];
                    samples[index2] = samples[index1];
                    samples[index1] = obj;
                }
                int length = samples.Length;
                do
                {
                    length >>= 1;
                    index1 ^= length;
                }
                while ((index1 & length) == 0);
            }
        }

    }
}
