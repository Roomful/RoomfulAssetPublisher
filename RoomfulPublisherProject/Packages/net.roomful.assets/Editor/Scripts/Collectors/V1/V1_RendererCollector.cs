using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.assets.editor
{
    internal class V1_RendererCollector : BaseCollector
    {
        public override void Run(IAssetBundle asset) {
            var renderers = asset.gameObject.GetComponentsInChildren<Renderer>(true);
            foreach (var ren in renderers) {
                if (ren.sharedMaterials.Length > 0) {
                    var recreatedMaterials = new List<Material>();

                    foreach (var m in ren.sharedMaterials) {
                        if (m == null) {
                            continue;
                        }

                        var newMaterial = new Material(m);
                        newMaterial.shader = Shader.Find(newMaterial.shader.name);

                        newMaterial.name = m.name.Replace("/", "");

                        AssetDatabase.SaveAsset(asset, newMaterial);

                        var shadersPropertyLength = UnityEditor.ShaderUtil.GetPropertyCount(newMaterial.shader);
                        for (var i = 0; i < shadersPropertyLength; i++) {
                            var propertyName = UnityEditor.ShaderUtil.GetPropertyName(newMaterial.shader, i);
                            var propertyType = UnityEditor.ShaderUtil.GetPropertyType(newMaterial.shader, i);

                            if (propertyType == UnityEditor.ShaderUtil.ShaderPropertyType.TexEnv) {
                                var tex = newMaterial.GetTexture(propertyName);

                                if (tex != null) {
                                    var texName = tex.name;

                                    AssetDatabase.SaveAsset(asset, tex);

                                    if (propertyName.Equals("_BumpMap")) {
                                        ReimportTextureAsNormalMap(asset, texName);
                                    }

                                    AssetDatabase.LoadAsset<Material>(asset, newMaterial.name).SetTexture(propertyName, AssetDatabase.LoadAsset<Texture>(asset, texName));
                                }
                            }

                            if (propertyName.Equals("_Mode")) {
                                var renderMode = (int) newMaterial.GetFloat(propertyName);
                                switch (renderMode) {
                                    case 0: //Opaque
                                        AssetDatabase.LoadAsset<Material>(asset, newMaterial.name).renderQueue = -1;
                                        break;
                                    case 1: // Cut out
                                        AssetDatabase.LoadAsset<Material>(asset, newMaterial.name).renderQueue = 2450;
                                        break;
                                    case 2: // Fade
                                        AssetDatabase.LoadAsset<Material>(asset, newMaterial.name).renderQueue = 3000;
                                        break;
                                    case 3: // Transparent
                                        AssetDatabase.LoadAsset<Material>(asset, newMaterial.name).renderQueue = 3000;
                                        break;
                                }
                            }
                        }

                        recreatedMaterials.Add(AssetDatabase.LoadAsset<Material>(asset, newMaterial.name));
                    }

                    ren.materials = recreatedMaterials.ToArray();
                }
            }
        }

        private void ReimportTextureAsNormalMap(IAssetBundle asset, string texName) {
            var path = UnityEditor.AssetDatabase.GetAssetPath(AssetDatabase.LoadAsset<Texture>(asset, texName));

            var ti = (UnityEditor.TextureImporter) UnityEditor.AssetImporter.GetAtPath(path);
            var settings = new UnityEditor.TextureImporterSettings();
            ti.ReadTextureSettings(settings);
            settings.textureType = UnityEditor.TextureImporterType.NormalMap;
            ti.SetTextureSettings(settings);
            ti.SaveAndReimport();
        }
    }
}