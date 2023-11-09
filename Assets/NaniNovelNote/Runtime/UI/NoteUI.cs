using System;
using System.Collections.Generic;
using System.Linq;
using Naninovel;
using Naninovel.UI;
using UnityEngine;

namespace NaninovelNote
{
    /// <summary>
    /// Component applied to the root of inventory UI prefab.
    /// </summary>
    public class NoteUI : CustomUI
    {
        /// <summary>
        /// Represents serializable inventory state.
        /// More info about using custom state in Naninovel: https://naninovel.com/guide/state-management.html#custom-state
        /// </summary>
        [Serializable]
        private new class GameState
        {
            public Vector3 Position;
            public NoteSlot.State[] Slots;
        }

        [Tooltip("Number of slots the Note has.")]
        [SerializeField] private int capacity = 80;
        [Tooltip("Reference to the Note grid layout component.")]
        [SerializeField] private NoteGrid grid = default;
        [Tooltip("Content of the Note.")]
        [SerializeField] private RectTransform content = default;

        private NoteManager noteManager;
        private IInputManager inputManager;

        /// <summary>
        /// Returns number of items with the provided ID currently assigned
        /// to the inventory slots.
        /// </summary>
        /// <param name="pageId">Identifier of the item to count.</param>
        public int CountPage(string pageId) => grid.NoteSlots.Sum(s => !s.Empty && s.Page.Id == pageId ? s.StackCount : 0);

        /// <summary>
        /// Attempts to add an item with the provided <paramref name="pageId"/> to the first 
        /// empty inventory slot, or slot with the same item, when stack limit allows.
        /// </summary>
        /// <param name="pageId">Identifier of the item to add.</param>
        /// <param name="amount">Number of items to add.</param>
        /// <returns>Whether the item(s) were added.</returns>
        public async UniTask<bool> AddPageAsync(string pageId, int amount = 1)
        {
            // Check if we can stack the items.
            var stackSlotIndex = grid.NoteSlots.IndexOf(CanStack);
            if (stackSlotIndex >= 0) return await AddPageAtAsync(pageId, stackSlotIndex, amount);

            // In case can't stack, find first empty slot.
            var emptySlotIndex = grid.NoteSlots.IndexOf(s => s.Empty);
            if (emptySlotIndex < 0)
            {
                Debug.LogError($"Failed to add `{pageId}`: no empty slots available.");
                return false;
            }

            return await AddPageAtAsync(pageId, emptySlotIndex, amount);

            bool CanStack(NoteSlot slot)
            {
                if (slot.Empty || slot.Page.Id != pageId) return false;
                return slot.Page.StackCountLimit >= slot.StackCount + amount;
            }
        }

        /// <summary>
        /// Attempts to add an item with the provided <paramref name="pageId"/> to an inventory 
        /// slot with the provided <paramref name="slotIndex"/>.
        /// </summary>
        /// <param name="pageId">Identifier of the item to add.</param>
        /// <param name="slotIndex">Index of the inventory slot to put the item into.</param>
        /// <param name="amount">Number of items to add.</param>
        /// <returns>Whether the item(s) were added.</returns>
        public async UniTask<bool> AddPageAtAsync(string pageId, int slotIndex, int amount = 1)
        {
            if (!grid.NoteSlots.IsIndexValid(slotIndex))
            {
                Debug.LogError($"Failed to add `{pageId}` to `{slotIndex}` slot: slot with the provided ID doesn't exist.");
                return false;
            }
            var slot = grid.NoteSlots[slotIndex];

            var page = await noteManager.GetPageAsync(pageId);
            if (!ObjectUtils.IsValid(page))
            {
                Debug.LogError($"Failed to add `{pageId}` to `{slot}` slot: page with the provided ID doesn't exist.");
                return false;
            }

            if (!slot.Empty && slot.Page.Id != pageId)
            {
                Debug.LogError($"Failed to add `{pageId}` to `{slot}` slot: slot is already occupied with `{slot.Page.Id}` item.");
                return false;
            }

            if (!slot.Empty && slot.StackCount + amount > page.StackCountLimit)
            {
                Debug.LogError($"Failed to add `{pageId}` (x{amount}) to `{slot}` slot: exceeding stack count limit.");
                return false;
            }

            if (slot.Empty) slot.SetPage(page);
            slot.SetStackCount(slot.StackCount + amount);

            return true;
        }

        /// <summary>
        /// Attempts to remove item with the provided <paramref name="pageId"/> from the inventory.
        /// </summary>
        /// <param name="pageId">Identifier of the item to remove.</param>
        /// <param name="amount">Number of items to remove.</param>
        /// <returns>Whether the item was removed.</returns>
        public bool RemovePage(string pageId, int amount = 1)
        {
            while (amount > 0)
            {
                var slot = grid.NoteSlots.FirstOrDefault(s => !s.Empty && s.Page.Id == pageId);
                if (slot is null)
                {
                    Debug.LogError($"Failed to remove `{pageId}` page (x{amount}): page is not added to the inventory or not enough stacks.");
                    return false;
                }

                var removeCount = Mathf.Min(amount, slot.StackCount);
                RemovePageAt(slot.Id, removeCount);
                amount -= removeCount;
            }
            return true;
        }

