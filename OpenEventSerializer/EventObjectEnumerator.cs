using System.Collections.Generic;
using OpenEventSerializer.EventObjects;
using System.Collections;

namespace OpenEventSerializer
{
    public class EventObjectEnumerator : IEnumerator<EventObject>
    {
        public EventObject Current => ActiveScript.Objects[objectIndex];
        public EventScript ActiveScript { get; set; }

        private EventSerializer _eventSerializer;
        // Start at -1 because we will be calling MoveNext before starting the iteration
        private int objectIndex = -1;

        public EventObjectEnumerator(EventSerializer eventSerializer, EventScript script)
        {
            _eventSerializer = eventSerializer;
            ActiveScript = script;
        }

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if(objectIndex < ActiveScript.Objects.Count - 1)
            {
                objectIndex++;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void MoveTo(int index)
        {
            objectIndex = index;
        }

        public void Reset()
        {
            objectIndex = 0;
        }

        public void Dispose()
        {
            
        }

        public override string ToString()
            => $"EventObject Enumerator, Current: {Current}, Script: {ActiveScript}, Index: {objectIndex}";
    }
}
