using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Calibration
{
    /// <summary>
    /// The CIE colorspace matching for the x,y,z functions. For optical frequency (lambda)
    /// in native units. Based on Simple Analytic Approximations to the CIE XYZ 
    /// Color Matching Functions by Chris Wyman, Peter-Pike Salon and Peter Shirley from NVIDIA
    /// http://www.ppsloan.org/publications/XYZJCGT.pdf
    /// </summary>
    public class CIE1931ColorSpace
    {
        /// <summary>
        /// Returns the X coefficiant.
        /// </summary>
        /// <param name="lambda">In nm units</param>
        /// <returns>The x coefficient</returns>
        public static double GetXCoef(double lambda)
        {
            return
                1.065 * Math.Exp(-0.5 * Math.Pow((lambda - 595.8) / 33.33, 2)) +
                0.366 * Math.Exp(-0.5 * Math.Pow((lambda - 446.8) / 19.44, 2));
        }

        /// <summary>
        /// Returns the Y coefficiant.
        /// </summary>
        /// <param name="lambda">In nm units</param>
        /// <returns>The y coefficient</returns>
        public static double GetYCoef(double lambda)
        {
            return
                1.014 * Math.Exp(-0.5 * Math.Pow((Math.Log(lambda) - Math.Log(556.3)) / 0.075, 2));
        }

        /// <summary>
        /// Returns the Z coefficiant.
        /// </summary>
        /// <param name="lambda">In nm units</param>
        /// <returns>The z coefficient</returns>
        public static double GetZCoef(double lambda)
        {
            return
                1.014 * Math.Exp(-0.5 * Math.Pow((Math.Log(lambda) - Math.Log(449.8)) / 0.051, 2));
        }
    }
}
