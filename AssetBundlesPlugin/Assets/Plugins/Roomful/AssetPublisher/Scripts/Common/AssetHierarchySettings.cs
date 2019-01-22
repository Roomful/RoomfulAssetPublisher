using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RF.AssetWizzard {

	public static class AssetHierarchySettings  {


	



		private static List<string> _HierarchyLayers = null;
		public static List<string> HierarchyLayers {
			get {
				if(_HierarchyLayers == null) {

					_HierarchyLayers = new List<string> ();
					foreach (HierarchyLayers layer in System.Enum.GetValues(typeof(HierarchyLayers))) {
						_HierarchyLayers.Add (layer.ToString ());
					}

				}

				return _HierarchyLayers;
			}
		}

	}
}
