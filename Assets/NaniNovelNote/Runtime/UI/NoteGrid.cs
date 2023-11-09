using System;
using System.Collections.Generic;
using Naninovel;

namespace NaninovelNote
{
    /// <summary>
    /// Attached to the game object, that hosts <see cref="UnityEngine.UI.GridLayoutGroup"/>.
    /// Provides several utility methods to work with the grid layout (via <see cref="ScriptableGrid{TSlot}"/>).
    /// </summary>
    public class NoteGrid : ScriptableGrid<NoteGridSlot>
    {
        public Action<int> OnSlotClicked;

        public IReadOnlyList<NoteSlot> NoteSlots { get; private set; }

        public override void Initialize(int pagesCount)
        {
            var slots = new NoteSlot[pagesCount];
            for (int i = 0; i < pagesCount; i++)
                slots[i] = new NoteSlot(i);
            NoteSlots = slots;

            base.Initialize(pagesCount);
        }

        protected override void InitializeSlot(NoteGridSlot gridSlot)
        {
            gridSlot.OnButtonClicked += () => OnSlotClicked?.Invoke(gridSlot.BoundSlot?.Id ?? -1);
        }

        protected override void BindSlot(NoteGridSlot gridSlot, int pageIndex)
        {
            var noteSlot = NoteSlots[pageIndex];
            gridSlot.Bind(noteSlot);
        }
    }
}
