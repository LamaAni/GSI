using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSI.OpenCL
{
    /// <summary>
    /// Holds information about the specified device info.
    /// </summary>
    public class GpuTaskDeviceInfo
    {
        internal GpuTaskDeviceInfo(Cloo.ComputeDevice device)
        {
            Device = device;
        }

        public Cloo.ComputeDevice Device { get; private set; }

        public long VendorId { get { return Device.VendorId; } }

        public string DeviceName { get { return Device.Name; } }

        public string Vendor { get { return Device.Vendor; } }

        public override string ToString()
        {
            return string.Join(" - ", new string[] { Vendor, DeviceName, VendorId.ToString() });
        }

    }
}