        /// <summary>
        /// Attempts to remove an item from an inventory slot with the provided <paramref name="slotIndex"/>.
        /// </summary>
        /// <param name="slotIndex">Index of the inventory slot to remove item from.</param>
        /// <param name="amount">Number of items to remove.</param>
        /// <returns>Whether the item was removed.</returns>
        public bool RemovePageAt(int slotIndex, int amount = 1)
        {
            if (!grid.NoteSlots.IsIndexValid(slotIndex))
            {
                Debug.LogError($"Failed to remove an page from `{slotIndex}` slot: slot with the provided ID doesn't exist.");
                return false;
            }
            var slot = grid.NoteSlots[slotIndex];

            if (slot.Empty)
            {
                Debug.LogError($"Failed to remove an page from `{slotIndex}` slot: no page is assigned to the slot.");
                return false;
            }

            if (slot.StackCount < amount)
            {
                Debug.LogError($"Failed to remove `{slot.Page.Id}` page (x{amount}) from `{slotIndex}` slot: not enough stacks.");
                return false;
            }

            slot.SetStackCount(slot.StackCount - amount);
            if (slot.StackCount == 0) slot.SetPage(null);
            return true;
        }

        /// <summary>
        /// Removes all the items assigned to inventory slots.
        /// </summary>
        public void RemoveAllPages()
        {
            foreach (var slot in grid.NoteSlots)
                slot.SetEmpty();
        }

        /// <summary>
        /// Attempts to use an item with the provided <paramref name="itemId"/>.
        /// </summary>
        /// <param name="pageId">Identifier of the item to use.</param>
        /// <returns>Whether the item was found and used.</returns>
        public bool UsePage(string pageId)
        {
            var slot = grid.NoteSlots.FirstOrDefault(s => !s.Empty && s.Page.Id == pageId);
            if (slot is null)
            {
                Debug.LogError($"Failed to use `{pageId}` page: page doesn't exist in inventory.");
                return false;
            }

            slot.Page.Use(slot.Id);

            return true;
        }

        /// <summary>
        /// Attempts to use an item with the provided <paramref name="slotIndex"/>.
        /// </summary>
        public virtual void UsePageAtSlot(int slotIndex)
        {
            if (!grid.NoteSlots.IsIndexValid(slotIndex)) return;

            var slot = grid.NoteSlots[slotIndex];
            if (slot.Empty) return;

            slot.Page.Use(slotIndex);
        }

        protected override void Awake()
        {
            base.Awake();
            this.AssertRequiredObjects(grid, content); // make sure the required objects are assigned in the inspector

            // Store reference to the engine services -- we'll need them later.
            noteManager = Engine.GetService<NoteManager>();
            inputManager = Engine.GetService<IInputManager>();

            grid.Initialize(capacity);
            grid.OnSlotClicked += UsePageAtSlot;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            // Start listening for `ToggleInventory` input event to toggle UI's visibility.
            var toggleSampler = inputManager.GetSampler("ToggleNote");
            if (toggleSampler != null)
                toggleSampler.OnStart += ToggleVisibility;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            // Stop listening for `ToggleInventory` input event.
            var toggleSampler = inputManager?.GetSampler("ToggleNote");
            if (toggleSampler != null)
                toggleSampler.OnStart -= ToggleVisibility;
        }

        protected override void SerializeState(GameStateMap stateMap)
        {
            // Invoked when the game is saved.

            base.SerializeState(stateMap);

            // Serialize UI state.
            var state = new GameState
            {
                Position = content.transform.position,
                Slots = grid.NoteSlots.Select(s => s.GetSate()).ToArray()
            };
            stateMap.SetState(state);
        }

        protected override async UniTask DeserializeState(GameStateMap stateMap)
        {
            // Invoked when the game is loaded.

            await base.DeserializeState(stateMap);

            RemoveAllPages();

            var state = stateMap.GetState<GameState>();
            if (state is null) return; // empty state, do nothing

            // Restore UI state.
            if (state.Slots?.Length > 0)
            {
                var tasks = new List<UniTask>();
                for (int i = 0; i < state.Slots.Length; i++)
                    if (!string.IsNullOrEmpty(state.Slots[i].PageId))
                        tasks.Add(AddPageAtAsync(state.Slots[i].PageId, i, state.Slots[i].StackCount));
                await UniTask.WhenAll(tasks);
            }
            content.transform.position = state.Position;
        }
    }
}
