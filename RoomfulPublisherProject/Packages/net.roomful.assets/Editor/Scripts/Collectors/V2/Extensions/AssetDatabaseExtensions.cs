using UnityEngine;

namespace net.roomful.assets.editor.extensions
{
    static class AssetDatabaseExtensions
    {
        public static Mesh SaveMesh(this AssetDatabase @this, IAssetBundle asset, Mesh source) {
            var mesh = new Mesh
            {
                vertices = source.vertices,
                triangles = source.triangles,
                uv = source.uv,
                uv2 = source.uv2,
                normals = source.normals,
                colors = source.colors,
                tangents = source.tangents,
                name = source.name,
                subMeshCount = source.subMeshCount,
                bindposes = source.bindposes,
                boneWeights = source.boneWeights
            };

            for (var i = 0; i < source.subMeshCount; i++) {
                mesh.SetTriangles(source.GetTriangles(i), i);
            }

            @this.SaveAsset(asset, mesh);
            return @this.LoadAsset<Mesh>(asset, mesh.name);
        }

        public static PhysicMaterial SavePhysicMaterial(this AssetDatabase @this, IAssetBundle asset, PhysicMaterial source)
        {
            var physicMaterial = new PhysicMaterial
            {
                name = source.name,
                bounciness = source.bounciness,
                bounceCombine = source.bounceCombine,
                frictionCombine = source.frictionCombine,
                staticFriction = source.staticFriction,
                dynamicFriction = source.dynamicFriction
            };
            
            @this.SaveAsset(asset, physicMaterial);
            return @this.LoadAsset<PhysicMaterial>(asset, physicMaterial.name);
        }
    }
}
