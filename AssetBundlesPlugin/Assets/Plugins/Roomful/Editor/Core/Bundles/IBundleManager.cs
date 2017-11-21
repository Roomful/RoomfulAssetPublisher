using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Editor
{
    public interface IBundleManager
    {

        void Create(Template tpl);
        void Upload(IAsset asset);
        void Download(Template tpl);
        void ResumeUpload();


        System.Type AssetType { get; }
        System.Type TemplateType { get;}
        bool IsUploadInProgress { get; }

    }
}