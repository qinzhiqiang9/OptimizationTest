using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OptimizationTest
{
    class Program
    {
        static void Main(string[] args)
        {
 
        }

        private Int32 m_flag = 0;
        private Int32 m_value = 0;
        // This method is executed by one thread
        public void Thread1()
        {
            
            // Note: These could execute in reverse order
            m_value = 5;
            m_flag = 1;
        }

        public void Thread2()
        {
            if (m_flag == 1)
            { Console.WriteLine(m_value); }
        }

    }

    //
    //class Program
    //{
    //    private static Boolean s_stopWorker = false;

    //    static void Main(string[] args)
    //    {
    //        Console.WriteLine("Main: letting worker run for 5 seconds");
    //        Thread t = new Thread(Worker);
    //        t.Start();
    //        Thread.Sleep(5000);
    //        s_stopWorker = true;
    //        Console.WriteLine("Main: waiting for worker to stop");
    //        t.Join();

    //        Console.WriteLine("Main: Worker thread has stopped");
    //    }

    //    private static void Worker(Object o)
    //    {
    //        Int32 x = 0;
    //        while (!s_stopWorker)
    //        { x++; }
    //        Console.WriteLine("Worker: stopped when x={0}", x);
    //    }
    //}
}
