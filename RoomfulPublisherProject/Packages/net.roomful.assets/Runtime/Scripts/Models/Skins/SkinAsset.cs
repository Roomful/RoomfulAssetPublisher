using System.Collections.Generic;
using net.roomful.assets.serialization;
using UnityEngine;

namespace net.roomful.assets
{
    internal class SkinAsset : PropAsset
    {
        protected override void CheckHierarchy() { }

        private void SetCustomizableSkinGameObjectTypeWithoutOverride(GameObject go, CustomizableSkinGameObjectType type) {
            if (go.GetComponent<SerializedCustomizableSkinGameObject>() == null) {
                var customizableSkinGameObject = go.AddComponent<SerializedCustomizableSkinGameObject>();
                customizableSkinGameObject.Type = type;
            }
        }

        public override void PrepareForUpload() {
            var root = GetLayer(HierarchyLayers.Graphics);
            var variantTransforms = new List<Transform>();
            foreach (var rend in SkinModel.Variant.GameObjects) {
                variantTransforms.Add(rend.transform);

                // We won't replace if SerializedCustomizableSkinGameObject if it already exists with another custom flag
                // Like HierarchyRootTarget for example
                SetCustomizableSkinGameObjectTypeWithoutOverride(rend.transform.gameObject, CustomizableSkinGameObjectType.ReplaceTarget);
            }

            //Let's check if we already have some skin markers
            var customizableSkinGameObjects = root.GetComponentsInChildren<SerializedCustomizableSkinGameObject>();
            foreach (var customizableSkinGameObject in customizableSkinGameObjects) {
                if (customizableSkinGameObject.Type == CustomizableSkinGameObjectType.MountedPoint) {
                    var childTransforms = customizableSkinGameObject.GetComponentsInChildren<Transform>();
                    foreach (var childTransform in childTransforms) {
                        SetCustomizableSkinGameObjectTypeWithoutOverride(childTransform.gameObject, CustomizableSkinGameObjectType.SkinChild);
                    }
                }
            }

            // Mark Required Transforms
            foreach (var rend in SkinModel.Variant.GameObjects) {
                var target = rend.transform;
                while (target != root) {
                    target = target.parent;
                    if (!variantTransforms.Contains(target)) {
                        SetCustomizableSkinGameObjectTypeWithoutOverride(target.gameObject, CustomizableSkinGameObjectType.Dummy);
                    }
                }
            }

            // Clean non-required transforms and components
            var transforms = root.GetComponentsInChildren<Transform>();
            foreach (var target in transforms) {
                if (target == null)
                    continue;

                var customizableSkinGameObject = target.GetComponent<SerializedCustomizableSkinGameObject>();
                if (customizableSkinGameObject != null) {
                    switch (customizableSkinGameObject.Type) {
                        case CustomizableSkinGameObjectType.Dummy:
                        case CustomizableSkinGameObjectType.MountedPoint:
                            // Clean up components
                            var components = target.GetComponents<Component>();
                            foreach (var component in components) {
                                if (component is Transform)
                                    continue;

                                if (component is SerializedCustomizableSkinGameObject)
                                    continue;

                                DestroyImmediate(component);
                            }

                            break;
                        case CustomizableSkinGameObjectType.ReplaceMaterial:
                            // Clean up components
                            var replaceMaterialComponents = target.GetComponents<Component>();
                            foreach (var component in replaceMaterialComponents) {
                                if (component is Transform)
                                    continue;

                                if (component is Renderer)
                                    continue;

                                if (component is SerializedCustomizableSkinGameObject)
                                    continue;

                                DestroyImmediate(component);
                            }

                            break;
                    }
                }
                else {
                    // Delete skin unrelated object
                    DestroyImmediate(target.gameObject);
                }
            }

            base.PrepareForUpload();
        }

        public PropSkinUploadModel SkinModel => (PropSkinUploadModel) _Template;
    }

    public class RequiredTransform : MonoBehaviour
    { }
}
