using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace df
{
    public class dfvThreadPool
    {
        ConcurrentQueue<Action> tasks_ = new ConcurrentQueue<Action>();
        int threadSize_ = 1;

        AutoResetEvent threadEvent_ = null;

        public volatile bool exit_ = false;

        public dfvThreadPool(int threadSize = 1)
        {
            threadSize_ = threadSize;
        }


        public void run(Action task)
        {
            init();

            tasks_.Enqueue(task);

            threadEvent_.Set();
        }

        public void exit()
        {
            exit_ = true;

            if (threadEvent_ == null)
                return;
            for(int i = 0; i < threadSize_; i++)
            {
                threadEvent_.Set();
            }
        }

        void WorkThread()
        {
            for (; ; )
            {
                if (exit_)
                    break;

                threadEvent_.WaitOne();

                if (exit_)
                    break;

                Action task;
                while (tasks_.TryDequeue(out task))
                {
                    try
                    {
                        task();
                    }
                    catch (Exception e)
                    {
                        dfv.log("thread pool exception:\r\n" + e);
                        if (dfv.errMsg == 1)
                            dfv.msgERR(e.Message);
                    }
                }
            }
        }


        public void init()
        {
            if (threadEvent_ == null)
            {
                lock (tasks_)
                {
                    if (threadEvent_ == null)
                    {
                        threadEvent_ = new AutoResetEvent(false);
                        for (int i = 0; i < threadSize_; i++)
                        {
                            new Thread(WorkThread).Start();
                        }
                    }
                }
            }
        }
    }
}
