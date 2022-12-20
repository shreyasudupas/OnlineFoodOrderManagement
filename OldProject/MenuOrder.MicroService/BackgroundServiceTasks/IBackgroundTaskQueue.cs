using System;
using System.Threading;
using System.Threading.Tasks;

namespace MenuOrder.MicroService.BackgroundServiceTasks
{
    public interface IBackgroundTaskQueue
    {
        /// <summary>
        /// This is for Adding Task to the Queue
        /// </summary>
        /// <param name="workItem"></param>
        void QueueBackgroundWorkItem(Func<CancellationToken,Task> workItem);
        /// <summary>
        /// Remove the Task from the queue
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Func<CancellationToken, Task>> DequeueBackgroundWorkItem(CancellationToken cancellationToken);

    }
}
