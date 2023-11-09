using Naninovel;

namespace NaninovelNote
{
    [ExpressionFunctions]
    public static class NoteFunctions
    {
        public static int PageCount(string pageId)
        {
            var uiManager = Engine.GetService<IUIManager>();
            var note = uiManager.GetUI<NoteUI>();
            return note.CountPage(pageId);
        }

        public static bool PageExist(string pageId) => PageCount(pageId) > 0;
    }
}
