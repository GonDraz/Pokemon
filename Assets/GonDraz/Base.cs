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
            _isSubscribe = true;
            Subscribe();
        }

        protected virtual void OnDisable()
        {
            if (!UnsubscribeUsingOnDisable() || !_isSubscribe) return;
            _isSubscribe = false;
            Unsubscribe();
        }

        protected virtual void OnDestroy()
        {
            _isSubscribe = false;
            Unsubscribe();
        }

        public virtual void Subscribe()
        {
            _isSubscribe = true;
        }

        public virtual void Unsubscribe()
        {
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
            var childArray = GetComponentsInChildren<Base>();

            foreach (var child in childArray) child.Subscribe();
        }

        protected virtual void SubscribesChild<T>() where T : Base
        {
            var childArray = GetComponentsInChildren<Base>();

            foreach (var child in childArray) child.Subscribe();
        }

        protected virtual void UnsubscribeChild()
        {
            var childArray = GetComponentsInChildren<Base>();

            foreach (var child in childArray) child.Unsubscribe();
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