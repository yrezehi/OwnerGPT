namespace OwnerGPT.Core.Threads
{
    public class ThreadJobManager
    {
        public readonly Queue<ThreadJob> Queue;

        public ThreadJobManager()
        {
            Queue = new Queue<ThreadJob>();
        }

        public bool IsAnyJobAvailable()
        {
            return Queue.Count > 0;
        }
    }
}
