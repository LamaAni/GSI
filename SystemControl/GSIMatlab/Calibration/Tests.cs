using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Calibration
{
    public static class Tests
    {
        static Tests()
        {
        }

        public static Tuple<double,double> FindRotationAndPixelSize()
        {
            //GSI.Matlab.Calibration calib = new Matlab.Calibration();
            Matlab.Class1 c1 = new Matlab.Class1();
            c1.FindRotationAndPixelSize();
            return null;
        }
    }
}
