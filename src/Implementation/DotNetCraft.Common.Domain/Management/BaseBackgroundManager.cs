using System;
using System.Threading;
using System.Threading.Tasks;
using DotNetCraft.Common.Core.Domain.Management;
using DotNetCraft.Common.Core.Utils.Logging;
using DotNetCraft.Common.Domain.Management.Exceptions;
using DotNetCraft.Common.Utils.Logging;

namespace DotNetCraft.Common.Domain.Management
{
    public abstract class BaseBackgroundManager<TManagerConfiguration> : BaseManager<TManagerConfiguration>, IBackgroundManager
        where TManagerConfiguration : IBackgroundManagerConfiguration
    {
        private readonly ICommonLogger logger = LogManager.GetCurrentClassLogger();
        private CancellationTokenSource cancellationTokenSource;
        private Task worker;

        /// <summary>
        /// Flag shows that object is running
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="managerConfiguration">The TManagerConfiguration instance.</param>
        protected BaseBackgroundManager(TManagerConfiguration managerConfiguration) : base(managerConfiguration)
        {
            if (managerConfiguration.StartImmediately)
            {
                logger.Info("{0} is starting immediately...", Name);
                Start();
                logger.Info("The {0} has been started.", Name);
            }
        }

        /// <summary>
        /// Occurs when exception has been raised in the background work.
        /// </summary>
        /// <param name="exception">The exception</param>
        protected virtual void OnBackroundException(Exception exception)
        {
            logger.Error(exception, "There was an exception during background job execution.");
        }

        /// <summary>
        /// Occurs when manager should do background work.
        /// </summary>
        protected abstract void OnBackroundExecution();

        /// <summary>
        /// Occurs when background work has been completed.
        /// </summary>
        protected virtual void OnBackgorundWorkCompleted()
        {
            logger.Trace("The {0}'s background work has been completed.", Name);
        }

        private void ManagerBackgorundProcess(object state)
        {
            CancellationToken cancellationToken = (CancellationToken) state;
            TimeSpan sleepTime = managerConfiguration.SleepTime;
            while (cancellationToken.IsCancellationRequested == false)
            {
                try
                {
                    OnBackroundExecution();
                    cancellationToken.WaitHandle.WaitOne(sleepTime);
                }
                catch (Exception ex)
                {
                    OnBackroundException(ex);                    
                }
            }

            OnBackgorundWorkCompleted();
        }

        #region Implementation of IStartStoppable        

        /// <summary>
        /// Start manager.
        /// </summary>
        public void Start()
        {
            logger.Debug("{0} is starting...", Name);
            if (IsRunning)
                throw new ManagerException("The manager has been already run.");

            cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;
            worker = new Task(ManagerBackgorundProcess, cancellationToken);
            worker.Start();
            IsRunning = true;
            logger.Trace("The {0] has been started.", Name);
        }

        /// <summary>
        /// Stop.
        /// </summary>
        public void Stop()
        {
            if (worker != null)
            {
                cancellationTokenSource.Cancel();
                worker.Wait();
                worker = null;
                IsRunning = false;
            }
        }

        #endregion

        #region Overrides of DisposableObject

        /// <summary>
        /// Releases resources held by the object.
        /// </summary>
        /// <param name="disposing"><c>True</c> if called manually, otherwise by GC.</param>
        public override void Dispose(bool disposing)
        {
            if (disposing)
                Stop();                    

            base.Dispose(disposing);
        }

        #endregion
    }
}
