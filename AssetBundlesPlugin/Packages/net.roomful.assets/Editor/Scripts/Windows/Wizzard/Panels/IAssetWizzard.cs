namespace net.roomful.assets.Editor
{
    internal interface IAssetWizzard
    {
        void OnGUI(bool guiState);

        bool HasAsset { get; }
    }
}