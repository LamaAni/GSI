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
        protected GpuTask(ComputeContext context, ComputeProgram program, GpuTaskDeviceInfo di=null)
        {
            Context = context;
            Program = program;

            SelectedDevice = di != null ? di.Device : context.Devices[0];
        }

        #region device lookup

        public static GpuTaskDeviceInfo[] GetDevices()
        {
            try
            {
                return Cloo.ComputePlatform.Platforms.SelectMany(p => p.Devices)
                    .Select(d => new GpuTaskDeviceInfo(d)).ToArray();
            }
            catch(Exception ex)
            {
                return new GpuTaskDeviceInfo[0];
            }
        }

        #endregion

        #region static methods

        /// <summary>
        /// Gets the (maximal) default device memory in bytes.
        /// </summary>
        /// <returns></returns>
        public static long GetDefaultDeviceMemoryInBytes()
        {
            return GetDefaultDevice().GlobalMemorySize;
        }

        /// <summary>
        /// The maximal memory capped by 32bit parameters.
        /// </summary>
        /// <returns></returns>
        public static long GetDefaultDeviceMaxMemoryFor32BitInBytes()
        {
            long maxmem = GetDefaultDeviceMemoryInBytes();
            if (maxmem > int.MaxValue)
                return int.MaxValue;
            return maxmem;
        }

        /// <summary>
        /// The default computartion device.
        /// </summary>
        /// <returns></returns>
        public static ComputeDevice GetDefaultDevice()
        {
            ComputeDevice device =
                ComputePlatform.Platforms.SelectMany(p => p.Devices).OrderBy(d => d.MaxComputeUnits).FirstOrDefault();
            if (device == null)
                throw new Exception("No computation devices found");
            return device;
        }

        /// <summary>
        /// Creates a new gpu task, and compiles the code.
        /// </summary>
        /// <param name="kernalCode">The kernal code. May contain more then one kernal.</param>
        /// <param name="platform">The platform to run on (Nvidia Cuda for example), if default takes the first available.</param>
        /// <param name="devices">The devices to run on. If null assumes all devices.</param>
        /// <returns></returns>
        public static GpuTask Create(string kernalCode,
            IEnumerable<ComputeDevice> devices = null, bool autobuild = true,
            ComputePlatform platform = null)
        {
            if (devices == null)
            {
                if (platform != null)
                {
                    devices = platform.Devices;
                }
                else
                {
                    devices = new ComputeDevice[1] { GpuTask.GetDefaultDevice() };
                }
            }
            
            if(platform==null)
                platform = devices.First().Platform;

            ComputeContext context = new ComputeContext(devices.ToArray(),
                new ComputeContextPropertyList(platform), null, IntPtr.Zero);

            ComputeProgram program = new ComputeProgram(context, kernalCode);
            GpuTask task = new GpuTask(context, new ComputeProgram(context, kernalCode));

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
            GpuTask task = GpuTask.Create(kernalCode, null, true);
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

        /// <summary>
        /// The selected computation device.
        /// </summary>
        public ComputeDevice SelectedDevice { get; private set; }

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
                ComputeDevice[] devices = new ComputeDevice[1] { SelectedDevice };
                Program.Build(devices, "", null, IntPtr.Zero);
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
