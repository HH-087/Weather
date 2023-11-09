using Naninovel;

namespace NaninovelNote
{
    /// <summary>
    /// Attached to the game object that hosts button to close (hide) inventory UI.
    /// </summary>
    public class NoteCloseButton : ScriptableLabeledButton
    {
        private NoteUI noteUI;

        protected override void Awake()
        {
            base.Awake();

            noteUI = GetComponentInParent<NoteUI>();
        }

        protected override void OnButtonClick() => noteUI.Hide();
    }
}
