///////////来源http://wiki.unity3d.com/index.php/Advanced_CSharp_Messenger////////

//#define LOG_ALL_MESSAGES
//#define LOG_ADD_LISTENER
//#define LOG_BROADCAST_MESSAGE
#define REQUIRE_LISTENER


using System;
using System.Collections.Generic;
using UnityEngine;

static internal class Messenger
{
    static public Dictionary<MessengerEventType, Delegate> eventTable = new Dictionary<MessengerEventType, Delegate>();


    static public List<MessengerEventType> permanentMessages = new List<MessengerEventType>();


    static public void MarkAsPermanent(MessengerEventType eventType)
    {
#if LOG_ALL_MESSAGES
		Debug.Log("Messenger MarkAsPermanent \t\"" + eventType + "\"");
#endif

        permanentMessages.Add(eventType);
    }


    static public void Cleanup()
    {
#if LOG_ALL_MESSAGES
		Debug.Log("MESSENGER Cleanup. Make sure that none of necessary listeners are removed.");
#endif

        List<MessengerEventType> messagesToRemove = new List<MessengerEventType>();

        foreach (KeyValuePair<MessengerEventType, Delegate> pair in eventTable)
        {
            bool wasFound = false;

            foreach (MessengerEventType message in permanentMessages)
            {
                if (pair.Key == message)
                {
                    wasFound = true;
                    break;
                }
            }

            if (!wasFound)
                messagesToRemove.Add(pair.Key);
        }

        foreach (MessengerEventType message in messagesToRemove)
        {
            eventTable.Remove(message);
        }
    }

    static public void PrintEventTable()
    {
        Debug.Log("\t\t\t=== MESSENGER PrintEventTable ===");

        foreach (KeyValuePair<MessengerEventType, Delegate> pair in eventTable)
        {
            Debug.Log("\t\t\t" + pair.Key + "\t\t" + pair.Value);
        }

        Debug.Log("\n");
    }



    static public void OnListenerAdding(MessengerEventType eventType, Delegate listenerBeingAdded)
    {
#if LOG_ALL_MESSAGES || LOG_ADD_LISTENER
		Debug.Log("MESSENGER OnListenerAdding \t\"" + eventType + "\"\t{" + listenerBeingAdded.Target + " -> " + listenerBeingAdded.Method + "}");
#endif

        if (!eventTable.ContainsKey(eventType))
        {
            eventTable.Add(eventType, null);
        }

        Delegate d = eventTable[eventType];
        if (d != null && d.GetType() != listenerBeingAdded.GetType())
        {
            throw new ListenerException(string.Format("Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}", eventType, d.GetType().Name, listenerBeingAdded.GetType().Name));
        }
    }

    static public void OnListenerRemoving(MessengerEventType eventType, Delegate listenerBeingRemoved)
    {
#if LOG_ALL_MESSAGES
		Debug.Log("MESSENGER OnListenerRemoving \t\"" + eventType + "\"\t{" + listenerBeingRemoved.Target + " -> " + listenerBeingRemoved.Method + "}");
#endif

        if (eventTable.ContainsKey(eventType))
        {
            Delegate d = eventTable[eventType];

            if (d == null)
            {
                throw new ListenerException(string.Format("Attempting to remove listener with for event type \"{0}\" but current listener is null.", eventType));
            }
            else if (d.GetType() != listenerBeingRemoved.GetType())
            {
                throw new ListenerException(string.Format("Attempting to remove listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being removed has type {2}", eventType, d.GetType().Name, listenerBeingRemoved.GetType().Name));
            }
        }
        else
        {
            throw new ListenerException(string.Format("Attempting to remove listener for type \"{0}\" but Messenger doesn't know about this event type.", eventType));
        }
    }

    static public void OnListenerRemoved(MessengerEventType eventType)
    {
        if (eventTable[eventType] == null)
        {
            eventTable.Remove(eventType);
        }
    }

    static public void OnBroadcasting(MessengerEventType eventType)
    {
#if REQUIRE_LISTENER
        if (!eventTable.ContainsKey(eventType))
        {
            throw new BroadcastException(string.Format("Broadcasting message \"{0}\" but no listener found. Try marking the message with Messenger.MarkAsPermanent.", eventType));
        }
#endif
    }

    static public BroadcastException CreateBroadcastSignatureException(MessengerEventType eventType)
    {
        return new BroadcastException(string.Format("Broadcasting message \"{0}\" but listeners have a different signature than the broadcaster.", eventType));
    }

    public class BroadcastException : Exception
    {
        public BroadcastException(string msg)
            : base(msg)
        {
        }
    }

