using System;
using System.Collections.Generic;
using System.Linq;
using net.roomful.api;
using UnityEngine;

namespace net.roomful.assets.editor
{
    internal static class VariantsService
    {
        public static bool DebugMode { get; set; }
        public static List<PropVariant> Variants { get; } = new List<PropVariant>();

        private static readonly Dictionary<string, PropVariant> s_gameObjectNameToVariant = new Dictionary<string, PropVariant>();

        public static void LoadVariantsList(string assetId, Action callback, Action callbackError = null ) {

            Variants.Clear();
            var getVariantsList = new GetVariantsList(assetId);
            getVariantsList.PackageCallbackText = data => {
                Debug.Log("GetVariantsList:" + data);
                var variantsList = Json.Deserialize(data) as List<object>;

                s_gameObjectNameToVariant.Clear();
                foreach (var variantData in variantsList) {
                    var json = new JSONData(variantData);
                    var variant = new PropVariant(json);
                    Variants.Add(variant);
                }

                Variants.Sort(new PropVariantComparer());
                RefreshCachedVariants();
                callback.Invoke();
            };
            getVariantsList.PackageCallbackError = l => {
                callbackError.Invoke();
            };
            getVariantsList.Send();
        }

        private static void RefreshCachedVariants() {

            foreach (var variant in Variants) {
                foreach (var gameobjectName in variant.GameobjectsNames) {
                    s_gameObjectNameToVariant[gameobjectName] = variant;
                }

                variant.FinRenderers(PropAsset.GetLayer(HierarchyLayers.Graphics));
            }
        }

        public static void GetSkinsList(string assetId, string variantId, Action<List<PropSkin>> callback) {
            var getVariantsList = new GetSkinsList(assetId, variantId);
            getVariantsList.PackageCallbackText = data => {
                Debug.Log("GetSkinsList:" + data);
                var skins = new List<PropSkin>();
                var skinsList = Json.Deserialize(data) as List<object>;
                foreach (var skinData in skinsList) {
                    var json = new JSONData(skinData);
                    skins.Add(new PropSkin(json));
                }

                skins.Sort(new PropSkinComparer());

                callback.Invoke(skins);
            };

            getVariantsList.Send();
        }

        public static void SaveVariantMeta(string assetId, PropVariant variant) {
            var thumbnailUploader = new VariantThumbnailUploader(assetId, variant);
            thumbnailUploader.Upload(variant.Thumbnail, iconRes => {
                variant.SetIconResource(iconRes);

                var updateVariant = new UpdateVariant(assetId, variant);
                updateVariant.PackageCallbackText = data => {
                    MessageUtils.ShowNotification("Variant properties saved.");
                };
                updateVariant.Send();
            });
        }

        public static void SaveSkinMeta(string assetId, PropSkin skin, bool showSavedMessage) {
            var thumbnailUploader = new SkinThumbnailUploader(assetId, skin);
            thumbnailUploader.Upload(skin.Thumbnail, iconRes => {
                skin.SetIconResource(iconRes);

                var updateVariant = new UpdateSkin(assetId, skin);
                updateVariant.PackageCallbackText = data => {
                    if(showSavedMessage)
                        MessageUtils.ShowNotification("Skin properties saved.");
                };
                updateVariant.Send();
            });
        }

        public static void CreateVariant(string variantName, PropAsset propAsset, List<GameObject> gameObjects, Action<PropVariant> onVariantCreated) {
            if (ValidateVariantCreate(propAsset, gameObjects)) {
                var propVariant = new PropVariant(variantName, gameObjects, propAsset.GetLayer(HierarchyLayers.Graphics));
                var createVariant = new CreateVariant(propAsset.Template.Id, propVariant);
                createVariant.Send();
                createVariant.PackageCallbackText = data => {
                    var json = new JSONData(data);
                    var variant = new PropVariant(json);
                    Variants.Add(variant);
                    RefreshCachedVariants();
                    onVariantCreated.Invoke(variant);
                };
            }
        }

        public static void CreateSkin(string skinName, string variantId, string assetId, Action<PropSkin> callback) {
            var propSkin = new PropSkin(skinName, variantId);
            var createSkin = new CreateSkin(assetId, propSkin);
            createSkin.Send();
            createSkin.PackageCallbackText = data => {
                var json = new JSONData(data);
                var skin = new PropSkin(json);
                callback.Invoke(skin);
            };
        }

        public static void DeleteVariant(string assetId, PropVariant variant, Action callback) {
            var deleteVariant = new DeleteVariant(assetId, variant.Id);
            deleteVariant.Send();
            deleteVariant.PackageCallbackText = data => {
                MessageUtils.ShowNotification($"Variant {variant.Id} deleted.");
                callback.Invoke();
            };
        }

        public static void DeleteSkin(string assetId, PropSkin skin, Action<PropSkin> callback) {
            var deleteSkin = new DeleteSkin(assetId, skin.Id);
            deleteSkin.Send();
            deleteSkin.PackageCallbackText = data => {
                Debug.Log($"Skin {skin.Id} deleted.");
                callback.Invoke(skin);
            };
        }

        private static bool ValidateVariantCreate(PropAsset propAsset, List<GameObject> gameObjects) {
            if (!gameObjects.Any()) {
                MessageUtils.ShowNotification("No GameObjects Selected!");
                return false;
            }

            foreach (var gameObject in gameObjects) {
                var path = SkinUtility.GetGameObjectPath(gameObject.transform, propAsset.GetLayer(HierarchyLayers.Graphics));
                if (s_gameObjectNameToVariant.ContainsKey(path)) {
                    MessageUtils.ShowNotification($"{path} already used by {s_gameObjectNameToVariant[path].Name}");
                    return false;
                }
            }

            return true;
        }

        private static PropAsset s_propAsset;
        private static PropAsset PropAsset {
            get {
                if (s_propAsset == null) {
                    s_propAsset = FindObjectWithType<PropAsset>();
                }

                return s_propAsset;
            }
        }

        private  static T FindObjectWithType<T>() {
            var allFoundObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject));
            foreach (var obj in allFoundObjects) {
                var gameObject = (GameObject) obj;
                var target = gameObject.GetComponent<T>();

                if (target != null) {
                    return target;
                }
            }

            return default;
        }
    }
}
