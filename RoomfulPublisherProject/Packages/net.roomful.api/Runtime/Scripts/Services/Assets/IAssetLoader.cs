using System;

namespace net.roomful.api.assets
{
    /// <summary>
    /// Asset loader.
    /// </summary>
    public interface IAssetLoader
    {
        /// <summary>
        /// Runs asset loader.
        /// </summary>
        /// <param name="callback">Callback is fired when asset is downloaded and parsed.</param>
        void Run(Action callback);

        /// <summary>
        /// Gets asset template.
        /// </summary>
        /// <returns>Loader's asset template.</returns>
        IAssetTemplate GetAssetTemplate();

        /// <summary>
        /// Defines loader priority in the load queue.
        /// </summary>
        float LoadPriority { get; }

        /// <summary>
        /// Size in megabytes downloaded from web.
        /// </summary>
        float WebLoadedDataSizeMb { get; }

        /// <summary>
        /// Returns `true` if asset is valid and can be downloaded. Returns `false` otherwise.
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Returns `true` if asset is cached locally. Returns `false` otherwise.
        /// </summary>
        bool IsAssetAvailableLocally { get; }

        /// <summary>
        /// Loads asset data but without parsing it.
        /// </summary>
        /// <param name="onComplete">Fired when we request is completed and asset data os available.</param>
        void LoadFromWeb(Action onComplete);

        /// <summary>
        /// Current loader status string.
        /// </summary>
        string Status { get; }
    }
}
