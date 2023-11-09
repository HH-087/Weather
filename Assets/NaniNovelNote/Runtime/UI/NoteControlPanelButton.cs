using Naninovel;

namespace NaninovelNote
{
    public class InventoryControlPanelButton : ScriptableLabeledButton
    {
        private NoteUI note;

        protected override void Awake()
        {
            base.Awake();

            note = Engine.GetService<IUIManager>().GetUI<NoteUI>();
        }

        protected override void OnButtonClick() => note.ToggleVisibility();
    }
}
