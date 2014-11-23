using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cloo;
using Cloo.Bindings;

namespace GSI.OpenCL
{
    /// <summary>
    /// Implements a generic runner for a specified kernal.
    /// </summary>
    public class GpuTask : IDisposable
    {
        /// <summary>
        /// Creates a new gpu task.
        /// </summary>
        /// <param name="context"></param>
        protected GpuTask(ComputeContext context, ComputeProgram program)
        {
            Context = context;
            Program = program;
        }

        #region static methods

        /// <summary>
        /// Creates a new gpu task, and compiles the code.
        /// </summary>
        /// <param name="kernalCode">The kernal code. May contain more then one kernal.</param>
        /// <param name="platform">The platform to run on (Nvidia Cuda for example), if default takes the first available.</param>
        /// <param name="devices">The devices to run on. If null assumes all devices.</param>
        /// <returns></returns>
        public static GpuTask Create(string kernalCode, bool autobuild = true,
            ComputePlatform platform = null,
            IEnumerable<ComputeDevice> devices = null)
        {
            if (platform == null)
            {
                if (ComputePlatform.Platforms.Count == 0)
                    throw new Exception("No computation platforms found.");
                platform = ComputePlatform.Platforms[0];
            }

            if(devices==null)
            {
                devices = platform.Devices;
            }

            ComputeContext context = new ComputeContext(devices.ToArray(),
                new ComputeContextPropertyList(platform), null, IntPtr.Zero);

            ComputeProgram program = new ComputeProgram(context, kernalCode);
            GpuTask task=new GpuTask(context, new ComputeProgram(context, kernalCode));

            // compile if needed.
            if (autobuild)
                task.Compile();

            // initializing the context and returning the task.
            return task;
        }

        /// <summary>
        /// Call to run a kernal.
        /// </summary>
        /// <param name="kernalCode"></param>
        /// <param name="kernalName"></param>
        /// <param name="prepare">Call on prepare</param>
        /// <param name="complete">Call when complete</param>
        /// <param name="count">The run count</param>
        public static void Run(string kernalCode, string kernalName,
            Action<ExecutingKernal> prepare, Action<ExecutingKernal> complete, int count)
        {
            GpuTask task = GpuTask.Create(kernalCode, true);
            task.RunKernal(kernalName, prepare, complete, count);
            task.Dispose();
        }

        #endregion

        #region members

        /// <summary>
        /// If true the kernal code has been compiled.
        /// </summary>
        public bool HasBeenCompiled { get; private set; }

        /// <summary>
        /// The computation context.
        /// </summary>
        public ComputeContext Context { get; private set; }

        /// <summary>
        /// The computation program.
        /// </summary>
        public ComputeProgram Program { get; private set; }

        #endregion

        #region methods

        /// <summary>
        /// Called to compile the code.
        /// </summary>
        public void Compile()
        {
            if (HasBeenCompiled)
                return;
            // build the program.
            try
            {
                Program.Build(Context.Devices, null, null, IntPtr.Zero);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Creats a new kernal to run, via the kernal name.
        /// </summary>
        /// <param name="kernalName"></param>
        /// <param name="deviceIndex"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        public ExecutingKernal CreateKernal(string kernalName, int deviceIndex = 0, bool async = false)
        {
            return new ExecutingKernal(this, kernalName, deviceIndex, async);
        }

        /// <summary>
        /// Call to run a specific kernal.
        /// </summary>
        /// <param name="kernalName">The name of the kernal function (name of the function)</param>
        /// <param name="prepare">Called when the kernal is prepared.</param>
        /// <param name="complete">Called when the kernal execution is complete.</param>
        /// <param name="async">if true run asnyc.</param>
        public void RunKernal(string kernalName,
            Action<ExecutingKernal> prepare,
            Action<ExecutingKernal> complete, int runCount, int deviceIndex = 0, bool async = false)
        {
            // create the kernal.
            ExecutingKernal kernal = CreateKernal(kernalName, deviceIndex, async);
            RunKernal(kernal, prepare, complete, runCount, deviceIndex, async);
            kernal.Dispose();
        }

        /// <summary>
        /// Call to run a specific kernal.
        /// </summary>
        /// <param name="kernalName">The name of the kernal function (name of the function)</param>
        /// <param name="prepare">Called when the kernal is prepared.</param>
        /// <param name="complete">Called when the kernal execution is complete.</param>
        /// <param name="async">if true run asnyc.</param>
        public void RunKernal(ExecutingKernal kernal,
            Action<ExecutingKernal> prepare,
            Action<ExecutingKernal> complete, int runCount, int deviceIndex = 0, bool async = false)
        {
            kernal.Run(() =>
            {
                if (prepare != null)
                    prepare(kernal);
            },
            () =>
            {
                if (complete != null)
                    complete(kernal);

            }, runCount);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this.Program.Dispose();
            this.Program = null;

        }

        #endregion
    }


}
