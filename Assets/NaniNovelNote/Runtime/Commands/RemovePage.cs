using Naninovel;

namespace NaninovelNote
{
    [Documentation("Removes page with the specified identifier from the inventory.")]
    public class RemovePage : Command
    {
        [RequiredParameter, ParameterAlias(NamelessParameterAlias), Documentation("Identifier of the page to remove.")]
        public StringParameter PageId;
        [Documentation("Number of pages to remove.")]
        public IntegerParameter Amount = 1;

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var uiManager = Engine.GetService<IUIManager>();
            var note = uiManager.GetUI<NoteUI>();

            note.RemovePage(PageId, Amount);

            return UniTask.CompletedTask;
        }
    }
}
