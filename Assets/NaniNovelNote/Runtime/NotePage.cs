using UnityEngine;
using UnityEngine.Events;

namespace NaninovelNote
{
    public class NotePage : MonoBehaviour
    {
        [System.Serializable]
        public class PageUsedEvent : UnityEvent<int> { }

        public string Id => name;
        public int StackCountLimit => stackCountLimit;

        [Tooltip("How many pages of this type can be stacked on a single Note slot.")]
        [SerializeField] private int stackCountLimit = 1;
        [Tooltip("Invoked when the page is used; returns slot index of the used page.")]
        [SerializeField] private PageUsedEvent onPageUsed = default;

        /// <summary>
        /// Use the item. Override the method in inherited types 
        /// or use <see cref="onPageUsed"/> event for item-specific effects.
        /// </summary>
        /// <param name="slotIndex">Index of the slot the item was used from.</param>
        public virtual void Use(int slotIndex)
        {
            onPageUsed?.Invoke(slotIndex);
        }
    }
}
