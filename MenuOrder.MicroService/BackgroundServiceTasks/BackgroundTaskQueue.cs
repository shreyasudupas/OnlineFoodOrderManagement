using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MenuOrder.MicroService.BackgroundServiceTasks
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private ConcurrentQueue<Func<CancellationToken, Task>> _workItems = new ConcurrentQueue<Func<CancellationToken, Task>>();
        private SemaphoreSlim signal = new SemaphoreSlim(0);
        public void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem)
        {
            if (workItem == null) 
                throw new ArgumentNullException(nameof(workItem));

            _workItems.Enqueue(workItem);
            signal.Release();

        }
        public async Task<Func<CancellationToken, Task>> DequeueBackgroundWorkItem(CancellationToken cancellationToken)
        {
            await signal.WaitAsync(cancellationToken);
            _workItems.TryDequeue(out var workItem);
            return workItem;
        }

    }
}
