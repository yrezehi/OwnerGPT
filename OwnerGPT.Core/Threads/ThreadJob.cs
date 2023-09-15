using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnerGPT.Core.Threads
{
    public class ThreadJob
    {
        public int Id { get; set; }
        public readonly string Description;
        public readonly DateTime CreationTime;
        public DateTime StartTime { get; set; }
        public Thread JobThread { get; }
        public readonly ParameterizedThreadStart JonAction;

        public ThreadJob(string description, ParameterizedThreadStart jobAction, bool triggerImmediately = false)
        {
            CreationTime = DateTime.Now;
            Description = description;
            JonAction = jobAction;

            JobThread = new Thread(jobAction);
            Id = JobThread.ManagedThreadId;

            if (triggerImmediately)
            {
                StartTime = DateTime.Now;
                JobThread.Start();
            }
        }

        public void Trigger()
        {
            if (JobThread.ThreadState == ThreadState.Unstarted)
            {
                StartTime = DateTime.Now;
                JobThread.Start();
            }
        }

    }
}
