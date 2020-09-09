using System;

namespace net.roomful.assets.Editor
{
    internal interface IBundleManager
    {
        void Create(Template tpl);
        void Upload(IAsset asset);
        void UpdateMeta(IAsset asset);
        void Download(Template tpl);

        void ResumeUpload();

        Type AssetType { get; }
        Type TemplateType { get; }
        bool IsUploadInProgress { get; }
    }
}