﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace RF.AssetWizzard.Editor
{
    public static class Validation 
    {
        private static List<BuildTarget> s_allowedPlatfroms = new List<BuildTarget>();

        static Validation() {
            s_allowedPlatfroms.Add(BuildTarget.iOS);
            s_allowedPlatfroms.Add(BuildTarget.WebGL);
        }


        public static bool Run(PropAsset asset) {
            float max = Mathf.Max(asset.Size.x, asset.Size.y, asset.Size.z);

            if (max < AssetBundlesSettings.MIN_ALLOWED_SIZE) {
                EditorUtility.DisplayDialog("Error", "Your Asset is too small", "Ok");
                return false;
            }

            if (max > AssetBundlesSettings.MAX_AlLOWED_SIZE) {
                //EditorUtility.DisplayDialog ("Error", "Your Asset is too big", "Ok");
               // return false;
            }

            if (asset.Model.childCount < 1) {
                EditorUtility.DisplayDialog("Error", "Asset is empty!", "Ok");
                return false;
            }


            var icon = asset.Icon;
            string path = UnityEditor.AssetDatabase.GetAssetPath(icon);
            TextureImporter ti = (TextureImporter)TextureImporter.GetAtPath(path);
            if(ti != null) {
                if(!ti.isReadable) {
                    ti.isReadable = true;  
                }

                TextureImporterPlatformSettings currentPlatfromSettings = ti.GetPlatformTextureSettings(EditorUserBuildSettings.activeBuildTarget.ToString());

                currentPlatfromSettings.textureCompression = TextureImporterCompression.Uncompressed;
                currentPlatfromSettings.maxTextureSize = 128;
               
                TextureImporterPlatformSettings defaultsettings = ti.GetDefaultPlatformTextureSettings();
                defaultsettings.textureCompression = TextureImporterCompression.Uncompressed;
                defaultsettings.maxTextureSize = 128;


                ti.SetPlatformTextureSettings(defaultsettings);
                ti.SetPlatformTextureSettings(currentPlatfromSettings);

    
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            }

            icon.EncodeToPNG();



            if (AssetBundlesSettings.Instance.TargetPlatforms.Count > 0) {
                foreach(BuildTarget platfrom in AssetBundlesSettings.Instance.TargetPlatforms) {
                    if(!s_allowedPlatfroms.Contains(platfrom)) {
                        EditorUtility.DisplayDialog("Error", platfrom.ToString() + " platfrom is not supported", "Ok");
                        WizardWindow.SelectedSectionIndex = 2;
                        return false;
                    }
                }

            } else {
                EditorUtility.DisplayDialog("Error", "Please select at least one platfrom to upload", "Ok");
                WizardWindow.SelectedSectionIndex = 2;
                return false;
            }



            return true;
        }
    }
}
