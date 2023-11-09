using System;
using Naninovel;

namespace NaninovelNote
{
 
    public class NoteSlot
    {

        [Serializable]
        public struct State
        {
            public string PageId;
            public int StackCount;
        }


        public event Action<NotePage> OnPageChanged;

        public event Action<int> OnStackCountChanged;


        public int Id { get; }

        public NotePage Page { get; private set; }

        public int StackCount { get; private set; }

        public bool Empty => !ObjectUtils.IsValid(Page);

        public NoteSlot(int id)
        {
            Id = id;
        }

        public State GetSate() => new State
        {
            PageId = Empty ? null : Page.Id,
            StackCount = StackCount
        };


        public void SetEmpty()
        {
            SetPage(null);
            SetStackCount(0);
        }


        public void SetPage(NotePage page)
        {
            Page = page;
            OnPageChanged?.Invoke(page);
        }


        public void SetStackCount(int stackCount)
        {
            StackCount = stackCount;
            OnStackCountChanged?.Invoke(stackCount);
        }
    }
}
