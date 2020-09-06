using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.assets.Editor
{
    public interface IBundleManager
    {
        event Action OnUploaded;

        void Create(Template tpl);
        void Upload(IAsset asset);
        void UpdateMeta(IAsset asset);
        void Download(Template tpl);
        
        void ResumeUpload();


        System.Type AssetType { get; }
        System.Type TemplateType { get;}
        bool IsUploadInProgress { get; }

    }
}