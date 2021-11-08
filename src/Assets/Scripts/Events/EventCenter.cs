using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public enum EventType
    {
        RunLeft,
        LeftBtnUp,
        RunRight,
        RightBtnUp,
        Jump,
        Interactive,
        InteractiveUp,
        Skill,
        SkillUp,
        Push,
        CupBlowUp,
        KnifeStop,
        Attach,
        TouchWater,
        Dive,
        FrezeeAll,
        FrezeeAttack,
        FrezeeInteractive,
        TurnWaterFaucetOn,
        TurnWaterFaucetOff,
        WaterLeak,
        FocusOn,
        Facing,
        DivingInPool,
        AnDeath,
        AnRespawn,
        PankapuDeath,
        PankapuRespawn,
        ZoomOut,
        BossCamera
    }


    public delegate void EventCallBack();
    public delegate void EventCallBack<T>(T arg);
    public delegate void EventCallBack<T, W>(T arg1, W arg2);
    public delegate void EventCallBack<T, W, E>(T arg1, W arg2, E arg3);
    public delegate void EventCallBack<T, W, E, R>(T arg1, W arg2, E arg3, R arg4);
    public delegate void EventCallBack<T, W, E, R, Y>(T arg1, W arg2, E arg3, R arg4, Y arg5);
    public class EventCenter
    {
        private static Dictionary<EventType, Delegate> eventTable = new Dictionary<EventType, Delegate>();

        static void AddListenerTemplate(EventType eventType, Delegate callBack)
        {
            if (!eventTable.ContainsKey(eventType))
            {
                eventTable.Add(eventType, null);
            }

            Delegate d = eventTable[eventType];

            if (d != null && d.GetType() != callBack.GetType())
            {
                throw new Exception(string.Format("Intend to add different type of delegate to {0} event." +
                    "current event's delegate is {1}，the delegate being added is {2}",
                    eventType, d.GetType(), callBack.GetType()));
            }
        }

        static void RemoveListenerTemplate(EventType eventType, Delegate callBack)
        {
            if (eventTable.ContainsKey(eventType))
            {
                Delegate d = eventTable[eventType];
                if (d == null)
                {
                    throw new Exception(string.Format("Error on removing listener: event {0} doesn't have delegate", eventType));
                }
                else if (d.GetType() != callBack.GetType())
                {
                    throw new Exception(string.Format("Error on removing listener: attend to remove a different type of delegate from event {0}." +
                        "Current type of delegate is {}. The object being removed is {2}", eventType, d, callBack));
                }
                
            }
            else
            {
                throw new Exception(string.Format("Error on removing listener: no event code {0}", eventType));
            }
        }

        static void OnListenerRemove(EventType eventType)
        {
            if (eventTable[eventType] == null)
            {
                eventTable.Remove(eventType);
            }
        }

        /// <summary>
        /// Add event
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public static void AddListener(EventType eventType, EventCallBack callBack)
        {
            AddListenerTemplate(eventType, callBack);
            eventTable[eventType] = (EventCallBack)eventTable[eventType] + callBack;
        }

        public static void AddListener<T>(EventType eventType, EventCallBack<T> callBack)
        {
            AddListenerTemplate(eventType, callBack);
            eventTable[eventType] = (EventCallBack<T>)eventTable[eventType] + callBack;
        }

        public static void AddListener<T, W>(EventType eventType, EventCallBack<T, W> callBack)
        {
            AddListenerTemplate(eventType, callBack);
            eventTable[eventType] = (EventCallBack<T, W>)eventTable[eventType] + callBack;
        }

        public static void AddListener<T, W, E>(EventType eventType, EventCallBack<T, W, E> callBack)
        {
            AddListenerTemplate(eventType, callBack);
            eventTable[eventType] = (EventCallBack<T, W, E>)eventTable[eventType] + callBack;
        }

        public static void AddListener<T, W, E, R>(EventType eventType, EventCallBack<T, W, E, R> callBack)
        {
            AddListenerTemplate(eventType, callBack);
            eventTable[eventType] = (EventCallBack<T, W, E, R>)eventTable[eventType] + callBack;
        }

        public static void AddListener<T, W, E, R, Y>(EventType eventType, EventCallBack<T, W, E, R, Y> callBack)
        {
            AddListenerTemplate(eventType, callBack);
            eventTable[eventType] = (EventCallBack<T, W, E, R, Y>)eventTable[eventType] + callBack;
        }

        /// <summary>
        /// Remove event
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public static void RemoveListener(EventType eventType, EventCallBack callBack)
        {
            RemoveListenerTemplate(eventType, callBack);
            eventTable[eventType] = (EventCallBack)eventTable[eventType] - callBack;
            OnListenerRemove(eventType);
        }

        public static void RemoveListener<T>(EventType eventType, EventCallBack<T> callBack)
        {
            RemoveListenerTemplate(eventType, callBack);
            eventTable[eventType] = (EventCallBack<T>)eventTable[eventType] - callBack;
            OnListenerRemove(eventType);
        }

        public static void RemoveListener<T, W>(EventType eventType, EventCallBack<T, W> callBack)
        {
            RemoveListenerTemplate(eventType, callBack);
            eventTable[eventType] = (EventCallBack<T, W>)eventTable[eventType] - callBack;
            OnListenerRemove(eventType);
        }

        public static void RemoveListener<T, W, E>(EventType eventType, EventCallBack<T, W, E> callBack)
        {
            RemoveListenerTemplate(eventType, callBack);
            eventTable[eventType] = (EventCallBack<T, W, E>)eventTable[eventType] - callBack;
            OnListenerRemove(eventType);
        }

        public static void RemoveListener<T, W, E, R>(EventType eventType, EventCallBack<T, W, E, R> callBack)
        {
            RemoveListenerTemplate(eventType, callBack);
            eventTable[eventType] = (EventCallBack<T, W, E, R>)eventTable[eventType] - callBack;
            OnListenerRemove(eventType);
        }

        public static void RemoveListener<T, W, E, R, Y>(EventType eventType, EventCallBack<T, W, E, R, Y> callBack)
        {
            RemoveListenerTemplate(eventType, callBack);
            eventTable[eventType] = (EventCallBack<T, W, E, R, Y>)eventTable[eventType] - callBack;
            OnListenerRemove(eventType);
        }

        /// <summary>
        /// Broadcast event
        /// </summary>
        /// <param name="eventType"></param>
        public static void Braodcast(EventType eventType)
        {
            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                EventCallBack callBack = d as EventCallBack;
                if (callBack != null)
                {
                    callBack();
                }
                else
                {
                    throw new Exception(string.Format("Error on broadcasting event: event {0} has a different type of delegate", eventType));
                }
            }
        }

        public static void Braodcast<T>(EventType eventType, T arg)
        {
            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                EventCallBack<T> callBack = d as EventCallBack<T>;
                if (callBack != null)
                {
                    callBack(arg);
                }
                else
                {
                    throw new Exception(string.Format("Error on broadcasting event: event {0} has a different type of delegate", eventType));
                }
            }
        }

        public static void Braodcast<T, W>(EventType eventType, T arg1, W arg2)
        {
            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                EventCallBack<T, W> callBack = d as EventCallBack<T, W>;
                if (callBack != null)
                {
                    callBack(arg1, arg2);
                }
                else
                {
                    throw new Exception(string.Format("Error on broadcasting event: event {0} has a different type of delegate", eventType));
                }
            }
        }

        public static void Braodcast<T, W, E>(EventType eventType, T arg1, W arg2, E arg3)
        {
            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                EventCallBack<T, W, E> callBack = d as EventCallBack<T, W, E>;
                if (callBack != null)
                {
                    callBack(arg1, arg2, arg3);
                }
                else
                {
                    throw new Exception(string.Format("Error on broadcasting event: event {0} has a different type of delegate", eventType));
                }
            }
        }

        public static void Braodcast<T, W, E, R>(EventType eventType, T arg1, W arg2, E arg3, R arg4)
        {
            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                EventCallBack<T, W, E, R> callBack = d as EventCallBack<T, W, E, R>;
                if (callBack != null)
                {
                    callBack(arg1, arg2, arg3, arg4);
                }
                else
                {
                    throw new Exception(string.Format("Error on broadcasting event: event {0} has a different type of delegate", eventType));
                }
            }
        }

        public static void Braodcast<T, W, E, R, Y>(EventType eventType, T arg1, W arg2, E arg3, R arg4, Y arg5)
        {
            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                EventCallBack<T, W, E, R, Y> callBack = d as EventCallBack<T, W, E, R, Y>;
                if (callBack != null)
                {
                    callBack(arg1, arg2, arg3, arg4, arg5);
                }
                else
                {
                    throw new Exception(string.Format("Error on broadcasting event: event {0} has a different type of delegate", eventType));
                }
            }
        }
    }
}
