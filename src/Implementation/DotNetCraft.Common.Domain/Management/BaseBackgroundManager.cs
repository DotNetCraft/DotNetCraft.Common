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
        protected CancellationTokenSource cancellationTokenSource;
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

        protected virtual void BeforeStarting() { }

        protected virtual void AfterStarting() { }

        protected virtual void BeforeStopping() { }

        protected virtual void AfterStopping() { }

        protected virtual void OnStartWorking() { }

        /// <summary>
        /// Occurs when exception has been raised in the background work.
        /// </summary>
        /// <param name="exception">The exception</param>
        protected virtual void OnBackroundException(Exception exception) { }

        /// <summary>
        /// Occurs when manager should do background work.
        /// </summary>
        protected virtual void OnBackroundExecution() { }

        /// <summary>
        /// Occurs when background work has been completed.
        /// </summary>
        protected virtual void OnBackgorundWorkCompleted() { }

        protected virtual void OnBackgorundWorkFailed() { }

        private void ManagerBackgorundProcess(object state)
        {
            try
            {
                Thread.CurrentThread.Name = string.Format("Thread_{0}", Name);
                CancellationToken cancellationToken = (CancellationToken) state;
                TimeSpan sleepTime = managerConfiguration.SleepTime;
                while (cancellationToken.IsCancellationRequested == false)
                {
                    try
                    {
                        OnBackroundExecution();
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "There was an exception during background job execution.");
                        OnBackroundException(ex); //TODO: Make a decision to terminate, sleep or something else
                    }
                    cancellationToken.WaitHandle.WaitOne(sleepTime); //TODO: Change to decision maker
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Manager working thread has been crashed");
                OnBackgorundWorkFailed();
                return;
            }

            logger.Trace("The {0}'s background work has been completed.", Name);
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
            logger.Trace("The {0} has been started.", Name);
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
