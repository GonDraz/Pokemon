using System;
using System.Linq;
using UnityEngine;

namespace GonDraz.Events
{
    public sealed class GEvent<T1, T2, T3, T4>
    {
        private readonly string _name;

        private Action<T1, T2, T3, T4> _action;

        public GEvent()
        {
            _name = ToString();
        }

        public GEvent(Action<T1, T2, T3, T4> action)
        {
            _name = action.Method.Name;
            _action = action;
        }

        public GEvent(string name)
        {
            _name = name;
        }

        public GEvent(string name, params Action<T1, T2, T3, T4>[] actions)
        {
            _name = name;
            _action = null;
            foreach (var action in actions)
                if (CheckForDuplicates(this, action))
                    _action += action;
        }

        public void Invoke(T1 parameter1, T2 parameter2, T3 parameter3, T4 parameter4, Action onComplete = null)
        {
            if (_action == null) return;

            foreach (var d in _action.GetInvocationList())
                CallAction(d, parameter1, parameter2, parameter3, parameter4);

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
                    $"Event <color=yellow>[{_name}]</color> : has been infected : <color=red>[{action.Method.Name}]</color>\n" +
                    $"Exception: {e.Message}\nStackTrace: {e.StackTrace}"
                );
            }
        }

        private static bool CheckForDuplicates(GEvent<T1, T2, T3, T4> e, Action<T1, T2, T3, T4> newAction)
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

        public static GEvent<T1, T2, T3, T4> operator +(GEvent<T1, T2, T3, T4> e, Action<T1, T2, T3, T4> newAction)
        {
            e ??= new GEvent<T1, T2, T3, T4>();

            if (CheckForDuplicates(e, newAction)) return e;

            e._action += newAction;
            return e;
        }

        public static GEvent<T1, T2, T3, T4> operator -(GEvent<T1, T2, T3, T4> e, Action<T1, T2, T3, T4> action)
        {
            e ??= new GEvent<T1, T2, T3, T4>();

            e._action -= action;
            return e;
        }

        public static implicit operator GEvent<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action)
        {
            return new GEvent<T1, T2, T3, T4>(action);
        }

        public static implicit operator Action<T1, T2, T3, T4>(GEvent<T1, T2, T3, T4> e)
        {
            e ??= new GEvent<T1, T2, T3, T4>();

            Action<T1, T2, T3, T4> action = null;
            if (e._action != null) action = e._action;
            return action;
        }
    }
}