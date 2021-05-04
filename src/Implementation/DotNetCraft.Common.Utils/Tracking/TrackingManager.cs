using System;
using System.Collections.Generic;
using DotNetCraft.Common.Core.Utils.ReflectionExtensions;
using DotNetCraft.Common.Core.Utils.Tracking;
using DotNetCraft.Common.Utils.Tracking.Config;
using Microsoft.Extensions.Logging;

namespace DotNetCraft.Common.Utils.Tracking
{
    public class TrackingManager : ITrackingManager, IDisposable
    {
        private readonly IReflectionManager _reflectionManager;
        private readonly TrackingManagerConfig _trackingManagerConfig;
        private readonly List<TrackDetails> _trackQueue;
        private readonly ILogger<TrackingManager> _logger;
        private readonly List<INotifyPropertyChangedExtended> _trackingObjects;

        private readonly object _syncObject = new object();

        public TrackingManager(IReflectionManager reflectionManager, TrackingManagerConfig trackingManagerConfig, ILogger<TrackingManager> logger)
        {
            if (reflectionManager == null)
                throw new ArgumentNullException(nameof(reflectionManager));
            if (trackingManagerConfig == null)
                throw new ArgumentNullException(nameof(trackingManagerConfig));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _reflectionManager = reflectionManager;
            _trackingManagerConfig = trackingManagerConfig;
            _trackQueue = new List<TrackDetails>();
            _trackingObjects = new List<INotifyPropertyChangedExtended>();

            _logger = logger;
        }

        #region IDisposable

        public void Dispose()
        {
            lock (_syncObject)
            {
                foreach (INotifyPropertyChangedExtended trackingObject in _trackingObjects)
                {
                    trackingObject.PropertyChanged -= Instance_PropertyChanged;
                }
            }

            Clear();
        }

        #endregion

        #region Implementation of ITrackingManager

        public void Attach(INotifyPropertyChangedExtended instance)
        {
            if (_trackingManagerConfig.IsActive == false)
                return;

            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            _logger.LogTrace("Attaching {0}...", instance);

            lock (_syncObject)
            {
                instance.PropertyChanged += Instance_PropertyChanged;
                _trackingObjects.Add(instance);
            }

            _logger.LogTrace("The object has been attached");
        }

        public void Detach(INotifyPropertyChangedExtended instance)
        {
            if (_trackingManagerConfig.IsActive == false)
                return;

            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            _logger.LogTrace("Detaching {0}...", instance);

            lock (_syncObject)
            {
                bool isRemoved = _trackingObjects.Remove(instance);

                if (isRemoved)
                {
                    instance.PropertyChanged -= Instance_PropertyChanged;
                    _logger.LogTrace("The {0} has been removed from tracking collection.");
                }
                else
                {
                    _logger.LogTrace("There was no such object ({0}) in the tracking collection.", instance);
                }
            }

            _logger.LogTrace("The {0} has been detached");
        }

        private void Instance_PropertyChanged(object sender, PropertyChangedExtendedEventArgs e)
        {
            _logger.LogTrace("The value has been changed in {0}: {1} => Adding to tracking queue...", sender, e);
            TrackDetails trackDetails = new TrackDetails(sender, e.PropertyName, e.OldValue);
            _trackQueue.Add(trackDetails);

            if (_trackQueue.Count > _trackingManagerConfig.MaxSize)
            {
                _logger.LogTrace("Limit of the tracking queue has been overcame => removing the oldest item...");
                _trackQueue.RemoveAt(0);
                _logger.LogTrace("The item has been removed.");
            }

            _logger.LogTrace("Item has been added.");
        }

        public bool Undo()
        {
            if (_trackingManagerConfig.IsActive == false)
                return false;

            TrackDetails trackDetails;
            lock (_syncObject)
            {
                if (_trackQueue.Count == 0)
                    return false;

                trackDetails = _trackQueue[_trackQueue.Count - 1];
                _trackQueue.RemoveAt(_trackQueue.Count - 1);
            }

            var property = _reflectionManager.GetPropertyInfoByName(trackDetails.PropertyName, trackDetails.Sender);
            property.SetValue(trackDetails.Sender, trackDetails.OldValue, null);
            return true;
        }

        public void Clear()
        {
            lock (_syncObject)
            {
                _trackQueue.Clear();
            }
        }

        #endregion
    }
}
