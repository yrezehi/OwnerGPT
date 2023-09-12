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
        public string Description { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime StartTime { get; set; }
        public Thread Job { get; set; }

        public ThreadJob(ParameterizedThreadStart jobAction, string description)
        {
            Job = new Thread(jobAction);

            Id = Job.ManagedThreadId;
            CreationTime = DateTime.Now;

            Description = description;
        }

    }
}
