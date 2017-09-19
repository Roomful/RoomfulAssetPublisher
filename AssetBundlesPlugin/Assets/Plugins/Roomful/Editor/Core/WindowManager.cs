using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



namespace RF.AssetWizzard.Editor {

	public static class WindowManager  {


		public static void ShowWizard() {

			Wizzard.minSize = new Vector2(600f, 450);
			Wizzard.maxSize = new Vector2(Wizzard.minSize.x, Wizzard.maxSize.y);
			Wizzard.position = new Rect(new Vector2(100f, 100f), Wizzard.minSize);
			Wizzard.Show();
		}


		public static void ShowCreateNewAsset() {


			EditorWindow window = EditorWindow.GetWindow<CreateAssetWindow>(true, "New Asset");

			window.minSize = new Vector2(375f, 135f);
			window.maxSize = new Vector2(window.minSize.x, window.maxSize.y);
			window.position = new Rect(new Vector2(Wizzard.position.x + 100f, Wizzard.position.y + 100f), window.minSize);
            window.Focus();

            window.Show();
		}


		public static WizardWindow Wizzard {
			get {
				return EditorWindow.GetWindow<WizardWindow>(true, "Asset Bundles Wizzard");
			}
		}

	}
}
