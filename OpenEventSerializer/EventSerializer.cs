using System.ComponentModel;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OpenEventSerializer.EventObjects;

namespace OpenEventSerializer
{
    public class EventSerializer
    {
        private Dictionary<string, Delegate> _actions = new Dictionary<string, Delegate>();
        private Dictionary<string, EventScript> _scripts = new Dictionary<string, EventScript>();

        public void AddAction(string name, Delegate action)
        {
            _actions.Add(name, action);
        }

        public void AddAction<T>(string name, Action<T> action) => AddAction(name, action as Delegate);

        public void CallAction(string name, params object[] args)
        {
            Delegate action = _actions[name];

            action.DynamicInvoke(args);
        }

        public void LoadScript(string name, string json)
        {
            using(TextReader reader = new StringReader(json))
            {
                EventScript script = new EventScript();
                script.Load(reader);
                _scripts.Add(name, script);
            }
        }

        public EventObjectEnumerator StartScript(string name)
            => new EventObjectEnumerator(this, _scripts[name]);

        public void Handle(EventObjectEnumerator enumerator)
        {
            switch(enumerator.Current)
            {
                case ActionEventObject actionEvent:
                    CallAction(actionEvent.Target, actionEvent.Arguments);
                    break;
                case JumpEventObject jumpEvent:
                    EventScript script = (jumpEvent.Script != null ? _scripts[jumpEvent.Script] : enumerator.ActiveScript);
                    int index = script.Objects.FindIndex(o => o.Id == jumpEvent.Target);
                    if(index == -1)
                    {
                        throw new ArgumentException($"Invalid jump target {jumpEvent.Target}");
                    }
                    enumerator.ActiveScript = script;
                    // subtract one because you will be calling the .MoveNext method
                    // At the start of the next iteration
                    enumerator.MoveTo(index - 1); 
                    break;
                case EndEventObject endEvent:
                    enumerator.MoveTo(enumerator.ActiveScript.Objects.Count);
                    break;
                case NoneEventObject noneEvent:
                    break;
                default:
                    throw new Exception($"Unknown event object {enumerator.Current.GetType()}");
            }
        }
    }
}
