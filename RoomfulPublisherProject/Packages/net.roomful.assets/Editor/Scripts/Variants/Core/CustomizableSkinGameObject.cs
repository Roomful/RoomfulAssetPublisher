using System.Collections.Generic;
using System.Linq;
using net.roomful.assets.serialization;
using UnityEngine;

namespace net.roomful.assets.editor
{
    internal static class CustomizableSkinGameObject
    {
        internal static List<Renderer> SearchColoredSkin(PropAsset propAsset, PropVariant selectedVariant) {
            var result = new List<Renderer>();
            var customizableSkinGameObjects = SearchCustomizableSkinGameObjects(propAsset, selectedVariant);
            foreach (var gameObject in customizableSkinGameObjects) {
                AddCustomizableSkinGameObject(gameObject, result);
            }
            return result.Distinct().ToList();
        }

        internal static bool IsHeavySkinSkin(PropAsset propAsset, PropSkin selectedSkin) {
            var heavySkin = selectedSkin.HeavySkin;
            var root = propAsset.GetLayer(HierarchyLayers.Graphics);
            var customizableSkinGameObjects = root.GetComponentsInChildren<SerializedCustomizableSkinGameObject>();
            if (customizableSkinGameObjects.Any(customizableSkinGameObject => customizableSkinGameObject.Type == CustomizableSkinGameObjectType.MountedPoint)) {
                return true;
            }

            return heavySkin;
        }
        
        internal static List<GameObject> SearchCustomizableSkinGameObjects(PropAsset propAsset, PropVariant selectedVariant) {
            var result = new List<GameObject>();
            var root = propAsset.GetLayer(HierarchyLayers.Graphics);
            var variantTransforms = new List<Transform>();

            foreach (var gameObject in selectedVariant.GameObjects) {
                variantTransforms.Add(gameObject.transform);
                result.Add(gameObject);
            }

            var customizableSkinGameObjects = root.GetComponentsInChildren<SerializedCustomizableSkinGameObject>();
            foreach (var customizableSkinGameObject in customizableSkinGameObjects) {
                if (customizableSkinGameObject.Type == CustomizableSkinGameObjectType.MountedPoint) {
                    var childTransforms = customizableSkinGameObject.GetComponentsInChildren<Transform>();
                    foreach (var childTransform in childTransforms) {
                        result.Add(childTransform.gameObject);
                    }
                }
            }

            foreach (var rend in selectedVariant.GameObjects) {
                var target = rend.transform;
                while (target != root) {
                    target = target.parent;
                    if (!variantTransforms.Contains(target)) {
                        result.Add(target.gameObject);
                    }
                }
            }

            var transforms = root.GetComponentsInChildren<Transform>();
            foreach (var target in transforms) {
                if (target == null)
                    continue;

                var customizableSkinGameObject = target.GetComponent<SerializedCustomizableSkinGameObject>();
                if (customizableSkinGameObject != null) {
                    switch (customizableSkinGameObject.Type) {
                        case CustomizableSkinGameObjectType.Dummy:
                        case CustomizableSkinGameObjectType.MountedPoint:
                            continue;
                    }
                    result.Add(target.gameObject);
                }
            }

            return result.Distinct().ToList();
        }
        
        internal static void DisableSkinUnrelatedRenderers(PropAsset propAsset, PropVariant selectedVariant) {
            foreach (var rend in propAsset.Renderers) {
                rend.enabled = false;
            }

            var customizableSkinGameObjects = SearchCustomizableSkinGameObjects(propAsset, selectedVariant);
            foreach (var rend in customizableSkinGameObjects.Select(gameObject => 
                gameObject.GetComponent<Renderer>()).Where(rend => rend != null)) {
                rend.enabled = true;
            }
        }

        private static void AddCustomizableSkinGameObject(GameObject gameObject, List<Renderer> result) {
            var coloredSkinObject = gameObject.GetComponent<SerializedColorizedSkinObject>();
            var customizableSkinGameObject = gameObject.GetComponent<SerializedCustomizableSkinGameObject>();
            var rend = gameObject.GetComponent<Renderer>();
            if (customizableSkinGameObject == null && coloredSkinObject != null) {
                result.Add(rend);
            }
        }
    }
}