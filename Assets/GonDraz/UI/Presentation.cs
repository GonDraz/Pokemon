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

#if UNITY_EDITOR
        private void OnValidate()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
#endif

        public virtual void Show()
        {
            Active();
            canvasGroup.alpha = 0f;

            canvasGroup.alpha = 1f;
        }

        public virtual void Hide()
        {
            canvasGroup.alpha = 0f;
            Inactive();
        }
    }
}