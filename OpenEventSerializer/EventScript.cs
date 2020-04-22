using System.IO;
using Jil;
using System.Collections.Generic;
using System;
using OpenEventSerializer.EventObjects;

namespace OpenEventSerializer
{
    public class EventScript
    {
        public List<EventObject> Objects { get; private set; } = new List<EventObject>();

        public void Load(TextReader reader)
        {
            dynamic root = JSON.DeserializeDynamic(reader);
            foreach(dynamic obj in root)
            {
                EventObject eventObject = EventObject.Load(obj);
                Objects.Add(eventObject);
            }
        }
    }
}