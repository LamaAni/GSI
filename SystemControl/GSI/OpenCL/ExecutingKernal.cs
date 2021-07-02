
using Cloo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GSI.OpenCL
{
    /// <summary>
    /// Actions that pertain to a kernal that is executing.
    /// </summary>
    public class ExecutingKernal : IDisposable
    {

        /// <summary>
        /// Creates a new kernal.
        /// </summary>
        public ExecutingKernal(GpuTask task, string kernalName, int deviceIndex = 0, bool asnyc = false)
        {
            ParametersList = new List<IParameter>();
            Task = task;
            Kernal = task.Program.CreateKernel(kernalName);
            Async = asnyc;
            DeviceIndex = deviceIndex;
        }

        #region helper classes

        internal interface IParameter
        {
            void SetToKernal(ExecutingKernal kernal);
            void ReadIfNeeded(ExecutingKernal kernal, ComputeCommandQueue commands, ComputeEventList events);

            int Index { get; }
            object GetBufferValue();
            void CleanMemory();
        }

        /// <summary>
        /// Parameter implementations.
        /// </summary>
        internal class Parameter<T> : IParameter
            where T:struct
        {
            public Parameter(int index, ref object val,
                bool isBuffer, 
                bool isReadOnly,
                Action doOnClean)
            {
                Val = val;
                IsBuffer = isBuffer;
                IsReadOnly = isReadOnly;
                Index = index;
                DoOnClean = doOnClean;
            }
            
            ~Parameter()
            {
                CleanMemory();
            }

            public int Index { get; private set; }
            public object Val { get; private set; }
            public bool IsBuffer { get; private set; }
            public bool IsReadOnly { get; private set; }
            public T[] BufferResult { get; private set; }
            public Action DoOnClean { get; private set; }

            #region IParameter Members

            public void SetToKernal(ExecutingKernal kernal)
            {
                if (IsBuffer)
                    kernal.Kernal.SetMemoryArgument(Index, (ComputeMemory)Val);
                else kernal.Kernal.SetValueArgument<T>(Index, (T)Val);
            }

            public void ReadIfNeeded(ExecutingKernal kernal, ComputeCommandQueue commands, ComputeEventList events)
            {
                if (IsReadOnly)
                    return;
                if (!IsBuffer)
                    return;

                ComputeBuffer<T> buffer = (ComputeBuffer<T>)Val;
                T[] rslt = new T[buffer.Count];
                commands.ReadFromBuffer<T>(buffer, ref rslt, false, events);
                BufferResult = rslt;
            }

            public object GetBufferValue()
            {
                if (IsReadOnly || !IsBuffer)
                    return null;
                return BufferResult;
            }

            public void CleanMemory()
            {
                if (Val == null)
                    return;
                ComputeMemory mem = Val as ComputeMemory;
                Val = null;

                if (DoOnClean != null)
                    DoOnClean();

                if (mem == null)
                    return;
                try
                {
                    mem.Dispose();
                }
                catch(Exception ex)
                {
                    // no need to catch, dispose is ok.
                }
            }

            #endregion
        }

        #endregion

        #region members

        /// <summary>
        /// The device indes for running the task.
        /// </summary>
        public int DeviceIndex { get; private set; }

        /// <summary>
        /// The task of the kernal.
        /// </summary>
        public GpuTask Task { get; private set; }

        /// <summary>
        /// If true calles the kernal asnychronically.
        /// </summary>
        public bool Async { get; private set; }

        /// <summary>
        /// The running kernal.
        /// </summary>
        public ComputeKernel Kernal { get; private set; }

        /// <summary>
        /// The list of parameters.
        /// </summary>
        List<IParameter> ParametersList { get; set; }

        #endregion

        #region Run methods

        /// <summary>
        /// Runs the kernal.
        /// <param name="callPrepare">Calls the prepare event on the kernal</param>
        /// </summary>
        public void Run(Action prepare, Action complete, int count)
        {
            if (prepare != null)
                prepare();

            GSI.Coding.CodeTimer timer = new Coding.CodeTimer(); 

            // call to prepare parameters.
            PrepareParameters();

            timer.Mark("Set params");

            // event list associated with the execution.
            ComputeEventList eventList = new ComputeEventList();

            // Create the command queue. This is used to control kernel 
            // execution and manage read/write/copy operations.
            ComputeCommandQueue commands = new ComputeCommandQueue(Task.Context,
                Task.Context.Devices[DeviceIndex], ComputeCommandQueueFlags.None);

            // Called to execute the kernal.
            Action run = () =>
            {
                // executing.
                commands.Execute(Kernal, null, new long[] { count }, null, eventList);

                timer.Mark("Execute kernal");

                // post read.
                PostReadBufferCommands(eventList, commands);

                timer.Mark("Read parameters");
                
                // finish.
                commands.Finish();

                commands.Dispose();

                if (complete != null)
                    complete();
            };

            if (Async)
                System.Threading.Tasks.Task.Run(run);
            else run();
        }

        protected virtual void PostReadBufferCommands(ComputeEventList eventList, ComputeCommandQueue commands)
        {
            // reading buffers.
            for (int i = 0; i < ParametersList.Count; i++)
            {
                ParametersList[i].ReadIfNeeded(this, commands, eventList);
            }
        }

        /// <summary>
        /// Runs the kernal.
        /// <param name="callPrepare">Calls the prepare event on the kernal</param>
        /// </summary>
        public void Run(Action complete, int count)
        {
            Run(null, complete, count);
        }

        /// <summary>
        /// Called to parepare the kernal parameters before running.
        /// </summary>
        protected virtual void PrepareParameters()
        {
            for (int i = 0; i < ParametersList.Count; i++)
            {
                if (ParametersList[i] == null)
                    throw new Exception("Missing paremter with index " + i);
                ParametersList[i].SetToKernal(this);
            }
        }

        #endregion

        #region parameter methods

        /// <summary>
        /// Internal collection function.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <param name="isBuffer"></param>
        /// <param name="isReadOnly"></param>
        void SetParameterObject<T>(int index, ref object value, bool isBuffer, bool isReadOnly, Action doOnClean = null)
            where T : struct
        {
            if (index < 0)
            {
                ParametersList.Add(new Parameter<T>(ParametersList.Count, ref value, isBuffer, isReadOnly, doOnClean));
            }
            else
            {
                while (index >= ParametersList.Count)
                    ParametersList.Add(null);
                ParametersList[index] = new Parameter<T>(index, ref value, isBuffer, isReadOnly, doOnClean);
            }
        }

        /// <summary>
        /// Sets a paremater to the kernal.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index">The index to insert the paramter into. -1=append</param>
        /// <param name="value">The value</param>
        /// <param name="isReadOnly">If true read only.</param>
        public void SetParamter<T>(T value, bool isReadOnly, int index = -1)
            where T : struct
        {
            object val = value;
            SetParameterObject<T>(index, ref val, false, isReadOnly);
        }


        /// <summary>
        /// Sets a buffer parameter to the kernal.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index">The index to insert the paramter into. -1=append</param>
        /// <param name="value">The value</param>
        /// <param name="isReadOnly">If true read only.</param>
        public unsafe void SetBufferParameter<T>(ref T[] value,
            bool isReadOnly, int index = -1)
            where T : struct
        {
            if (value == null)
                value = new T[1];

            ComputeMemoryFlags flags = ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.UseHostPointer;
            if (isReadOnly)
                flags = ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.UseHostPointer;

            // creating the pointer.
            //GCHandle handle = GCHandle.Alloc(value, GCHandleType.Pinned);
            object val = new ComputeBuffer<T>(Task.Context, flags, value);
            SetParameterObject<T>(index, ref val, true, isReadOnly);
        }

        /// <summary>
        /// Returns the buffer value after completed. 
        /// If read only or not completed this value will be null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T[] GetBufferValue<T>(int index)
        {
            return ParametersList[index].GetBufferValue() as T[];
        }

        #endregion

        #region IDisposable Members

        public void ClearParameterMemory()
        {
            IParameter[] prs = ParametersList.ToArray();
            ParametersList.Clear();
            foreach (IParameter pr in prs)
            {
                pr.CleanMemory();
            }
        }

        public void Dispose()
        {
            ClearParameterMemory();
            Kernal = null;
        }

        #endregion
    }
}
