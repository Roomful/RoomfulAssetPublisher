using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace RF.AssetWizzard {

	public static class PrefabManager  {





		public static GameObject CreatePrefab(string prefabName) {
			#if UNITY_EDITOR

				string path = AssetBundlesSettings.PLUGIN_PREFABS_LOCATION + prefabName +  ".prefab";

				Object prafabObject = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
				return (GameObject) PrefabUtility.InstantiatePrefab (prafabObject);
			#else
				return new GameObject(prefabName);
			#endif
		}

	}


}