using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using net.roomful.api;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace net.roomful.assets.editor
{
    public class OriginalResourcesManager : Editor
    {
        static readonly string[] s_ResourcesTypesFolders = { "Textures", "Cubemaps"};

        static readonly string s_AbsoluteOriginalResourcesZipPath =
            Application.dataPath + "/Plugins/Roomful/Editor/Temp/AssetOriginalResources.zip";
        
        static readonly string s_AbsoluteOriginalResourcesZipPath2 =
            Application.dataPath + "/Plugins/Roomful/AssetOriginalResources.zip";
        
        static readonly string s_AbsoluteAssetOriginalResourcesPath =
            Application.dataPath + "/Plugins/Roomful/Editor/Temp/AssetOriginalResources";
        static readonly string s_AbsoluteSkinOriginalResourcesPath =
            Application.dataPath + "/Plugins/Roomful/Editor/Temp/SkinOriginalResources";
        const string k_RelativeAssetPrefabResourcesPath = "Plugins/Roomful/Editor/Temp/AssetPrefabResources";
        static readonly string s_AbsoluteAssetPrefabResourcesPath =
            Application.dataPath + "/Plugins/Roomful/Editor/Temp/AssetPrefabResources";
        static readonly string s_AbsoluteSkinPrefabResourcesPath =
            Application.dataPath + "/Plugins/Roomful/Editor/Temp/SkinPrefabResources";

        public static void UploadResourcesArchive(string assetId, string assetOrSkinName, string skinId = null) {
            var prefabName = $"{assetOrSkinName}_{assetId}";
            CollectPrefabOriginalResources(prefabName);
            
            if (UpdateOriginalResources(skinId)) {
                EditorProgressBar.AddProgress(assetOrSkinName, "Uploading Original Resources", 0.2f);
                CompressArchive();
                CreateResource(assetOrSkinName + "_OriginalResources", data =>
                {
                    var json = new JSONData(data);
                    var resourceId = json.GetValue<string>("id");
                
                    GetUploadLink(resourceId, linkToUploadTo =>
                    {
                        var resourceBytes = File.ReadAllBytes(s_AbsoluteOriginalResourcesZipPath);
                        var uploadRequest = new ResourceUpload(linkToUploadTo, resourceBytes);
            
                        uploadRequest.PackageCallbackText = uploadCallback => {
                            ConfirmUpload(resourceId, () =>
                            {
                                Debug.Log("Upload Successful");
                                if (skinId != null)
                                    SetOriginalResourceToSkin(assetId, resourceId, skinId);
                                else
                                    SetOriginalResourceToAsset(assetId, resourceId);
                            });
                        };
                        uploadRequest.Send();
                    });
                });
            }
        }

        static void CollectPrefabOriginalResources(string prefabName) {
            foreach (var folder in s_ResourcesTypesFolders)
                FolderUtils.CreateFolder(k_RelativeAssetPrefabResourcesPath + "/" + folder);
            
            var go = PrefabUtility.LoadPrefabContents(AssetBundlesSettings.FULL_ASSETS_TEMP_LOCATION + prefabName + ".prefab");
            var renderers = go.transform.GetComponentsInChildren<Renderer>();
            foreach (var rend in renderers) {
                foreach (var obj in EditorUtility.CollectDependencies(new Object[] {rend})) {
                    if (obj is Texture) {
                        var texPath = UnityEditor.AssetDatabase.GetAssetPath(obj);
                        var fileName = texPath.Split('/').Last();

                        if (texPath.Contains("Library/unity") || texPath.Contains(".ttf"))
                            break;
                        // Removing importer compressions, etc. to get original textures
                        var ti = AssetImporter.GetAtPath(texPath) as TextureImporter;
                        var texTempPath = "";

                        if (ti.textureShape == TextureImporterShape.TextureCube)
                            texTempPath = k_RelativeAssetPrefabResourcesPath + "/Cubemaps/" + fileName;
                        else
                            texTempPath = k_RelativeAssetPrefabResourcesPath + "/Textures/" + fileName;
                        
                        File.Copy(texPath, Application.dataPath + Path.AltDirectorySeparatorChar + texTempPath, true);
                        UnityEditor.AssetDatabase.ImportAsset("Assets/" + texTempPath);
                        var tempti = AssetImporter.GetAtPath("Assets/" + texTempPath) as TextureImporter;
                        tempti.textureCompression = TextureImporterCompression.Uncompressed;
                        tempti.maxTextureSize = 8192;
                        tempti.filterMode = FilterMode.Point;
                        tempti.mipmapEnabled = false;
                        tempti.SaveAndReimport();

                        if (ti.textureShape != TextureImporterShape.TextureCube && !fileName.Contains(".png"))
                        {
                            Debug.Log("Converting " + fileName);
                            var tex = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture>("Assets/" + texTempPath);
                            var tmp = RenderTexture.GetTemporary(tex.width, tex.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.sRGB);

                            Graphics.Blit(tex, tmp);
                            var previous = RenderTexture.active;
                            RenderTexture.active = tmp;

                            var myTexture2D = new Texture2D(tex.width, tex.height);
                            myTexture2D.name = tex.name;
                            myTexture2D.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
                            myTexture2D.Apply();

                            RenderTexture.active = previous;
                            RenderTexture.ReleaseTemporary(tmp);
                        
                            File.WriteAllBytes(s_AbsoluteAssetPrefabResourcesPath + "/Textures/" + obj.name + ".png",
                                myTexture2D.EncodeToPNG());
                        
                            File.Delete(Application.dataPath + Path.AltDirectorySeparatorChar + texTempPath);
                        }
                    }
                }
            }
        }

        static bool UpdateOriginalResources(string skinId) {
            List<string> downloadedResFiles;
            List<string> prefabResFiles;
            if (skinId != null) {
                if (!Directory.Exists(s_AbsoluteSkinOriginalResourcesPath))
                    return true;
                downloadedResFiles = Directory.GetFiles(s_AbsoluteSkinOriginalResourcesPath, 
                    "*", SearchOption.AllDirectories).Where(name => !name.EndsWith(".meta")).ToList();

                prefabResFiles = Directory.GetFiles(s_AbsoluteSkinPrefabResourcesPath, 
                    "*", SearchOption.AllDirectories).Where(name => !name.EndsWith(".meta")).ToList();
            }
            else {
                if (!Directory.Exists(s_AbsoluteAssetOriginalResourcesPath))
                    return true;
                downloadedResFiles = Directory.GetFiles(s_AbsoluteAssetOriginalResourcesPath, 
                    "*", SearchOption.AllDirectories).Where(name => !name.EndsWith(".meta")).ToList();

                prefabResFiles = Directory.GetFiles(s_AbsoluteAssetPrefabResourcesPath, 
                    "*", SearchOption.AllDirectories).Where(name => !name.EndsWith(".meta")).ToList();
            }
            
            if (downloadedResFiles.Count != prefabResFiles.Count)
                return true;
            
            for (int i = 0; i < downloadedResFiles.Count; i++) {
                var downloadedResFile = new FileInfo(downloadedResFiles[i]);
                var prefabResFile = new FileInfo(prefabResFiles[i]);

                if (downloadedResFile.Length != prefabResFile.Length)
                    return true;
            }
            return false;
        }

        static void CompressArchive()
        {
            // Deleting .meta files
            foreach (var folder in s_ResourcesTypesFolders) {
                var filesList = Directory.GetFiles(s_AbsoluteAssetPrefabResourcesPath + "/" + folder);
                foreach (var item in filesList) {
                    if (item.EndsWith(".meta"))
                        File.Delete(item);
                }

                if (filesList.Length == 0)
                    File.WriteAllText(s_AbsoluteAssetPrefabResourcesPath + "/Dummy", "");
            }
            
            var dir = new DirectoryInfo(s_AbsoluteAssetPrefabResourcesPath);
            var dest = new DirectoryInfo(s_AbsoluteOriginalResourcesZipPath);

            ProcessStartInfo shellEnv;
            if (Application.platform == RuntimePlatform.WindowsEditor) {
                shellEnv = new ProcessStartInfo("powershell") {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    Arguments =
                        $"Compress-Archive -Path \"{dir.FullName.Replace(" ", "` ")}\\*\"" +
                        $" -DestinationPath \"{dest.FullName.Replace(" ", "` ")}\" -Force"
                };
            }
            else {
                shellEnv = new ProcessStartInfo("/bin/bash") {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    Arguments =
                        $"-c \"cd {dir.FullName.Replace(" ", "\\ ")};" +
                        " cd ..;" +
                        $" zip -r {dest.FullName.Replace(" ", "\\ ")} AssetPrefabResources/ -x \"*.DS_Store\"\""
                };
            }
            var process = Process.Start(shellEnv);
            process.WaitForExit();

            Directory.Delete(s_AbsoluteAssetPrefabResourcesPath, true);
        }

        static void SetOriginalResourceToAsset(string assetId, string resourceId) {
            var resourceReference = new ReferenceAssetResource(assetId, resourceId);
            resourceReference.PackageCallbackText = data => {
                //Debug.Log(data);
            };
            resourceReference.Send();
        }

        static void SetOriginalResourceToSkin(string assetId, string resourceId, string skinId) {
            var resourceReference = new ReferenceSkinResource(assetId, resourceId, skinId);
            resourceReference.PackageCallbackText = data => {
                Debug.Log(data);
            };
            resourceReference.Send();
        }
        public static void DownloadAssetResourcesAndOverwrite(string assetId, string assetTitle) {
            EditorProgressBar.AddProgress(assetTitle, "Downloading Original Resources", 0.2f);
            FolderUtils.CreateFolder("Plugins/Roomful/Editor/Temp");
            var assetResourcesPath = Application.dataPath + Path.AltDirectorySeparatorChar + 
                                     AssetBundlesSettings.ASSETS_RESOURCES_LOCATION + Path.AltDirectorySeparatorChar + assetTitle;
            // Used in resources comparison
            var tempOriginalResourcesPath = s_AbsoluteAssetOriginalResourcesPath;
            
            var resourceAssetData = new GetAssetResourceData(assetId);
            resourceAssetData.PackageCallbackData = data => {
                File.WriteAllBytes(s_AbsoluteOriginalResourcesZipPath, data);
                File.WriteAllBytes(s_AbsoluteOriginalResourcesZipPath2, data);
                ExpandArchive(s_AbsoluteOriginalResourcesZipPath, assetResourcesPath, assetTitle);
                ExpandArchive(s_AbsoluteOriginalResourcesZipPath, tempOriginalResourcesPath, assetTitle);
            };
            resourceAssetData.Send();
        }

        public static void DownloadSkinResourcesAndOverwrite(string assetId, string skinId, string skinTitle) {
            EditorProgressBar.AddProgress(skinTitle, "Downloading Original Resources", 0.2f);
            FolderUtils.CreateFolder("Plugins/Roomful/Editor/Temp");
            var assetResourcesPath = Application.dataPath + Path.AltDirectorySeparatorChar + 
                                     AssetBundlesSettings.ASSETS_RESOURCES_LOCATION + Path.AltDirectorySeparatorChar + skinTitle;
            // Used in resources comparison
            var tempOriginalResourcesPath = s_AbsoluteSkinOriginalResourcesPath;
            
            var resourceAssetData = new GetSkinResourceData(assetId, skinId);
            resourceAssetData.PackageCallbackData = data => {
                File.WriteAllBytes(s_AbsoluteOriginalResourcesZipPath, data);
                ExpandArchive(s_AbsoluteOriginalResourcesZipPath, assetResourcesPath, skinTitle);
                ExpandArchive(s_AbsoluteOriginalResourcesZipPath, tempOriginalResourcesPath, skinTitle);
            };
            resourceAssetData.Send();
        }

        static void ExpandArchive(string archivePath, string destPath, string assetOrSkinName) {
            var archiveDirInfo = new DirectoryInfo(archivePath);
            var destDirInfo = new DirectoryInfo(destPath);
            
            ProcessStartInfo shellEnv;
            if (Application.platform == RuntimePlatform.WindowsEditor) {
                shellEnv = new ProcessStartInfo("powershell") {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    Arguments =
                        $"Expand-Archive -Path \"{archiveDirInfo.FullName.Replace(" ", "` ")}\"" +
                        $" -DestinationPath \"{destDirInfo.FullName.Replace(" ", "` ")}\" -Force"
                };
            }
            else {
                shellEnv = new ProcessStartInfo("/bin/bash") {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    Arguments =
                        $"-c \"unzip -o {archiveDirInfo.FullName.Replace(" ", "\\ ")} -d {destDirInfo.FullName.Replace(" ", "\\ ")};" +
                        $" cd {destDirInfo.FullName.Replace(" ", "\\ ")};" +
                        $" cp -rf AssetPrefabResources/. {assetOrSkinName.Replace(" ", "\\ ")};" +
                        " rm -vr AssetPrefabResources\""
                };
            }
            var process = Process.Start(shellEnv);
            process.WaitForExit();
            EditorProgressBar.FinishUploadProgress();
        }

        static void CreateResource(string fileName, Action<string> data) {
            var createResourceRequest = new CreateResourceMetadata(fileName);
            createResourceRequest.PackageCallbackText = data.Invoke;
            createResourceRequest.Send();
        }

        static void GetUploadLink(string resourceId, Action<string> uploadLink) {
            var uploadLinkRequest = new GetResourceUploadLink(resourceId);
            uploadLinkRequest.Headers.Add("X-Resource-Id", resourceId);
            uploadLinkRequest.PackageCallbackText = uploadLink.Invoke;
            uploadLinkRequest.Send();
        }

        static void ConfirmUpload(string resourceId, Action onComplete) {
            var confirm = new ResourceUploadConfirmation(resourceId);
            confirm.Headers.Add("X-Resource-Id", resourceId);
            confirm.PackageCallbackText = confirmCallback => {
                onComplete.Invoke();
            };
            confirm.Send();
        }
    }

    class CreateResourceMetadata : BaseWebPackage
    {
        const string REQUEST_URL = "/api/v0/resource/create";
        ResourceMetadata m_meta;
        bool m_addToSortingTable;

        public CreateResourceMetadata(string fileName, bool addToSortingTable = false) : base(REQUEST_URL) {
            m_meta = new ResourceMetadata();
            m_meta.Name = fileName;
            m_addToSortingTable = addToSortingTable;
        }

        public override Dictionary<string, object> GenerateData() {
            var metaInfo = new Dictionary<string, object>();
            metaInfo.Add("metadata", m_meta.ToDictionary());

            var originalJson = new Dictionary<string, object>();
            originalJson.Add("resource", metaInfo);
            originalJson.Add("addToSortingTable", m_addToSortingTable);
            return originalJson;
        }
    }

    class GetResourceUploadLink : BaseWebPackage
    {
        const string REQUEST_URL = "/api/v0/resource/upload/link";
        readonly string m_assetId;

        public GetResourceUploadLink(string assetId) : base(REQUEST_URL)
        {
            m_assetId = assetId;
        }

        public override Dictionary<string, object> GenerateData () {
            var originalJSON =  new Dictionary<string, object>();
            originalJSON.Add("asset", m_assetId);
            originalJSON.Add("contentType", "application/zip");
            return originalJSON;
        }
    }

    class ResourceUploadConfirmation : BaseWebPackage {
        const string REQUEST_URL = "/api/v0/resource/upload/link/complete";

        readonly string m_assetId;

        public ResourceUploadConfirmation (string assetId) : base (REQUEST_URL) {
            m_assetId = assetId;
        }

        public override Dictionary<string, object> GenerateData () {
            var originalJSON =  new Dictionary<string, object>();
            originalJSON.Add ("asset", m_assetId);
            return originalJSON;
        }
    }

    class ResourceUpload : BaseWebPackage
    {
        public ResourceUpload(string requestUrl, byte[] data) : base(requestUrl, RequestMethods.PUT) {
            m_PackData = data;
        }

        public override bool IsDataPack => true;

        public override Dictionary<string, object> GenerateData() {
            var originalJson = new Dictionary<string, object>();
            return originalJson;
        }
    }

    class ReferenceAssetResource : BaseWebPackage
    {
        const string REQUEST_URL = "/api/v0/asset/setOriginal";
        readonly string m_assetId;
        readonly string m_resourceId;

        public ReferenceAssetResource(string assetId, string resourceId) : base(REQUEST_URL) {
            m_resourceId = resourceId;
            m_assetId = assetId;
        }

        public override Dictionary<string, object> GenerateData() {
            var originalJSON = new Dictionary<string, object>();
            originalJSON.Add("assetId", m_assetId);
            originalJSON.Add("resourceId", m_resourceId);
            return originalJSON;
        }
    }

    class ReferenceSkinResource : BaseWebPackage
    {
        const string REQUEST_URL = "/api/v0/asset/skin/setOriginal";
        readonly string m_assetId;
        readonly string m_skinId;
        readonly string m_resourceId;

        public ReferenceSkinResource(string assetId, string resourceId, string skinId) : base(REQUEST_URL) {
            m_resourceId = resourceId;
            m_assetId = assetId;
            m_skinId = skinId;
        }

        public override Dictionary<string, object> GenerateData() {
            var originalJSON = new Dictionary<string, object>();
            originalJSON.Add("assetId", m_assetId);
            originalJSON.Add("skinId", m_skinId);
            originalJSON.Add("resourceId", m_resourceId);
            return originalJSON;
        }
    }

    class GetAssetResourceData : BaseWebPackage
    {
        const string REQUEST_URL = "/api/v0/asset/original/";

        public GetAssetResourceData(string assetId) : base(REQUEST_URL, RequestMethods.GET) {
            AddToUrl (assetId);
        }

        public override Dictionary<string, object> GenerateData() {
            var originalJSON = new Dictionary<string, object>();
            return originalJSON;
        }
    }

    class GetSkinResourceData : BaseWebPackage
    {
        const string REQUEST_URL = "/api/v0/asset/skin/original/";

        public GetSkinResourceData(string assetId, string skinId) : base(REQUEST_URL, RequestMethods.GET) {
            AddToUrl (assetId + "/" + skinId);
        }

        public override Dictionary<string, object> GenerateData() {
            var originalJSON = new Dictionary<string, object>();
            return originalJSON;
        }
    }
}