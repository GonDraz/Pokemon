using System;
using System.Linq;
using UnityEngine;

namespace GonDraz.Events
{
    public sealed class GEvent
    {
        private readonly string _name;

        private Action _action;

        public GEvent()
        {
            _name = ToString();
        }

        public GEvent(Action action)
        {
            _name = action.Method.Name;
            _action = action;
        }

        public GEvent(string name)
        {
            _name = name;
        }

        public GEvent(string name, params Action[] actions)
        {
            _name = name;
            _action = null;
            foreach (var action in actions)
                if (CheckForDuplicates(this, action))
                    _action += action;
        }

        public void Invoke(Action onComplete = null)
        {
            if (_action == null) return;

            foreach (var d in _action.GetInvocationList()) CallAction(d);

            onComplete?.Invoke();
        } // ReSharper disable Unity.PerformanceAnalysis
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

        private static bool CheckForDuplicates(GEvent e, Action newAction)
        {
            if (e._action == null || newAction == null) return false;
            if (!e._action.GetInvocationList().Contains(newAction)) return false;
            Debug.LogError(
                $"Event <color=yellow>[{e._name}]</color> : has been infected : <color=red>[{newAction.Method.Name}]</color>"
            );
            return true;
        }

        public static GEvent operator +(GEvent e, Action newAction)
        {
            if (CheckForDuplicates(e, newAction)) return e;

            e._action += newAction;
            return e;
        }

        public static GEvent operator -(GEvent e, Action action)
        {
            e._action -= action;
            return e;
        }

        public static implicit operator GEvent(Action action)
        {
            return new GEvent(action);
        }

        public static implicit operator Action(GEvent e)
        {
            Action action = null;
            if (e._action != null) action += e._action;
            return action;
        }
    }
}