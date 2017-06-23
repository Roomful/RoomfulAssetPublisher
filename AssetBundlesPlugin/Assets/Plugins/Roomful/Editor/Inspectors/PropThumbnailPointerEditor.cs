using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;


namespace RF.AssetWizzard.Editor {

	[CustomEditor(typeof(PropThumbnailPointer))]
	public class PropThumbnailPointerEditor : UnityEditor.Editor {


		public override void OnInspectorGUI() {

			EditorGUI.BeginChangeCheck(); {

				EditorGUILayout.Space ();
				EditorGUILayout.HelpBox ("Swithc between diffrent image ratio, to make sure your thumbnail will look good with any image", MessageType.Info);

				GUILayoutOption[] toolbarSize = new GUILayoutOption[]{GUILayout.Height(30), GUILayout.Width(250)};
				Model.ImageIndex = GUILayout.Toolbar (Model.ImageIndex, ImagesCintent, toolbarSize);

				
			} if(EditorGUI.EndChangeCheck()) {
				Model.Thumbnail = ImagesCintent [Model.ImageIndex].image as Texture2D;
				Model.Update ();
			}


			EditorGUILayout.Space ();
			EditorGUILayout.Space ();

		}

		private GUIContent[] _ImagesCintent = null;

		private GUIContent[] ImagesCintent {
			get {

				if(_ImagesCintent ==  null) {
					List<GUIContent> conetnt = new List<GUIContent> ();

					GUIContent c = new GUIContent ("", sqaure);
					conetnt.Add (c);

					c = new GUIContent ("", landscape);
					conetnt.Add (c);

					c = new GUIContent ("", portrait);
					conetnt.Add (c);
					_ImagesCintent = conetnt.ToArray ();
				}

				return _ImagesCintent;
					
			}
		}



		private Texture2D sqaure {
			get {
				Texture2D tex = Resources.Load ("logo_square") as Texture2D;
				return tex;
			}
		}


		private Texture2D landscape {
			get {
				Texture2D tex = Resources.Load ("logo_landscape") as Texture2D;
				return tex;
			}
		}


		private Texture2D portrait {
			get {
				Texture2D tex = Resources.Load ("logo_portrait") as Texture2D;
				return tex;
			}
		}


		private PropThumbnailPointer Model {
			get {
				return (PropThumbnailPointer) target;
			}
		}

	}

}
