using System.Collections.Generic;
using System.Text;
using net.roomful.assets.serialization;
using StansAssets.Foundation.Patterns;
using UnityEngine;

namespace net.roomful.assets
{
    public static class SkinUtility
    {
        private static readonly Dictionary<Transform, List<string>> s_skinMap = new Dictionary<Transform, List<string>>();

        public static void ApplySkin(Transform target, Transform skin) {
            var skinTransforms = skin.GetComponentsInChildren<Transform>();

            var reParentArray = ListPool<Transform>.Get();
            foreach (var skinTransform in skinTransforms) {
                var customizableSkinGameObject = skinTransform.GetComponent<SerializedCustomizableSkinGameObject>();
                switch (customizableSkinGameObject.Type) {
                    case CustomizableSkinGameObjectType.MountedPoint:
                    case CustomizableSkinGameObjectType.ReplaceTarget:
                    case CustomizableSkinGameObjectType.ReplaceMaterial:
                        reParentArray.Add(customizableSkinGameObject.transform);
                        AddToSkinMap(customizableSkinGameObject.transform, skin);
                        break;
                }
            }

            foreach (var customizableSkinTransform in reParentArray) {
                customizableSkinTransform.SetParent(skin);
            }

            foreach (var customizableSkinTransform in reParentArray) {
                var customizableSkinGameObject = customizableSkinTransform.GetComponent<SerializedCustomizableSkinGameObject>();

                switch (customizableSkinGameObject.Type) {
                    case CustomizableSkinGameObjectType.MountedPoint:
                        var targetRootObject = GetMatchedGameObjectOnTarget(customizableSkinTransform, target);
                        if (targetRootObject != null) {
                            ReParentChildren(customizableSkinTransform, targetRootObject);
                        }
                        else {
                            Debug.LogWarning($"Can't locate target hierarchy path for skin: {customizableSkinTransform.name}");
                        }

                        break;

                    case CustomizableSkinGameObjectType.ReplaceMaterial:
                        var materialTargetObject = GetMatchedGameObjectOnTarget(customizableSkinTransform, target);
                        if (materialTargetObject != null) {
                            var targetRenderer = materialTargetObject.GetComponent<Renderer>();
                            var skinRenderer = customizableSkinTransform.GetComponent<Renderer>();

                            targetRenderer.materials = skinRenderer.materials;
                        }
                        else {
                            Debug.LogWarning($"Can't locate target hierarchy path for skin: {customizableSkinTransform.name}");
                        }

                        break;
                    case CustomizableSkinGameObjectType.ReplaceTarget:
                        var targetObject = GetMatchedGameObjectOnTarget(customizableSkinTransform, target);
                        if (targetObject != null) {
                            customizableSkinTransform.SetParent(targetObject.transform.parent);

                            ReParentChildren(targetObject, customizableSkinTransform);
                            Object.DestroyImmediate(targetObject.gameObject);
                        }
                        else {
                            Debug.LogWarning($"Can't locate target hierarchy path for skin: {customizableSkinTransform.name}");
                        }

                        break;
                }
            }

            ListPool<Transform>.Release(reParentArray);
            foreach (var kvp in s_skinMap) {
                var pathNames = kvp.Value;
                ListPool<string>.Release(pathNames);
            }

            s_skinMap.Clear();
        }

        private static void AddToSkinMap(Transform skinObject, Transform root) {
            var pathNames = ListPool<string>.Get();
            pathNames.Add(skinObject.name);
            s_skinMap.Add(skinObject, pathNames);

            var parent = skinObject.parent;
            while (parent != root) {
                pathNames.Add(parent.name);
                parent = parent.parent;
            }
        }

        private static Transform GetMatchedGameObjectOnTarget(Transform skinObject, Transform target) {
            Transform result = null;
            var skinPath = s_skinMap[skinObject];
            for (var i = skinPath.Count - 1; i >= 0; i--) {
                var name = skinPath[i];
                result = target.Find(name);
                if (result == null)
                    return null;

                target = result;
            }

            return result;
        }

        private static void ReParentChildren(Transform source, Transform target) {
            var childrenToReParent = new List<Transform>();
            foreach (Transform child in source) {
                childrenToReParent.Add(child);
            }

            foreach (var child in childrenToReParent) {
                child.SetParent(target);
            }
        }

        public static string GetGameObjectPath(Transform gameObject, Transform root) {
            var pathNameBuilder = new StringBuilder();
            pathNameBuilder.Append(gameObject.name);
            var parent = gameObject.parent;
            while (parent != root) {
                pathNameBuilder.Insert(0, $"{parent.name}/");
                parent = parent.parent;
            }

            return pathNameBuilder.ToString();
        }
    }
}
