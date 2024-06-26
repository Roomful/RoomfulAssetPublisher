using System;
using net.roomful.api;
using UnityEditor;
using UnityEngine;

namespace net.roomful.assets.editor
{
    internal class SkinBundleManager : BundleManager<PropSkinUploadModel, SkinAsset>
    {
        protected override string PersistentTemplatePath => AssetBundlesSettings.FULL_ASSETS_TEMP_LOCATION + nameof(PropSkin) + ".txt";
        protected override bool IsAssetValid(SkinAsset asset)
        {
            return IsAssetMetadataValidate(asset);
        }

        protected override void ConfirmUpload(AssetTemplate tpl, BuildTarget platform, Action onComplete) {
            var template = BundleUtility.LoadTemplateFromFile<PropSkinUploadModel>(PersistentTemplatePath);
            var confirm = new SkinUploadConfirmation(template.AssetId, template.SkinId, platform.ToString());
            confirm.PackageCallbackText = confirmCallback => {
                onComplete.Invoke();
            };
            confirm.Send();
        }

        protected override void GetUploadLink(AssetTemplate tpl, BuildTarget platform, Action<string> uploadLink) {
            var template = BundleUtility.LoadTemplateFromFile<PropSkinUploadModel>(PersistentTemplatePath);

            var uploadLinkRequest = new GetSkinUploadLink(template.AssetId, template.SkinId, platform.ToString(), tpl.Title);
            uploadLinkRequest.PackageCallbackText += uploadLink.Invoke;
            uploadLinkRequest.Send();
        }

        public override void Upload(IAsset asset) {
            FolderUtils.CreateFolder(AssetBundlesSettings.ASSETS_TEMP_LOCATION);
            FolderUtils.CreateFolder(AssetBundlesSettings.ASSETS_RESOURCES_LOCATION);


            asset.PrepareForUpload();
            var template = asset.GetTemplate();
            BundleUtility.SaveTemplateToFile(PersistentTemplatePath, asset.GetTemplate());

            AssetBundlesSettings.Instance.ReplaceSavedTemplate(template);
            BundleUtility.SaveTemplateToFile(PersistentTemplatePath, template);
            BundleUtility.GenerateUploadPrefab(asset);

          //  var skinAsset = (SkinAsset) asset;
          //  OriginalResourcesManager.UploadResourcesArchive(skinAsset.SkinModel.AssetId, template.Title, template.Id);
          AssetsUploadLoop(0, template);
        }
        
        protected override void OnUploadFinished(PropSkinUploadModel tpl) {
            
            var propTemplate = new PropAssetTemplate();
            propTemplate.ParseData(new JSONData(tpl.ToDictionary()));
            propTemplate.Id = tpl.AssetId;
            propTemplate.Title = tpl.AssetTitle;
            
            BundleService.Download(propTemplate);
            EditorUtility.DisplayDialog("Success", tpl.Title + " prop skin has been successfully uploaded!", "Ok");
        }

        protected override void CreateAsset(PropSkinUploadModel tpl) {
            throw new NotImplementedException();
        }

        protected override IAsset CreateDownloadedAsset(PropSkinUploadModel tpl, GameObject gameObject) {
            throw new NotImplementedException();
        }

        protected override AssetMetadataRequest GenerateMeta_Create_Request(SkinAsset asset) {
            throw new NotImplementedException();
        }

        protected override AssetMetadataRequest GenerateMeta_Update_Request(SkinAsset asset) {
            throw new NotImplementedException();
        }
    }
}