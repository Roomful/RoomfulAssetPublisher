using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RF.AssetWizzard {
	public class MeshCollector : ICollector {

		public void Run(PropAsset propAsset) {
#if UNITY_EDITOR

			MeshFilter[] meshes = propAsset.gameObject.GetComponentsInChildren<MeshFilter> ();

			for (int i = 0; i < meshes.Length; i++) {
				Mesh newmesh = new Mesh();
				newmesh.vertices = meshes[i].sharedMesh.vertices;
				newmesh.triangles = meshes[i].sharedMesh.triangles;
				newmesh.uv = meshes[i].sharedMesh.uv;
				newmesh.normals = meshes[i].sharedMesh.normals;
				newmesh.colors = meshes[i].sharedMesh.colors;
				newmesh.tangents = meshes[i].sharedMesh.tangents;
				newmesh.name = meshes[i].sharedMesh.name;

				PropDataBase.SaveAsset<Mesh> (propAsset, newmesh);
				meshes[i].sharedMesh = PropDataBase.LoadAsset<Mesh> (propAsset, newmesh.name);
			}

#endif
		}
	}
}