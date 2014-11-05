using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Timing
{
    public interface ITimeKeeper
    {
        /// <summary>
        /// This function is called to set the current elpased time to zero.
        /// </summary>
        void SetZeroTime();
    }
}
