using System;
using System.Linq;
using UnityEngine;

namespace GonDraz.Events
{
    public class Event<T1, T2, T3, T4, T5> : IEvent
    {
        private readonly string _name;

        private Action<T1, T2, T3, T4, T5> _action;

        public Event()
        {
            _name = ToString();
        }

        public Event(string name)
        {
            _name = name;
        }

        public Event(string name, params Action<T1, T2, T3, T4, T5>[] actions)
        {
            foreach (var action in actions)
                if (action != null)
                    _action += action;
        }

        public void Invoke(T1 parameter1, T2 parameter2, T3 parameter3, T4 parameter4, T5 parameter5,
            Action onComplete = null)
        {
            if (_action == null) return;

            foreach (var d in _action.GetInvocationList())
                CallAction(d, parameter1, parameter2, parameter3, parameter4, parameter5);

            onComplete?.Invoke();
        }

        private void CallAction(Delegate action, params object[] parameters)
        {
            if (action == null) return;
            try
            {
                action.DynamicInvoke(parameters);
            }
            catch (Exception e)
            {
                Debug.LogError(
                    $"<color=yellow>Event[{_name}]</color> : error : [{action.Method.Name}] => {e}"
                );
            }
        }

        private static bool CheckForDuplicates(Event<T1, T2, T3, T4, T5> e, Action<T1, T2, T3, T4, T5> newAction)
        {
            if (e._action != null && newAction != null)
                if (e._action.GetInvocationList().Contains(newAction))
                {
                    Debug.LogError(
                        $"Event <color=yellow>[{e._name}]</color> : has been infected : [{newAction.Method.Name}]"
                    );
                    return true;
                }

            return false;
        }

        public static Event<T1, T2, T3, T4, T5> operator +(Event<T1, T2, T3, T4, T5> e,
            Action<T1, T2, T3, T4, T5> newAction)
        {
            if (CheckForDuplicates(e, newAction)) return e;

            e._action += newAction;
            return e;
        }

        public static Event<T1, T2, T3, T4, T5> operator -(Event<T1, T2, T3, T4, T5> e,
            Action<T1, T2, T3, T4, T5> action)
        {
            e._action -= action;
            return e;
        }
    }
}