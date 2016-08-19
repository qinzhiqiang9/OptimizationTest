using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OptimizationTest
{
    public enum MyCoordinationStatus{ AllDone, Timeout, Cancel}


    public static class GCNotify
    {
        private static Action s_notify = null;
        public static event Action Notify
        {
            add
            {
                if (s_notify == null) { new GenerationObject(0); new GenerationObject(2); }
                s_notify += value;
            }
            remove
            {
                s_notify -= value;
            }
        }


        private sealed class GenerationObject
        {
            private int m_generation;

            public GenerationObject(int generation)
            {
                this.m_generation = generation;
            }

            // this method invoked by GC
            ~GenerationObject()
            {
                if (GC.GetGeneration(this) >= m_generation)
                {
                    Action temp = Volatile.Read(ref s_notify);
                    temp();
                }

                if(
                    s_notify != null 
                    &&
                    !Environment.HasShutdownStarted
                    &&
                    !AppDomain.CurrentDomain.IsFinalizingForUnload()
                  )
                {
                    // thread run as expected
                    int thisGeneration = GC.GetGeneration(this);
                    if (thisGeneration == 0)
                    {
                        new GenerationObject(0);
                    }
                    else if (thisGeneration == 1)
                    {
 
                    }
                    else
                    {
                        GC.ReRegisterForFinalize(this);
                    }

                }

            }

        }

    }


    public sealed class MyAsyncCoodinator
    {
        private Int32 m_opCount = 1;
        private Int32 m_statusReported = 0;
        private Action<MyCoordinationStatus> m_callback;
        private Timer m_timer;

        private void TimeExpired(Object o)
        {
            ReportStatus(MyCoordinationStatus.Timeout);
        }

        private void ReportStatus(MyCoordinationStatus status)
        {
            if (Interlocked.Exchange(ref m_statusReported, 1) == 0)
            {
                m_callback(status);
            }
        }

        public void Cancel()
        {
            ReportStatus(MyCoordinationStatus.Cancel);
        }

        public void AboutToBegin(Int32 opsToAdd = 1)
        {
            Interlocked.Add(ref m_opCount, opsToAdd);
        }

        public void JustEnded()
        {
            if (Interlocked.Decrement(ref m_opCount) == 0)
            {
                // finished
                ReportStatus(MyCoordinationStatus.AllDone);
            }
        }

        public void AllBegin(Action<MyCoordinationStatus> callback,
            Int32 timeout = Timeout.Infinite)
        {
            m_callback = callback;
            if (timeout != Timeout.Infinite)
            {
                m_timer = new Timer(TimeExpired, null, timeout, Timeout.Infinite);
            }
            JustEnded();
        }
    }
}
