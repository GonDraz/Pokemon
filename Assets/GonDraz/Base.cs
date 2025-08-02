using GonDraz.Interface;
using UnityEngine;

namespace GonDraz
{
    public abstract class Base : MonoBehaviour, ISubscribable
    {
        private bool _isSubscribe;

        protected virtual void OnEnable()
        {
            if (!SubscribeUsingOnEnable() || _isSubscribe) return;
            Subscribe();
        }

        protected virtual void OnDisable()
        {
            if (!UnsubscribeUsingOnDisable() || !_isSubscribe) return;
            Unsubscribe();
        }

        protected virtual void OnDestroy()
        {
            Unsubscribe();
        }

        public virtual void Subscribe()
        {
            if (_isSubscribe) return;
            _isSubscribe = true;
        }

        public virtual void Unsubscribe()
        {
            if (!_isSubscribe) return;
            _isSubscribe = false;
        }

        protected virtual bool SubscribeUsingOnEnable()
        {
            return false;
        }

        protected virtual bool UnsubscribeUsingOnDisable()
        {
            return false;
        }

        protected virtual void SubscribesChild()
        {
            SubscribesChild<Base>();
        }

        protected virtual void SubscribesChild<T>() where T : Base
        {
            var childArray = GetComponentsInChildren<T>();

            foreach (var child in childArray) child.Subscribe();
        }

        protected virtual void UnsubscribeChild()
        {
            UnsubscribeChild<Base>();
        }

        protected virtual void UnsubscribeChild<T>() where T : Base
        {
            var childArray = GetComponentsInChildren<T>();

            foreach (var child in childArray) child.Unsubscribe();
        }

        protected virtual void Active()
        {
            gameObject.SetActive(true);
        }

        protected virtual void Inactive()
        {
            gameObject.SetActive(false);
        }
    }
}