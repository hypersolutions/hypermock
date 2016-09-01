using System;
using System.Linq;

namespace HyperMock.Core
{
    internal class EventDispatcher<T>
    {
        private readonly Mock<T> _mock;

        internal EventDispatcher(Mock<T> mock)
        {
            _mock = mock;
        }

        internal void Raise<TArgs>(Action<T> expression, TArgs args) where TArgs : EventArgs
        {
            // Raise the event with the empty handler 
            expression(_mock.Object);

            var recordedVisits = _mock.Dispatcher.Visits.RecordedVisits;

            // Capture the empty handler record - this will give us the event name
            var lastEventRecord = recordedVisits.Last();

            // Find the event record (if exists) for the event with an attached handler
            var eventRecord = recordedVisits.FirstOrDefault(
                v => v.Name == lastEventRecord.Name && v.Args != null && v.Args.Length == 1);

            var eventHandler = eventRecord?.Args[0] as Delegate;

            // No event handler attached (or found) don't bother continuing
            if (eventHandler == null) return;

            // Run through each attached handler and invoke the event
            foreach (var attachedHandler in eventHandler.GetInvocationList())
            {
                attachedHandler.DynamicInvoke(_mock.Object, args);
            }
        }
    }
}
