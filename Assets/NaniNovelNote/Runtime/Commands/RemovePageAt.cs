using Naninovel;

namespace NaninovelNote
{
    [Documentation("Removes page from an inventory slot with the specified identifier.")]
    public class RemovePageAt : Command
    {
        [RequiredParameter, ParameterAlias(NamelessParameterAlias), Documentation("Identifier of inventory slot to remove item from.")]
        public IntegerParameter SlotId;
        [Documentation("Number of pages to remove.")]
        public IntegerParameter Amount = 1;

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var uiManager = Engine.GetService<IUIManager>();
            var note = uiManager.GetUI<NoteUI>();

            note.RemovePageAt(SlotId, Amount);

            return UniTask.CompletedTask;
        }
    }
}
