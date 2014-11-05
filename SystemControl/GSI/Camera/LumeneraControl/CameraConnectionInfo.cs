using Lumenera.USB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Camera.LumeneraControl
{
    /// <summary>
    /// Holds information about a sepcific camera and host.
    /// </summary>
    public class CameraConnectionInfo
    {
        internal CameraConnectionInfo(int index, dll.LucamVersion version, bool isGigEInterface = false)
        {
            CameraIndex=index;
            Version = version;
            if (isGigEInterface)
            {
                dll.LgcamIPConfiguration camConfig = new dll.LgcamIPConfiguration();
                dll.LgcamIPConfiguration hostConfig = new dll.LgcamIPConfiguration();
                HostConfig = new dll.LgcamIPConfiguration();
                CamMac = new byte[6];
                HostMac = new byte[6];
                dll.LgcamGetIPConfiguration(CameraIndex, CamMac, out camConfig, HostMac, out hostConfig);
                CamConfig = camConfig;
                HostConfig = hostConfig;
                Serial = string.Format("{0:X2}.{1:X2}.{2:X2}.{3:X2}.{4:X2}.{5:X2}", CamMac[0], CamMac[1], CamMac[2], CamMac[3], CamMac[4], CamMac[5]);
            }
            else
            {
                Serial = Version.SerialNumber.ToString();
            }

            IsGigEInterface = isGigEInterface;
            Name = GetCameraNameFromCameraId(Version);
        }

        /// <summary>
        /// The camera index.
        /// </summary>
        public int CameraIndex { get; private set; }

        /// <summary>
        /// Applies only to GigE
        /// </summary>
        public byte[] CamMac { get; private set; }

        /// <summary>
        /// Applies only to GigE
        /// </summary>
        public byte[] HostMac { get; private set; }

        /// <summary>
        /// Applies only to GigE
        /// </summary>
        public dll.LgcamIPConfiguration CamConfig { get; private set; }

        /// <summary>
        /// Applies only to GigE
        /// </summary>
        public dll.LgcamIPConfiguration HostConfig { get; private set; }

        /// <summary>
        /// True if GigE interface.
        /// </summary>
        public bool IsGigEInterface { get; private set; }

        /// <summary>
        /// The camera serial.
        /// </summary>
        public string Serial { get; private set; }

        /// <summary>
        /// The camera version info.
        /// </summary>
        public dll.LucamVersion Version { get; private set; }

        /// <summary>
        /// The camera name.
        /// </summary>
        public string Name { get; private set; }

        #region camera name detection

        public static string GetCameraNameFromCameraId(dll.LucamVersion camera)
        {
            return GetCameraNameFromCameraId(camera.CameraId);
        }

        public static string GetCameraNameFromCameraId(int CameraId)
        {
            string name;

            switch (CameraId)
            {
                #region"LU050"
                case 0x091:
                case 0x095:
                    name = "LU050 Series";
                    break;
                #endregion

                #region"Lu056"
                case 0x090:
                case 0x093:
                    name = "LU056 Series";
                    break;
                #endregion

                #region"lu070"
                case 0x06c:
                case 0x07c:
                case 0x08c:
                    name = "LU070 Series";
                    break;
                #endregion

                #region"Lu080"
                case 0x085:
                    name = "LU080 Series";
                    break;
                #endregion

                #region"Lu100 series"
                case 0x12:
                case 0x052:
                case 0x092:
                case 0x09d:
                    name = "LU100 Sereis";
                    break;
                case 0x094:
                    name = "LU110 Series";
                    break;
                case 0x096:
                    name = "LU120 Series";
                    break;
                case 0x07a:
                case 0x09a:
                    name = "LU130 Series";
                    break;
                case 0x098:
                    name = "LU150 Series";
                    break;
                case 0x08a:
                    name = "LU160 Series";
                    break;
                case 0x04E:
                case 0x05e:
                case 0x06e:
                case 0x099:
                case 0x09e:
                    name = "LU170 Series";
                    break;
                case 0x062:
                case 0x072:
                case 0x082:
                    name = "LU176 Series";
                    break;
                #endregion

                #region lu200
                case 0x17:
                case 0x097:
                case 0x9c:
                    name = "LU200 Series";
                    break;
                case 0x08d:
                    name = "LU270 Series";
                    break;
                #endregion

                #region Lu300 series
                case 0x087:
                    name = "LU300 Series";
                    break;

                case 0x09b:
                    name = "LU330 Series";
                    break;

                case 0x05b:
                case 0x07b:
                case 0x08b:
                    name = "LU370 Series";
                    break;
                #endregion

                #region Lu5xx
                case 0x81:
                    name = "LU500 Series";
                    break;
                #endregion

                #region Lu6xxx
                case 0x086:
                    name = "LU620 Series";
                    break;
                #endregion

                #region Lw0xx
                case 0x189:
                    name = "Lw015 Series";
                    break;
                case 0x18c:
                    name = "Lw070 Series";
                    break;

                case 0x184:
                    name = "Lw080 Series";
                    break;
                #endregion

                #region lw1xx
                case 0x196:
                    name = "Lw120 Series";
                    break;

                case 0x11a:
                case 0x19a:
                    name = "Lw130 Series";
                    break;

                case 0x10a:
                case 0x18a:
                    name = "Lw160 Series";
                    break;
                case 0x19e:
                    name = "Lw170 Series";
                    break;

                case 0x15e:
                    name = "Lw175 Series";
                    break;
                #endregion

                #region Lw2xx
                case 0x110:
                case 0x180:
                case 0x1c0:
                    name = "Lw230 Series";
                    break;
                case 0x1c6:
                    name = "Lw250 Series";
                    break;

                case 0x16d:
                case 0x1cd:
                    name = "Lw290 Series";
                    break;
                #endregion

                #region lw3xx
                case 0x19b:
                    name = "Lw330 Series";
                    break;
                #endregion

                #region lw4xx
                case 0x1c7:
                    name = "Lw450 Series";
                    break;
                #endregion

                #region lw5xx
                case 0x1ce:
                    name = "Lw530 Series";
                    break;


                case 0x115:
                case 0x1c5:
                    name = "Lw570 Series";
                    break;
                #endregion

                #region lw6xx
                case 0x186:
                    name = "Lw620 Series";
                    break;
                case 0x1c8:
                #endregion

                #region lw11xxx
                case 0x1ca:
                    name = "Lw11050 Series";
                    break;
                case 0x1c9:
                    name = "Lw16050 Series";
                    break;
                #endregion

                #region INFINITY
                case 0x01e:
                case 0x031:
                case 0x0A1:
                case 0x0b1:
                case 0x0e1:
                case 0x1a6:
                case 0x1ac:
                case 0x1e5:
                    name = "INFINITY1 Series";
                    break;
                case 0x0A2:
                case 0x0b2:
                case 0x132:
                case 0x144:
                case 0x159:
                case 0x1a7:
                case 0x1b2:
                case 0x1b7:
                    name = "INFINITY2 Series";
                    break;
                case 0x01b:
                case 0x033:
                case 0x0a3:
                case 0x0b3:
                case 0x0e3:
                case 0x135:
                case 0x15A:
                case 0x1af:
                case 0x1b5:
                    name = "INFINITY3 Series";
                    break;
                case 0x044:
                case 0x0a4:
                case 0x0b4:
                case 0x168:
                case 0x1a8:
                case 0x1ab:
                case 0x1aa:
                case 0x1f9:
                    name = "INFINITY4 Series";
                    break;
                case 0x0a5:
                case 0x0b5:
                case 0x0ba:
                    name = "INFINITY5 Series";
                    break;
                case 0x020:
                case 0x040:
                case 0x0a0:
                case 0X0B0:
                case 0x0e0:
                case 0x129:
                case 0x1a9:
                case 0x1b9:
                    name = "INFINITYX Series";
                    break;

                #endregion

                #region LV series
                case 0x46c:
                    name = "Lu070";
                    break;
                case 0x49a:
                    name = "Lv130";
                    break;

                case 0x02E:
                case 0x49e:
                    name = "Lv170";
                    break;
                case 0x480:
                    name = "Lv230";
                    break;
                case 0x487:
                    name = "Lv900";
                    break;
                case 0x49f:
                    name = "Lw110";
                    break;
                case 0x48f:
                    name = "Lw310";
                    break;
                case 0x4ce:
                    name = "Lw560";
                    break;


                case 0x432:
                case 0x4a2:
                case 0x4b1:
                    name = "INFINITY1";
                    break;
                case 0x460:
                case 0x462:
                case 0x4ae:
                    name = "INFINITY2";
                    break;
                case 0x464:
                case 0x465:
                    name = "INFINITY3";
                    break;

                case 0x461:
                    name = "INFINITY Lite";
                    break;
                case 0x463:
                    name = "INFINITYX";
                    break;



                #endregion

                #region PhotoID
                case 0x083:
                    name = "Photoid Series";
                    break;
                #endregion

                #region SKYnyx

                case 0x1dc:
                    name = "SKYnyx 2-0 Series";
                    break;
                case 0x1d0:
                    name = "SKYnyx 2-2 Series";
                    break;
                case 0x1dA:
                    name = "SKYnyx 2-1 Series";
                    break;
                #endregion

                #region Mini LM
                case 0x27c:
                case 0x28c:
                    name = "LM070";
                    break;

                case 0x264:
                case 0x284:
                    name = "LM080";
                    break;
                case 0x279:
                case 0x27a:
                case 0x29a:
                    name = "LM130";
                    break;

                case 0x269:
                case 0x26a:
                case 0x28a:
                    name = "LM160";
                    break;
                case 0x2c5:
                    name = "LM570";
                    break;
                case 0x2c8:
                    name = "LM11050";
                    break;


                #endregion

                #region LC
                case 0x384:
                    name = "LC080";
                    break;
                case 0x36e:
                case 0x39e:
                    name = "LC170";
                    break;
                case 0x38d:
                    name = "LC270";
                    break;
                case 0x38b:
                    name = "LC370";
                    break;
                case 0x3ad:
                    name = "INFINITY Lite";
                    break;
                #endregion

                #region Giga ethernet
                case 0x40000:
                    name = "LG unprogrammed id";
                    break;
                case 0x40080:
                    name = "Lg230ii";
                    break;
                case 0x400c8:
                    name = "Lg11050";
                    break;
                case 0x400ca:
                    name = "Lg11050ii";
                    break;
                case 0x401ce:
                    name = "Lg565";
                    break;
                case 0x602:
                    name = "Lt220";
                    break;
                case 0x604:
                    name = "Lt420";
                    break;

                #endregion

                default:
                    name = "Unknown";
                    break;
            }
            return name;
        }

        #endregion
    }
}
