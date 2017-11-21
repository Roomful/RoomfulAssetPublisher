using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RF.AssetWizzard.Editor
{
	public class MeshCollector : ICollector {

		public void Run(IAsset asset) {
			MeshFilter[] meshes = asset.gameObject.GetComponentsInChildren<MeshFilter> (true);

			for (int i = 0; i < meshes.Length; i++) {
				Mesh newmesh = new Mesh();
				newmesh.vertices = meshes[i].sharedMesh.vertices;
				newmesh.triangles = meshes[i].sharedMesh.triangles;
				newmesh.uv = meshes[i].sharedMesh.uv;
				newmesh.normals = meshes[i].sharedMesh.normals;
				newmesh.colors = meshes[i].sharedMesh.colors;
				newmesh.tangents = meshes[i].sharedMesh.tangents;
				newmesh.name = meshes[i].sharedMesh.name;

				AssetDatabase.SaveAsset<Mesh> (asset, newmesh);
				meshes[i].sharedMesh = AssetDatabase.LoadAsset<Mesh> (asset, newmesh.name);
			}
		}
	}
}