using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathWorks.MATLAB.NET.Utility;
using System.Reflection;
using System.IO;

namespace GSI.Global
{
    public static class MatlabAPI
    {

        static MWMCR _matlabInstance;

        /// <summary>
        /// The global matlab instance.
        /// </summary>
        public static MWMCR Instance
        {
            get
            {
                ValidateInitialized();
                return _matlabInstance;
            }
        }

        /// <summary>
        /// Validates that the matlab api has been initialized.
        /// </summary>
        public static void ValidateInitialized()
        {
            if (_matlabInstance != null)
                return;
            try
            {
                if (!MWMCR.MCRAppInitialized)
                    throw new Exception("Cannot create MRC app instance.");
            }
            catch(Exception ex)
            {
                throw new Exception("Error in intializing MRC C# types, check 32-64 bit compatablity.",ex);
            }

            try
            {
                string ctfFileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                    + "\\matlab_resource.ctf";

                _matlabInstance = new MWMCR("", ctfFileName, true);
            }
            catch(Exception ex)
            {
                throw new Exception("Error in intializing MWMCR," + ex.Message, ex);
            }
            
        }
    }
}
