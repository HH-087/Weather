using Naninovel;
using UnityEngine;

namespace NaninovelNote
{

    [EditInProjectSettings]
    public class NoteConfiguration : Configuration
    {

        public const string DefaultPathPrefix = "Note";

        [Tooltip("Configuration of the resource loader used with Note resources.")]
        public ResourceLoaderConfiguration Loader = new ResourceLoaderConfiguration { PathPrefix = DefaultPathPrefix };

    }
}
