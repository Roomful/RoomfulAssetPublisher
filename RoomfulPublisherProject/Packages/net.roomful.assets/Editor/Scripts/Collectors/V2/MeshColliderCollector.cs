using net.roomful.assets.editor.extensions;
using UnityEngine;

namespace net.roomful.assets.editor
{
    class MeshColliderCollector : BaseCollector
    {
        public override void Run(IAssetBundle asset)
        {
            var meshColliders = asset.gameObject.GetComponentsInChildren<MeshCollider>(true);

            for (var i = 0; i < meshColliders.Length; i++) {
                if (meshColliders[i].sharedMesh != null) {
                    meshColliders[i].sharedMesh = AssetDatabase.SaveMesh(asset, meshColliders[i].sharedMesh);
                }

                if (meshColliders[i].sharedMaterial != null)
                {
                    meshColliders[i].sharedMaterial = AssetDatabase.SavePhysicMaterial(asset, meshColliders[i].sharedMaterial);
                }
            }
        }
    }
}
