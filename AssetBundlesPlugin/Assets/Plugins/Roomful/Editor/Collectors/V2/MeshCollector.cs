using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RF.AssetWizzard.Editor
{
	public class MeshCollector : ICollector {

		public void Run(IAsset asset) {
			MeshFilter[] meshes = asset.gameObject.GetComponentsInChildren<MeshFilter> (true);

			for (int i = 0; i < meshes.Length; i++)
			{
				meshes[i].sharedMesh = SaveMesh(asset, meshes[i].sharedMesh);
			}
			
			SkinnedMeshRenderer[] skinnedMeshRenderers = asset.gameObject.GetComponentsInChildren<SkinnedMeshRenderer> (true);

			for (int i = 0; i < skinnedMeshRenderers.Length; i++)
			{
				skinnedMeshRenderers[i].sharedMesh = SaveMesh(asset, skinnedMeshRenderers[i].sharedMesh);
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
			AssetDatabase.SaveAsset<Mesh>(asset, newmesh);
			return AssetDatabase.LoadAsset<Mesh>(asset, newmesh.name);
		}
	}
}