using Naninovel;

namespace NaninovelNote
{
    [Documentation("Removes all page in the inventory.")]
    public class RemoveAllPages : Command
    {
        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var uiManager = Engine.GetService<IUIManager>();
            var note = uiManager.GetUI<NoteUI>();

            note.RemoveAllPages();

            return UniTask.CompletedTask;
        }
    }
}
