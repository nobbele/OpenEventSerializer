using System;
namespace OpenEventSerializer.EventObjects
{
    [Serializable]
    public class UnknownEventObjectTypeException :  Exception
    {
        public UnknownEventObjectTypeException() { }
        public UnknownEventObjectTypeException(string message) : base(message) { }
        public UnknownEventObjectTypeException(string message, System.Exception inner) : base(message, inner) { }
        protected UnknownEventObjectTypeException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    public abstract class EventObject
    {
        public string Type { get; private set; }
        public string Id { get; private set; }

        internal static object ConvertDynamicToRealType(dynamic dyn)
        {
            object obj = null;
            if((obj = (string)dyn) != null)
                return obj;
            else if((obj = (int?)dyn) != null)
                return obj;
            else if((obj = (float?)dyn) != null)
                return obj;
            else
                return obj;
        }

        internal abstract void LoadFromDynamic(dynamic obj);

        public static EventObject Load(dynamic obj)
        {
            EventObject eventObject = null;

            string type = obj.type;

            if(type == null || type == "none")
                eventObject = new NoneEventObject();
            else
            {
                switch(type)
                {
                    case "action":
                        eventObject = new ActionEventObject();
                        break;
                    case "jump":
                        eventObject = new JumpEventObject();
                        break;
                    case "end":
                        eventObject = new EndEventObject();
                        break;
                    default:
                        throw new UnknownEventObjectTypeException($"Invalid event object type {type}");
                }
            }

            eventObject.Id = obj.id;
            eventObject.Type = obj.type;

            eventObject.LoadFromDynamic(obj);

            return eventObject;
        }
    }
}