using System;
using PrimeTween;
using UnityEngine;

namespace GonDraz.UI
{
    [DisallowMultipleComponent]
    [SelectionBase]
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class Presentation : Base
    {
        [SerializeField] private CanvasGroup canvasGroup;

        [SerializeField] [Range(0, 4)] private float showDuration = 0.25f;
        [SerializeField] [Range(0, 4)] private float hideDuration = 0.125f;

        protected Sequence Sequence;

#if UNITY_EDITOR
        private void OnValidate()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
#endif

        private void CreateSequence()
        {
            Sequence.Stop();
            Sequence = Sequence.Create();
        }

        protected override bool SubscribeUsingOnEnable()
        {
            return true;
        }

        protected override bool UnsubscribeUsingOnDisable()
        {
            return true;
        }
        
        public virtual void Show()
        {
            Active();
            CreateSequence();
            ChangeAlphaCanvasGroup(1f, showDuration);
        }

        public virtual void Hide()
        {
            CreateSequence();
            ChangeAlphaCanvasGroup(0f, hideDuration, Inactive);
        }

        private void ChangeAlphaCanvasGroup(float to, float duration, Action callback = null)
        {
            if (!Mathf.Approximately(canvasGroup.alpha, to)) Sequence.Chain(Tween.Alpha(canvasGroup, to, duration));
            if (callback != null) Sequence.OnComplete(callback);
        }
    }
}