using Naninovel;
using System;
using UnityEditor;

namespace NaninovelNote
{
    public class NoteSettings : ResourcefulSettings<NoteConfiguration>
    {
        protected override Type ResourcesTypeConstraint => typeof(NotePage);
        protected override string ResourcesCategoryId => Configuration.Loader.PathPrefix;
        protected override string ResourcesSelectionTooltip => "Use `@addPage %name%` to add the page to the player note.";

        [MenuItem("Naninovel/Resources/Note")]
        private static void OpenResourcesWindow() => OpenResourcesWindowImpl();
    }
}