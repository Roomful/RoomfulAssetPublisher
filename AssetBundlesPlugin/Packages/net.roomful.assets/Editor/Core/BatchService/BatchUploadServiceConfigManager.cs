using System.IO;
using UnityEngine;

namespace net.roomful.assets.Editor {
    public class BatchUploadServiceConfigManager {
        private const string CONFIG_FILE_NAME = "batchUploadConfig.json"; 
        private static BatchUploadServiceConfig s_config;
        private static void Load() {
            if (File.Exists(CONFIG_FILE_NAME)) {
                s_config = JsonUtility.FromJson<BatchUploadServiceConfig>(File.ReadAllText(CONFIG_FILE_NAME));
            } else {
                s_config = new BatchUploadServiceConfig();
            }
        }

        public static BatchUploadServiceConfig GetConfig() {
            if (s_config == null) {
                Load();
            }

            return s_config;
        }

        public static void Save() {
            if (s_config != null) {
                File.WriteAllText(CONFIG_FILE_NAME, JsonUtility.ToJson(s_config));
            }
        }
    }
}