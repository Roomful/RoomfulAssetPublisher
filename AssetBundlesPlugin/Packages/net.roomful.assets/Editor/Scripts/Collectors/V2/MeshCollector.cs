using UnityEngine;

namespace net.roomful.assets.Editor
{
    internal class MeshCollector : BaseCollector
    {
        public override void Run(IAsset asset) {
            var meshes = asset.gameObject.GetComponentsInChildren<MeshFilter>(true);

            for (var i = 0; i < meshes.Length; i++) {
                if (meshes[i].sharedMesh != null) {
                    meshes[i].sharedMesh = SaveMesh(asset, meshes[i].sharedMesh);
                }
            }

            var skinnedMeshRenderers = asset.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);

            for (var i = 0; i < skinnedMeshRenderers.Length; i++) {
                if (skinnedMeshRenderers[i].sharedMesh != null) {
                    skinnedMeshRenderers[i].sharedMesh = SaveMesh(asset, skinnedMeshRenderers[i].sharedMesh);
                }
            }
        }

        private Mesh SaveMesh(IAsset asset, Mesh oldSharedMesh) {
            var newmesh = new Mesh();
            newmesh.vertices = oldSharedMesh.vertices;
            newmesh.triangles = oldSharedMesh.triangles;
            newmesh.uv = oldSharedMesh.uv;
            newmesh.normals = oldSharedMesh.normals;
            newmesh.colors = oldSharedMesh.colors;
            newmesh.tangents = oldSharedMesh.tangents;
            newmesh.name = oldSharedMesh.name;
            newmesh.subMeshCount = oldSharedMesh.subMeshCount;
            for (var i = 0; i < oldSharedMesh.subMeshCount; i++) {
                newmesh.SetTriangles(oldSharedMesh.GetTriangles(i), i);
            }

            AssetDatabase.SaveAsset(asset, newmesh);
            return AssetDatabase.LoadAsset<Mesh>(asset, newmesh.name);
        }
    }
}