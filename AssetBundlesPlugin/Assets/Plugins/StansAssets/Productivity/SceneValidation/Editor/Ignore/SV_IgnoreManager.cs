using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


using SA.Foundation.Config;
using SA.Foundation.Utility;
using SA.Foundation.UtilitiesEditor;

using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;


namespace SA.Productivity.SceneValidator
{


    public class SV_IgnoreManager : UnityEditor.AssetModificationProcessor
    {

        private const string IGNOR_FILES_PATH = SA_Config.STANS_ASSETS_EDITOR_SETTINGS_PATH + "SceneValidation/";
        private static Dictionary<Scene, SV_IgnoreList> m_sceneIgnoreRecords = new Dictionary<Scene, SV_IgnoreList>();

        public static bool IsRuleIgnored(SV_iValidationRule rule, Component component) {
            if(component == null || component.gameObject == null) {
                return false;
            }

            var gameObject = component.gameObject;
            var scene = gameObject.scene;
            
            SV_IgnoreList ignoreList = GetIgnoreList(scene);
            if(ignoreList == null) {
                return false;
            }

            int fileId = SA_AssetDatabase.GetLocalIdentifierInFile(component);

            return ignoreList.IsRuleIgnored(fileId, rule);
        }



        public static void Ignore(SV_iValidationRule rule, Component component) {
            var gameObject = component.gameObject;
            var scene = gameObject.scene;

            int fileId = SA_AssetDatabase.GetLocalIdentifierInFile(component);
            if(fileId == 0) {
                string message = string.Format("You can't ignore the component wich is not part of the scene. Please, save the {0} scene and try again.", scene.name);
                EditorUtility.DisplayDialog("Error", message, "Ok");
                return;
            }

            SV_IgnoreList ignoreList = GetIgnoreList(scene);
            if(ignoreList == null) {
                ignoreList = new SV_IgnoreList();
            }

            var record = new SV_IgnoreRecord(fileId, rule.GetType().Name);
            ignoreList.AddRecord(record);

            //Now Save
            string json = JsonUtility.ToJson(ignoreList);
            string filePath = GetSceneIgnoreListFileName(scene);
            SA_FilesUtil.Write(filePath, json);
            SA_AssetDatabase.ImportAsset(filePath);

            SaveIgnoreListCache(scene, ignoreList);

            SV_Validation.API.Restart();

        }


        private static SV_IgnoreList GetIgnoreList(Scene scene) {
            if(m_sceneIgnoreRecords.ContainsKey(scene)) {
                return m_sceneIgnoreRecords[scene];
            } else {
                //Let's try to load
                SV_IgnoreList ignoreList = null;
                string filePath = GetSceneIgnoreListFileName(scene);
                if (SA_FilesUtil.IsFileExists(filePath)) {
                    string json = SA_FilesUtil.Read(filePath);
                    ignoreList = JsonUtility.FromJson<SV_IgnoreList>(json);
                    ignoreList.Cache();
                } 

                SaveIgnoreListCache(scene, ignoreList);
                return ignoreList;
            }
        }

        private static void SaveIgnoreListCache(Scene scene, SV_IgnoreList ignoreList) {
            m_sceneIgnoreRecords[scene] = ignoreList;
        }

        private static string GetSceneIgnoreListFileName(Scene scene) {
            string guid = AssetDatabase.AssetPathToGUID(scene.path);

            return GetSceneCachePath(scene.name, guid);

        }

        private static string GetSceneCachePath(string sceneName, string guid) {
            return IGNOR_FILES_PATH + sceneName + "_" + guid + ".txt";
        }


        private static string GetSceneCachePathByScenePath(string scenePath) {
            string guid = AssetDatabase.AssetPathToGUID(scenePath);
            string sceneName = SA_PathUtil.GetFileNameWithoutExtension(scenePath);

            return GetSceneCachePath(sceneName, guid);
        }


        //--------------------------------------
        // AssetModificationProcessor
        //--------------------------------------



        private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath) {

           
            string extension = SA_PathUtil.GetExtension(sourcePath);
            if (extension.Equals(".unity")) {
                string path = GetSceneCachePathByScenePath(sourcePath);
                if(SA_FilesUtil.IsFileExists(path)) {
                    string guid = AssetDatabase.AssetPathToGUID(sourcePath);

                    string newName = SA_PathUtil.GetFileNameWithoutExtension(destinationPath);
                    string newPath = GetSceneCachePath(newName, guid);
                    if (!path.Equals(newPath)) {
                        SA_AssetDatabase.MoveAsset(path, newPath);
                    }
                }
            }
            return AssetMoveResult.DidNotMove;

        }

        private static AssetDeleteResult OnWillDeleteAsset(string sourcePath, RemoveAssetOptions option) {

            string extension = SA_PathUtil.GetExtension(sourcePath);
            if (extension.Equals(".unity")) {
                string path = GetSceneCachePathByScenePath(sourcePath);
                if(SA_AssetDatabase.IsFileExists(path)) {
                    SA_AssetDatabase.DeleteAsset(path);
                }
            }     

            return AssetDeleteResult.DidNotDelete;
        }


    }
}