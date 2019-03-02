using System;
using System.Collections.Generic;
using DotNetCraft.Common.Core.Utils.Logging;
using DotNetCraft.Common.Core.Utils.ReflectionExtensions;
using DotNetCraft.Common.Core.Utils.Tracking;
using DotNetCraft.Common.Utils.Tracking.Config;

namespace DotNetCraft.Common.Utils.Tracking
{
    public class TrackingManager : ITrackingManager, IDisposable
    {
        private readonly IReflectionManager _reflectionManager;
        private readonly TrackingManagerConfig _trackingManagerConfig;
        private readonly List<TrackDetails> _trackQueue;
        private readonly ICommonLogger _logger;
        private readonly List<INotifyPropertyChangedExtended> _trackingObjects;

        private readonly object _syncObject = new object();

        public TrackingManager(IReflectionManager reflectionManager, TrackingManagerConfig trackingManagerConfig, ICommonLoggerFactory simpleLoggerFactory)
        {
            if (reflectionManager == null)
                throw new ArgumentNullException(nameof(reflectionManager));
            if (trackingManagerConfig == null)
                throw new ArgumentNullException(nameof(trackingManagerConfig));
            if (simpleLoggerFactory == null)
                throw new ArgumentNullException(nameof(simpleLoggerFactory));

            _reflectionManager = reflectionManager;
            _trackingManagerConfig = trackingManagerConfig;
            _trackQueue = new List<TrackDetails>();
            _trackingObjects = new List<INotifyPropertyChangedExtended>();

            _logger = simpleLoggerFactory.Create<TrackingManager>();
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

            _logger.Trace("Attaching {0}...", instance);

            lock (_syncObject)
            {
                instance.PropertyChanged += Instance_PropertyChanged;
                _trackingObjects.Add(instance);
            }

            _logger.Trace("The object has been attached");
        }

        public void Detach(INotifyPropertyChangedExtended instance)
        {
            if (_trackingManagerConfig.IsActive == false)
                return;

            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            _logger.Trace("Detaching {0}...", instance);

            lock (_syncObject)
            {
                bool isRemoved = _trackingObjects.Remove(instance);

                if (isRemoved)
                {
                    instance.PropertyChanged -= Instance_PropertyChanged;
                    _logger.Trace("The {0} has been removed from tracking collection.");
                }
                else
                {
                    _logger.Trace("There was no such object ({0}) in the tracking collection.", instance);
                }
            }

            _logger.Trace("The {0} has been detached");
        }

        private void Instance_PropertyChanged(object sender, PropertyChangedExtendedEventArgs e)
        {
            _logger.Trace("The value has been changed in {0}: {1} => Adding to tracking queue...", sender, e);
            TrackDetails trackDetails = new TrackDetails(sender, e.PropertyName, e.OldValue);
            _trackQueue.Add(trackDetails);

            if (_trackQueue.Count > _trackingManagerConfig.MaxSize)
            {
                _logger.Trace("Limit of the tracking queue has been overcame => removing the oldest item...");
                _trackQueue.RemoveAt(0);
                _logger.Trace("The item has been removed.");
            }

            _logger.Trace("Item has been added.");
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
