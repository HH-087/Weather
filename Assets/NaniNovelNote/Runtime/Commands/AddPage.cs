using Naninovel;

namespace NaninovelNote
{
    [Documentation("Adds page with the specified identifier to the inventory. When slot ID is provided, will assign item to the slot; otherwise will attempt to use first empty slot.")]
    public class AddPage : Command
    {
        [RequiredParameter, ParameterAlias(NamelessParameterAlias), Documentation("Identifier of the page to add.")]
        public StringParameter PageId;
        [Documentation("Identifier of the slot for which to assign the page.")]
        public IntegerParameter SlotId;
        [Documentation("Number of pages to add.")]
        public IntegerParameter Amount = 1;

        public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var uiManager = Engine.GetService<IUIManager>();
            var note = uiManager.GetUI<NoteUI>();

            if (Assigned(SlotId))
                await note.AddPageAtAsync(PageId, SlotId, Amount);
            else await note.AddPageAsync(PageId, Amount);
        }
    }
}
