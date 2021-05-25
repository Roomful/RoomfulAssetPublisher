using System;

namespace net.roomful.api.assets
{
    public interface IAssetLoader
    {
        void Run(Action callback);
        IAssetTemplate GetAssetTemplate();
        float LoadPriority { get; }
        float WebLoadedDataSizeMb { get; }
        bool IsValid { get; }
        bool IsAssetAvailableLocally { get; }
        void LoadFromWeb(Action onComplete);
        string Status { get; }
    }
}