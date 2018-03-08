using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;


namespace RF.AssetWizzard.Editor {

	[CustomEditor(typeof(PropThumbnail))]
	public class PropThumbnailEditor : UnityEditor.Editor {


		public override void OnInspectorGUI() {

			EditorGUI.BeginChangeCheck(); {

				EditorGUILayout.Space ();
				EditorGUILayout.HelpBox ("Switch between different image ratios, to make sure your thumbnail will look good with any image", MessageType.Info);

				GUILayoutOption[] toolbarSize = new GUILayoutOption[]{GUILayout.Height(30), GUILayout.Width(250)};
				Model.ImageIndex = GUILayout.Toolbar (Model.ImageIndex, ImagesCintent, toolbarSize);


				//EditorGUILayout.Space ();
				//EditorGUILayout.Space ();
				//EditorGUILayout.HelpBox ("Add part of the frame 3D parts (Not Required)", MessageType.Info);

				//Model.Border = (GameObject) EditorGUILayout.ObjectField("Border (top)", Model.Border, typeof (GameObject), true);
			//	Model.Corner = (GameObject) EditorGUILayout.ObjectField("Corner (top / left)", Model.Corner, typeof (GameObject), true);



				EditorGUILayout.Space ();
				EditorGUILayout.Space ();
				EditorGUILayout.HelpBox ("Scaling Options", MessageType.Info);


				Model.Settings.IsFixedRatio = SA.Common.Editor.Tools.ToggleFiled ("Fixed Ratio", Model.Settings.IsFixedRatio);



				if(Model.Settings.IsFixedRatio) {
					Model.Settings.XRatio = EditorGUILayout.IntField ("X", Model.Settings.XRatio);
					Model.Settings.YRatio = EditorGUILayout.IntField ("Y", Model.Settings.YRatio);
				}



				Model.Settings.IsBoundToResourceIndex = SA.Common.Editor.Tools.ToggleFiled("Bound To Resource Index", Model.Settings.IsBoundToResourceIndex);
				if (Model.Settings.IsBoundToResourceIndex) {
					Model.Settings.ResourceIndex = EditorGUILayout.IntField("Resource Index", Model.Settings.ResourceIndex);
                }


				EditorGUILayout.Space ();
				EditorGUILayout.Space ();
				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.Space ();


				if(Model.Frame == null) {
					bool pressed = GUILayout.Button ("Add Frame", EditorStyles.miniButton, new GUILayoutOption[] {GUILayout.Width(120)});
					if(pressed) {
						EditorMenu.AddFrame ();
					}
				} else {
					bool pressed = GUILayout.Button ("Remove Frame", EditorStyles.miniButton, new GUILayoutOption[] {GUILayout.Width(120)});
					if(pressed) {
						DestroyImmediate (Model.Frame);
					}
				}



				EditorGUILayout.EndHorizontal ();

            } if(EditorGUI.EndChangeCheck()) {
				var thumbnail = ImagesCintent [Model.ImageIndex].image as Texture2D;
				Model.SetThumbnail (thumbnail);
				Model.Update ();

				Scene.Update ();
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


		private PropThumbnail Model {
			get {
				return (PropThumbnail) target;
			}
		}

	}

}
