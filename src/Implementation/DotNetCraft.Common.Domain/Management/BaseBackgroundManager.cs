using System;
using System.Threading;
using System.Threading.Tasks;
using DotNetCraft.Common.Core.Domain.Management;
using DotNetCraft.Common.Domain.Management.Exceptions;
using Microsoft.Extensions.Logging;

namespace DotNetCraft.Common.Domain.Management
{
    public abstract class BaseBackgroundManager<TManagerConfiguration> : BaseManager<TManagerConfiguration>, IBackgroundManager
        where TManagerConfiguration : IBackgroundManagerConfiguration
    {
        protected CancellationTokenSource cancellationTokenSource;
        private Task worker;
        private readonly ManualResetEvent resetEvent;

        /// <summary>
        /// Flag shows that object is running
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="managerConfiguration">The TManagerConfiguration instance.</param>
        protected BaseBackgroundManager(TManagerConfiguration managerConfiguration, ILogger<BaseBackgroundManager<TManagerConfiguration>> logger) : base(managerConfiguration, logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            resetEvent = new ManualResetEvent(false);
            if (managerConfiguration.StartImmediately)
            {
                _logger.LogInformation("{0} is starting immediately...", Name);
                Start();
                _logger.LogInformation("The {0} has been started.", Name);
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
        protected virtual void OnBackgroundException(Exception exception) { }

        /// <summary>
        /// Occurs when manager should do background work.
        /// </summary>
        protected virtual void OnBackgroundExecution() { }

        /// <summary>
        /// Occurs when background work has been completed.
        /// </summary>
        protected virtual void OnBackgroundWorkCompleted() { }

        protected virtual void OnBackgroundWorkFailed() { }

        private void ManagerBackgroundProcess(object state)
        {
            try
            {
                Thread.CurrentThread.Name = $"Thread_{Name}";
                CancellationToken cancellationToken = cancellationTokenSource.Token;
                //CancellationToken cancellationToken = (CancellationToken) state;
                TimeSpan sleepTime = managerConfiguration.SleepTime;

                WaitHandle[] waitHandlers = new WaitHandle[] {resetEvent, cancellationToken.WaitHandle};

                while (cancellationToken.IsCancellationRequested == false)
                {
                    try
                    {
                        OnBackgroundExecution();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "There was an exception during background job execution.");
                        OnBackgroundException(ex); //TODO: Make a decision to terminate, sleep or something else
                    }

                    int signal = WaitHandle.WaitAny(waitHandlers, sleepTime);//TODO: Change to decision maker
                    if (signal == 0)
                        resetEvent.Reset();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Manager working thread has been crashed");
                OnBackgroundWorkFailed();
                return;
            }

            _logger.LogTrace("The {0}'s background work has been completed.", Name);
            OnBackgroundWorkCompleted();
        }        

        #region Implementation of IStartStoppable        

        /// <summary>
        /// Start manager.
        /// </summary>
        public void Start()
        {
            _logger.LogDebug("{0} is starting...", Name);
            if (IsRunning)
                throw new ManagerException("The manager has been already run.");

            cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;
            worker = new Task(ManagerBackgroundProcess, new object[] {resetEvent, cancellationToken});
            worker.Start();
            IsRunning = true;
            _logger.LogTrace("The {0} has been started.", Name);
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

        /// <summary>
        /// Background thread will be wake up immediately.
        /// </summary>
        /// <param name="reason">The reason</param>
        public void ForceRun(string reason)
        {
            _logger.LogDebug("{0} will be wake up immediately: {1}", Name, reason);
            resetEvent.Set();
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
