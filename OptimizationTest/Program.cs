using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

namespace OptimizationTest
{
    class Program
    {
        static bool status = false;

        static void Main(string[] args)
        {
            //Console.WriteLine("Main Start");
            //Thread thread = new Thread(Test);
            //thread.Start();
            //Thread.Sleep(TimeSpan.FromSeconds(2));
            //status = true;
            //Console.WriteLine("Main End");
            //thread.Join();

            for (int i = 0; i < 10; i++)
            {
                new BigObject(100 * 1000 * 1000);
            }

            for (int j = 0; j < 10; j++)
            {
                new HandleCollectorObj();
            }


            Console.ReadKey();
        }


        static void Test()
        {
            int i = 0;
            while (!status)
            {
                Console.WriteLine(i++);
            }
        }
    }

    public class BigObject
    {
        private readonly long _size;

        public BigObject(long size)
        {
            _size = size;
            GC.AddMemoryPressure(size);
            Console.WriteLine("BigObject initial");
        }

        ~BigObject()
        {
            GC.RemoveMemoryPressure(_size);
            Console.WriteLine("BigObject destroy");
        }
    }

    public class HandleCollectorObj
    {
        private static HandleCollector collector = new HandleCollector("HandleCollectorObj", 2);

        public HandleCollectorObj()
        {
            collector.Add();
            Console.WriteLine("HandleCollectorObj initial");
        }

        ~HandleCollectorObj()
        {
            collector.Remove();
            Console.WriteLine("HandleCollectorObj destroy");
        }
    }
}
