using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



namespace RF.AssetWizzard.Editor {

	[CustomEditor(typeof(StyleAsset))]
	public class StyleAssetInspector : AssetInspector<StyleTemplate, StyleAsset>
    {



		void OnEnable() {

		}


		public override void OnInspectorGUI() {

			serializedObject.Update();


            DrawActionButtons();
            serializedObject.ApplyModifiedProperties ();

		}


        public override StyleAsset Asset {
            get {
                return target as StyleAsset;
            }
        }


	
	}
}
