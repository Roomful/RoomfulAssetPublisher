using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace net.roomful.assets.Editor
{
	public class MeshCollector : BaseCollector {

		public override void Run(IAsset asset) {
			MeshFilter[] meshes = asset.gameObject.GetComponentsInChildren<MeshFilter> (true);

			for (int i = 0; i < meshes.Length; i++)
			{
				if (meshes[i].sharedMesh != null) {
					meshes[i].sharedMesh = SaveMesh(asset, meshes[i].sharedMesh);
				}
			}
			
			SkinnedMeshRenderer[] skinnedMeshRenderers = asset.gameObject.GetComponentsInChildren<SkinnedMeshRenderer> (true);

			for (int i = 0; i < skinnedMeshRenderers.Length; i++)
			{
				
				if (skinnedMeshRenderers[i].sharedMesh != null) {
					skinnedMeshRenderers[i].sharedMesh = SaveMesh(asset, skinnedMeshRenderers[i].sharedMesh);
				}
			}
		}

		private Mesh SaveMesh(IAsset asset, Mesh oldSharedMesh)
		{
            Mesh newmesh = new Mesh();
			newmesh.vertices = oldSharedMesh.vertices;
			newmesh.triangles = oldSharedMesh.triangles;
			newmesh.uv = oldSharedMesh.uv;
			newmesh.normals = oldSharedMesh.normals;
			newmesh.colors = oldSharedMesh.colors;
			newmesh.tangents = oldSharedMesh.tangents;
			newmesh.name = oldSharedMesh.name;
            newmesh.subMeshCount = oldSharedMesh.subMeshCount;
            for (var i = 0 ; i < oldSharedMesh.subMeshCount; i++) {
                newmesh.SetTriangles(oldSharedMesh.GetTriangles(i), i);
            }
			AssetDatabase.SaveAsset<Mesh>(asset, newmesh);
			return AssetDatabase.LoadAsset<Mesh>(asset, newmesh.name);
		}
	}
}