using System.Collections.Concurrent;

namespace TasksManager.BackgroundServices
{
    public class SampleData
    {
        public ConcurrentBag<string> Data { get; set; } = new ();  
    }
}
