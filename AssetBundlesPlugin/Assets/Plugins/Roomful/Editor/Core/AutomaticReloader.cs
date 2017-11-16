/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Editor {

  public class AutomaticReloader {

      public static void ReloadAllAssets() {
          AssetBundlesSettings.Instance.TemporaryAssetTemplates.Clear();

          var allAssetsRequest = new RF.AssetWizzard.Network.Request.GetAllAssets (0, 3, new List<string>() {""});

          allAssetsRequest.PackageCallbackText = (allAssetsCallback) => {
              List<object> allAssetsList = SA.Common.Data.Json.Deserialize(allAssetsCallback) as List<object>;

              foreach (object assetData in allAssetsList) {
                  AssetTemplate at = new AssetTemplate(new JSONData(assetData).RawData);
                  AssetBundlesSettings.Instance.TemporaryAssetTemplates.Add(at);
              }

              AssetBundlesSettings.Save();

              StartLoop();
          };

          allAssetsRequest.Send();
      }

      private static void StartLoop() {
          Debug.Log("TemporaryAssetTemplates.Count "+ AssetBundlesSettings.Instance.TemporaryAssetTemplates.Count);
          if (AssetBundlesSettings.Instance.TemporaryAssetTemplates.Count > 0) {

              bool isUrlValid = false;
#if UNITY_EDITOR

              string pl = UnityEditor.EditorUserBuildSettings.activeBuildTarget.ToString();

              foreach (var u in Peek().Urls) {
                  if (u.Platform.Equals(pl)) {
                      isUrlValid = !string.IsNullOrEmpty(u.Url);

                      break;
                  }
              }

#endif          
              if (isUrlValid) {
                  AssetBundlesSettings.Instance.IsInAutoloading = true;

                  BundleUtility.ClearLocalCache();

                  PropAsset.PropInstantieted += PropInstantiedtedHandler;

                  //AssetBundlesSettings.Instance.PublisherCurrentVersion = "1.0";
                  PropBundleManager.DownloadAssetBundle(Dequeue());

              } else {
                  Debug.Log("Url is invalid, load next");
                  Dequeue();
                  StartLoop();
              }

          } else {
              AssetBundlesSettings.Instance.IsInAutoloading = false;
          }
      }

      private static void PropInstantiedtedHandler() {
          Debug.Log("PropInstantiedtedHandler");
          PropAsset.PropInstantieted -= PropInstantiedtedHandler;

          UnityEditor.EditorApplication.update += OnUpdate;
      }

      private static int counter = 0;
      private static void OnUpdate() {
          counter += 1;

          if (counter > 2000) {
#if UNITY_EDITOR
              Debug.Log("Delay call done for reuplaod");
              counter = 0;
              UnityEditor.EditorApplication.update -= OnUpdate;
              if (AssetBundlesSettings.Instance.IsInAutoloading) {

                  //AssetBundlesSettings.Instance.PublisherCurrentVersion = "2.0";
                  PropBundleManager.ReuploadAsset (CurrentProp);
              }
#endif
          }
      }

      [UnityEditor.Callbacks.DidReloadScripts]
      private static void OnScriptsReloaded() {
          #if UNITY_EDITOR
          if (AssetBundlesSettings.Instance.IsInAutoloading) {
              PropBundleManager.AssetBundleUploadedEvent += AssetBundleUploadedHandler;
          }
          #endif
      }

      private static void AssetBundleUploadedHandler() {
          Debug.Log("Reupload complete");
          PropBundleManager.AssetBundleUploadedEvent -= AssetBundleUploadedHandler;

          StartLoop ();
      }

      private static PropAsset CurrentProp {
          get {
              return GameObject.FindObjectOfType<PropAsset> ();
          }
      }

      private static AssetTemplate Dequeue() {
          int indexOfLast = AssetBundlesSettings.Instance.TemporaryAssetTemplates.Count - 1;
          AssetTemplate curr = AssetBundlesSettings.Instance.TemporaryAssetTemplates [indexOfLast];

          AssetBundlesSettings.Instance.TemporaryAssetTemplates.Remove (curr);
          AssetBundlesSettings.Save();

          return curr;
      }


      private static AssetTemplate Peek() {
          int indexOfLast = AssetBundlesSettings.Instance.TemporaryAssetTemplates.Count - 1;

          return AssetBundlesSettings.Instance.TemporaryAssetTemplates [indexOfLast];
      }
  }

}
*/