    public class ListenerException : Exception
    {
        public ListenerException(string msg)
            : base(msg)
        {
        }
    }



    //No parameters
    static public void AddListener(MessengerEventType eventType, MessengerCallback handler)
    {
        OnListenerAdding(eventType, handler);
        eventTable[eventType] = (MessengerCallback)eventTable[eventType] + handler;
    }

    //Single parameter
    static public void AddListener<T>(MessengerEventType eventType, MessengerCallback<T> handler)
    {
        OnListenerAdding(eventType, handler);
        eventTable[eventType] = (MessengerCallback<T>)eventTable[eventType] + handler;
    }

    //Two parameters
    static public void AddListener<T, U>(MessengerEventType eventType, MessengerCallback<T, U> handler)
    {
        OnListenerAdding(eventType, handler);
        eventTable[eventType] = (MessengerCallback<T, U>)eventTable[eventType] + handler;
    }

    //Three parameters
    static public void AddListener<T, U, V>(MessengerEventType eventType, MessengerCallback<T, U, V> handler)
    {
        OnListenerAdding(eventType, handler);
        eventTable[eventType] = (MessengerCallback<T, U, V>)eventTable[eventType] + handler;
    }



    //No parameters
    static public void RemoveListener(MessengerEventType eventType, MessengerCallback handler)
    {
        OnListenerRemoving(eventType, handler);
        eventTable[eventType] = (MessengerCallback)eventTable[eventType] - handler;
        OnListenerRemoved(eventType);
    }

    //Single parameter
    static public void RemoveListener<T>(MessengerEventType eventType, MessengerCallback<T> handler)
    {
        OnListenerRemoving(eventType, handler);
        eventTable[eventType] = (MessengerCallback<T>)eventTable[eventType] - handler;
        OnListenerRemoved(eventType);
    }

    //Two parameters
    static public void RemoveListener<T, U>(MessengerEventType eventType, MessengerCallback<T, U> handler)
    {
        OnListenerRemoving(eventType, handler);
        eventTable[eventType] = (MessengerCallback<T, U>)eventTable[eventType] - handler;
        OnListenerRemoved(eventType);
    }

    //Three parameters
    static public void RemoveListener<T, U, V>(MessengerEventType eventType, MessengerCallback<T, U, V> handler)
    {
        OnListenerRemoving(eventType, handler);
        eventTable[eventType] = (MessengerCallback<T, U, V>)eventTable[eventType] - handler;
        OnListenerRemoved(eventType);
    }

    //No parameters
    static public void Broadcast(MessengerEventType eventType)
    {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
        OnBroadcasting(eventType);

        Delegate d;
        if (eventTable.TryGetValue(eventType, out d))
        {
            MessengerCallback MessengerCallback = d as MessengerCallback;

            if (MessengerCallback != null)
            {
                MessengerCallback();
            }
            else
            {
                throw CreateBroadcastSignatureException(eventType);
            }
        }
    }

    //Single parameter
    static public void Broadcast<T>(MessengerEventType eventType, T arg1)
    {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
        OnBroadcasting(eventType);

        Delegate d;
        if (eventTable.TryGetValue(eventType, out d))
        {
            MessengerCallback<T> MessengerCallback = d as MessengerCallback<T>;

            if (MessengerCallback != null)
            {
                MessengerCallback(arg1);
            }
            else
            {
                throw CreateBroadcastSignatureException(eventType);
            }
        }
    }

    //Two parameters
    static public void Broadcast<T, U>(MessengerEventType eventType, T arg1, U arg2)
    {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
        OnBroadcasting(eventType);

        Delegate d;
        if (eventTable.TryGetValue(eventType, out d))
        {
            MessengerCallback<T, U> MessengerCallback = d as MessengerCallback<T, U>;

            if (MessengerCallback != null)
            {
                MessengerCallback(arg1, arg2);
            }
            else
            {
                throw CreateBroadcastSignatureException(eventType);
            }
        }
    }

    //Three parameters
    static public void Broadcast<T, U, V>(MessengerEventType eventType, T arg1, U arg2, V arg3)
    {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
        OnBroadcasting(eventType);

        Delegate d;
        if (eventTable.TryGetValue(eventType, out d))
        {
            MessengerCallback<T, U, V> MessengerCallback = d as MessengerCallback<T, U, V>;

            if (MessengerCallback != null)
            {
                MessengerCallback(arg1, arg2, arg3);
            }
            else
            {
                throw CreateBroadcastSignatureException(eventType);
            }
        }
    }
}
