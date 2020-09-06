using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace net.roomful.assets {

	public static class PrefabManager  {


		public static GameObject CreatePrefab(string prefabName) {
			#if UNITY_EDITOR

			string path = AssetBundlesSettings.PLUGIN_PREFABS_LOCATION + prefabName +  ".prefab";
			Object prafabObject = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
            var PrefabInstance = (GameObject)PrefabUtility.InstantiatePrefab(prafabObject);
            //TODO check behaviour and either rethink flow either remove next line
            // PrefabUtility.DisconnectPrefabInstance(PrefabInstance);

            return PrefabInstance;


            #else
				return new GameObject(prefabName);
            #endif
        }

	}


}