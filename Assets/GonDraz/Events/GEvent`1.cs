using System;
using System.Linq;
using UnityEngine;

namespace GonDraz.Events
{
    public sealed class GEvent<T>
    {
        private readonly string _name;

        private Action<T> _action;

        public GEvent()
        {
            _name = ToString();
        }

        public GEvent(Action<T> action)
        {
            _name = action.Method.Name;
            _action = action;
        }

        public GEvent(string name)
        {
            _name = name;
        }

        public GEvent(string name, params Action<T>[] actions)
        {
            _name = name;
            _action = null;
            foreach (var action in actions)
                if (CheckForDuplicates(this, action))
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

        private static bool CheckForDuplicates(GEvent<T> e, Action<T> newAction)
        {
            if (e._action == null || newAction == null) return false;
            if (!e._action.GetInvocationList().Contains(newAction)) return false;
            Debug.LogError(
                $"Event <color=yellow>[{e._name}]</color> : has been infected : [{newAction.Method.Name}]"
            );
            return true;
        }

        public static GEvent<T> operator +(GEvent<T> e, Action<T> newAction)
        {
            if (CheckForDuplicates(e, newAction)) return e;

            e._action += newAction;
            return e;
        }

        public static GEvent<T> operator -(GEvent<T> e, Action<T> action)
        {
            e._action -= action;
            return e;
        }

        public static implicit operator GEvent<T>(Action<T> action)
        {
            return new GEvent<T>(action);
        }
    }
}