using System;
using Naninovel;
using UnityEngine;

namespace NaninovelNote
{
    /// <summary>
    /// A custom engine service used to manage available inventory items.
    /// </summary>
    [InitializeAtRuntime] // makes the service auto-initialize with other built-in engine services
    [Naninovel.Commands.Goto.DontReset] // skips the service reset on @goto commands (to preserve the inventory when navigating scripts)
    public class NoteManager : IEngineService<NoteConfiguration>
    {
        public NoteConfiguration Configuration { get; }

        private readonly IResourceProviderManager providersManager;
        private readonly ILocalizationManager localizationManager;
        private LocalizableResourceLoader<GameObject> pageLoader;

        public NoteManager(NoteConfiguration config,
            IResourceProviderManager providersManager, ILocalizationManager localizationManager)
        {
            // Engine service constructors are invoked when the engine is initializing;
            // remember that it's not safe to use other services here, as they are not initialized yet.
            // Instead, store references to the required services and use them in `InitializeServiceAsync()` method.

            Configuration = config;
            this.providersManager = providersManager; // required to load item prefabs
            this.localizationManager = localizationManager; // required to load localized versions of item prefabs
        }

        public UniTask InitializeServiceAsync()
        {
            // Invoked when the engine is initializing, after services required in the constructor are initialized;
            // it's safe to use the required services here (IResourceProviderManager in this case).

            // Initialize item prefab loader, as per the configuration.
            pageLoader = Configuration.Loader.CreateLocalizableFor<GameObject>(providersManager, localizationManager);

            return UniTask.CompletedTask;
        }

        public void ResetService()
        {
            // Invoked when resetting engine state (eg, loading a script or starting a new game).

            var note = Engine.GetService<IUIManager>().GetUI<NoteUI>();
            if (ObjectUtils.IsValid(note))
                note.RemoveAllPages(); // remove all items from the current inventory

            pageLoader?.ReleaseAll(this); // unload item prefabs to free memory
        }

        public void DestroyService()
        {
            // Invoked when destroying the engine.

            pageLoader?.ReleaseAll(this);
        }

        /// <summary>
        /// Attempts to retrieve item prefab with the specified identifier.
        /// </summary>
        public async UniTask<NotePage> GetPageAsync(string pageId)
        {
            // If item resource is already loaded, get it; otherwise load asynchronously.
            var pageResource = pageLoader.GetLoadedOrNull(pageId) ?? await pageLoader.LoadAndHoldAsync(pageId, this);
            if (!pageResource.Valid) throw new Exception($"Failed to load `{pageId}` page resource.");
            return pageResource.Object.GetComponent<NotePage>();
        }
    }
}
