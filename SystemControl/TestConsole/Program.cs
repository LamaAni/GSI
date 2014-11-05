using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string a = "adsd";
            string b = a;
            b = b + "asd";
            TestClass first = new TestClass();
            TestClass second = first;
            TestClass third = new TestClass("third test class");
            second.SetA("abcde");
            Console.WriteLine("First: " + first.InternalA);
            Console.WriteLine("Second: " + second.InternalA);
            Console.WriteLine("Third: " + third.InternalA);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void TestFunction(TestClass a)
        {

        }
    }
}
