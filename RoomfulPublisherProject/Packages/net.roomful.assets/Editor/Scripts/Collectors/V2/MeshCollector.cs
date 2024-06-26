using net.roomful.assets.editor.extensions;
using UnityEngine;

namespace net.roomful.assets.editor
{
    internal class MeshCollector : BaseCollector
    {
        public override void Run(IAssetBundle asset) {
            var meshes = asset.gameObject.GetComponentsInChildren<MeshFilter>(true);

            for (var i = 0; i < meshes.Length; i++) {
                if (meshes[i].sharedMesh != null) {
                    meshes[i].sharedMesh = AssetDatabase.SaveMesh(asset, meshes[i].sharedMesh);
                }
            }

            var skinnedMeshRenderers = asset.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);

            for (var i = 0; i < skinnedMeshRenderers.Length; i++) {
                if (skinnedMeshRenderers[i].sharedMesh != null) {
                    skinnedMeshRenderers[i].sharedMesh = AssetDatabase.SaveMesh(asset, skinnedMeshRenderers[i].sharedMesh);
                }
            }
        }
    }
}