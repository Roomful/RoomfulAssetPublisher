namespace net.roomful.assets.editor
{
    internal interface IAssetWizzard
    {
        void OnGUI(bool guiState);

        bool HasAsset { get; }
    }
}