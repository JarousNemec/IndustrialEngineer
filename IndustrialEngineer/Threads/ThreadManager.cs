using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using IndustrialEnginner.Interfaces;

namespace IndustrialEngineer.Threads
{
    public class ThreadManager
    {
        public List<Thread> Threads { get; set; }

        public ThreadManager()
        {
            Threads = new List<Thread>();
        }

        public void StartThread(IThread threadObject)
        {
            Thread thread = new Thread(threadObject.Run);
            Threads.Add(thread);
            thread.Start();
        }

        public void StopThread()
        {
            MessageBox.Show("Stop thread is not implemented!");
        }

        public void StopAllThreads()
        {
            foreach (var thread in Threads)
            {
                thread.Abort();
            }
        }
    }
}