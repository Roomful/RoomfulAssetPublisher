using System.Collections.Generic;
using UnityEngine;
using net.roomful.assets.serialization;
using UnityEditor;

namespace net.roomful.assets.editor
{
    internal class RendererCollector : BaseCollector
    {
        public override void Run(IAssetBundle asset) {
            var rens = asset.gameObject.GetComponentsInChildren<Renderer>(true);
            foreach (var ren in rens) {
                var materialsData = ren.gameObject.GetComponents<SerializedMaterial>();

                if (materialsData.Length > 0) {
                    var exportedMaterials = new List<Material>();

                    foreach (var sm in materialsData) {
                        var m = DeserializeMaterial(sm, asset);
                        exportedMaterials.Add(m);
                    }

                    ren.sharedMaterials = exportedMaterials.ToArray();

                    for (var i = materialsData.Length - 1; i >= 0; i--) {
                        Object.DestroyImmediate(materialsData[i]);
                    }
                }
            }

            var projectors = asset.gameObject.GetComponentsInChildren<Projector>();
            foreach (var p in projectors) {
                var sm = p.gameObject.GetComponent<SerializedMaterial>();

                if (sm != null) {
                    var m = DeserializeMaterial(sm, asset);
                    p.material = m;

                    Object.DestroyImmediate(sm);
                }
            }
        }

        private Material DeserializeMaterial(SerializedMaterial sm, IAssetBundle asset) {
            var newMaterial = MaterialDeserializer.CreateMaterial(sm);

            if (AssetDatabase.IsAssetExist(asset, newMaterial)) {
                return AssetDatabase.LoadAsset<Material>(asset, newMaterial.name);
            }

            foreach (var property in sm.ShadersProperties)
            {
                if (property.SerializedTextureValue != null) {
                    if (property.SerializedTextureValue.MainTexture != null) {
                        var texName = property.SerializedTextureValue.MainTexture.name;
                        if (property.SerializedTextureValue.TextureType == TextureImporterType.NormalMap.ToString())
                            AssetDatabase.SaveAsset(asset, DTXNormalMap2RGBA(property.SerializedTextureValue.MainTexture) as Texture);
                        else if (property.SerializedTextureValue.TextureShape != TextureImporterShape.TextureCube.ToString())
                            AssetDatabase.SaveAsset(asset, property.SerializedTextureValue.MainTexture);
                        
                        if (property.SerializedTextureValue.TextureShape == TextureImporterShape.TextureCube.ToString())
                            property.SerializedTextureValue.MainTexture = AssetDatabase.LoadCubemapAsset(
                                asset, asset.gameObject.GetComponent<SerializedEnvironment>());
                        else
                        {
                            var textureCollector = new TextureCollector();
                            textureCollector.SetAssetDatabase(AssetDatabase);
                            textureCollector.Run(asset, property.SerializedTextureValue);
                            property.SerializedTextureValue.MainTexture = AssetDatabase.LoadAsset<Texture>(asset, texName);
                        }
                    }
                }
            }

            // For some reason  AssetDatabase.SaveAsset(asset, property.SerializedTextureValue.MainTexture);
            // kills newMaterial instance some times. Have NO idea why, let's just make a new one.
            newMaterial = MaterialDeserializer.CreateMaterial(sm);
            MaterialDeserializer.ApplySerializedProperties(newMaterial, sm);
           
            AssetDatabase.SaveAsset(asset, newMaterial);
            
            return AssetDatabase.LoadAsset<Material>(asset, newMaterial.name);
            
        }

        private Texture2D DTXNormalMap2RGBA(Texture tex) {
            var tmp = RenderTexture.GetTemporary(tex.width, tex.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
            Graphics.Blit(tex, tmp);
            var previous = RenderTexture.active;
            RenderTexture.active = tmp;

            var myTexture2D = new Texture2D(tex.width, tex.height);
            myTexture2D.name = tex.name;
            myTexture2D.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
            myTexture2D.Apply();

            Color[] colors = myTexture2D.GetPixels();
            for(int i=0; i<colors.Length;i++) {
                Color c = colors[i];
                c.r = c.a*2-1;  //red<-alpha (x<-w)
                c.g = c.g*2-1; //green is always the same (y)
                Vector2 xy = new Vector2(c.r, c.g); //this is the xy vector
                c.b = Mathf.Sqrt(1-Mathf.Clamp01(Vector2.Dot(xy, xy))); //recalculate the blue channel (z)
                colors[i] = new Color(c.r*0.5f+0.5f, c.g*0.5f+0.5f, c.b*0.5f+0.5f); //back to 0-1 range
            }
            myTexture2D.SetPixels(colors);
            myTexture2D.Apply();
            return myTexture2D;
        }
    }
}