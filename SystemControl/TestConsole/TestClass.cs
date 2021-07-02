using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    public class TestClass
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TestClass()
            : this("dummy")
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="a"></param>
        public TestClass(string a)
        {
            InternalA = a;
        }

        /// <summary>
        /// Internal field a.
        /// </summary>
        public string InternalA;

        /// <summary>
        /// A function to set a.
        /// </summary>
        /// <param name="val"></param>
        public void SetA(string val)
        {
            InternalA = val;
        }
    }
}
