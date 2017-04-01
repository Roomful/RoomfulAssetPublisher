using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard {

	#if UNITY_EDITOR
	[ExecuteInEditMode]
	#endif
	public class PropAsset : MonoBehaviour {
		
		[SerializeField] [HideInInspector]
		private AssetTemplate _Template;

		//--------------------------------------
		// Initialization
		//--------------------------------------

		//--------------------------------------
		// Unity Editor
		//--------------------------------------
	
		//--------------------------------------
		// Public Methods
		//--------------------------------------

		public void SetTemplate(AssetTemplate tpl) {
			_Template = tpl;
		}

		//--------------------------------------
		// Get / Set
		//--------------------------------------


		public AssetTemplate Template {
			get {
				if (_Template == null) {
					_Template = new AssetTemplate ();
				}

				return _Template;
			}
		}
	}
}