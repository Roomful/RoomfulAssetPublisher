using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RF.AssetWizzard {
	public static class AssetBundlesMenu {

		[MenuItem("Roomful/Modules/AssetBundlesWizzard", false, 400)]
		public static void ShowWizzrd() {
			EditorWindow window = EditorWindow.GetWindow<WizzardWindow>(true, "Asset Bundles Wizzard");

			window.minSize = new Vector2(500f, 400f);
			window.maxSize = new Vector2(window.minSize.x, window.maxSize.y);
			window.position = new Rect(new Vector2(100f, 100f), window.minSize);
			window.Show();

		}
	}
}