using System;
using System.Linq;
using UnityEngine;

namespace GonDraz.Events
{
    public sealed class Event<T> : IEvent
    {
        private readonly string _name;

        private Action<T> _action;

        public Event()
        {
            _name = ToString();
        }

        public Event(string name)
        {
            _name = name;
        }

        public Event(string name, params Action<T>[] actions)
        {
            foreach (var action in actions)
                if (action != null)
                    _action += action;
        }

        public void Invoke(T parameter, Action onComplete = null)
        {
            if (_action == null) return;

            foreach (var d in _action.GetInvocationList()) CallAction(d, parameter);

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

        private static bool CheckForDuplicates(Event<T> e, Action<T> newAction)
        {
            if (e._action == null || newAction == null) return false;
            if (!e._action.GetInvocationList().Contains(newAction)) return false;
            Debug.LogError(
                $"Event <color=yellow>[{e._name}]</color> : has been infected : [{newAction.Method.Name}]"
            );
            return true;

        }

        public static Event<T> operator +(Event<T> e, Action<T> newAction)
        {
            if (CheckForDuplicates(e, newAction)) return e;

            e._action += newAction;
            return e;
        }

        public static Event<T> operator -(Event<T> e, Action<T> action)
        {
            e._action -= action;
            return e;
        }
    }
}