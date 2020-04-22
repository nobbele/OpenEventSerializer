using System;

namespace OpenEventSerializer.EventObjects
{
    public class JumpEventObject : EventObject
    {
        public string Target { get; private set; }
        public string Script { get; private set; }

        internal override void LoadFromDynamic(dynamic obj)
        {
            Target = obj.target;
            Script = obj.script;
        }
    }
}