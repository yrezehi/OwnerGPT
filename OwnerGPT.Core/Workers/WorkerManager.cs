﻿namespace OwnerGPT.Core.Threads
{
    public class WorkerManager
    {
        public readonly Queue<WorkerJob> Queue;

        public WorkerManager() =>
            Queue = new Queue<WorkerJob>();

        public bool IsAnyJobAvailable() =>
            Queue.Count > 0;
    }
}
