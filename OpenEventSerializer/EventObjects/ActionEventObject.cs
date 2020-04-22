using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace OpenEventSerializer.EventObjects
{
    public class ActionEventObject : EventObject
    {
        public string Target { get; private set; }
        public object[] Arguments { get; private set; }

        internal override void LoadFromDynamic(dynamic obj)
        {
            Target = obj.target;

            Arguments = new object[obj.arguments.Length];
            for(int i = 0; i < obj.arguments.Length; i++)
                Arguments[i] = ConvertDynamicToRealType(obj.arguments[i]);
        }
    }
}