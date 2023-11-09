using Naninovel;

namespace NaninovelNote
{
    [Documentation("Uses page with the specified identifier (in case it's exist in the inventory).")]
    public class UsePage : Command
    {
        [RequiredParameter, ParameterAlias(NamelessParameterAlias), Documentation("Identifier of the page to use.")]
        public StringParameter PageId;

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var uiManager = Engine.GetService<IUIManager>();
            var note = uiManager.GetUI<NoteUI>();

            note.UsePage(PageId);

            return UniTask.CompletedTask;
        }
    }
}
