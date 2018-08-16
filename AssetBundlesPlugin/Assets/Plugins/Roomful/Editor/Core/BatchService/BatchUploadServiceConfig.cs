using System;
using System.Collections.Generic;

namespace RF.AssetWizzard.Editor {
    [Serializable]
    public class BatchUploadServiceConfig {
        public string state;
        public List<UploadItem> uploadQueue; 

        public BatchUploadServiceConfig() {
            state = State.IDLE;
            uploadQueue = new List<UploadItem>(); 
        }

        public static class State {
            public const string IDLE = "idle"; 
            public const string PREPARED_BUNDLES = "preparedBundles";
            public const string UPLOADING = "uploading";
        }
        
        [Serializable]
        public class UploadItem {
            public string TemplateTitle;
            public string TemplatePath;
            public string Platform;
            public string AssetBundlePath;
            public UploadItem(string templateTitle, string templatePath, string platform) {
                TemplateTitle = templateTitle;
                TemplatePath = templatePath;
                Platform = platform; 
            }
        }
    }
    
    
}