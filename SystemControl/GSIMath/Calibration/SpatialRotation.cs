using MathWorks.MATLAB.NET.Arrays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Calibration
{
    public static class SpatialRotation
    {
        static SpatialRotation()
        {
            _calib = new Matlab.Calibration();
        }

        static Matlab.Calibration _calib = new Matlab.Calibration();

        /// <summary>
        /// Returns the angle and pixels size of a set of two images.
        /// </summary>
        /// <returns></returns>
        public static void FindRotationAndPixelSize(byte[] imga, byte[] imgb, int width, double deltaX,
            double deltaY, out double angle, out double pixelSize)
        {
            MWArray[] rslt = _calib.FindRotationAndPixelSize(2,
                new MWNumericArray(imga.Select(v=>(float)v).ToArray()),
                new MWNumericArray(imgb.Select(v => (float)v).ToArray()), width, deltaX, deltaY);

            if (rslt.Length != 2)
                throw new Exception("Not all prameters were returned from matlab");
            if (rslt.Any(r => !r.IsNumericArray))
                throw new Exception("All result values must be numeric");
            angle = (double)rslt[0].ToArray().GetValue(0, 0);
            pixelSize = (double)rslt[1].ToArray().GetValue(0, 0);
        }

    }
}
