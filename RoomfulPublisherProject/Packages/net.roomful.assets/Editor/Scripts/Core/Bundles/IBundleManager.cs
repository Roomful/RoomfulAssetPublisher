using System;

namespace net.roomful.assets.editor
{
    interface IBundleManager
    {
        void Create(AssetTemplate tpl);
        void Upload(IAsset asset);
        void UpdateMeta(IAsset asset);
        void Download(AssetTemplate tpl);

        void ResumeUpload();

        Type AssetType { get; }
        Type TemplateType { get; }
        bool IsUploadInProgress { get; }
    }
}