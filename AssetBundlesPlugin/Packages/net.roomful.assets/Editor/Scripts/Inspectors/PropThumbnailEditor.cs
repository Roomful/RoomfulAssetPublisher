﻿using System.Collections.Generic;
using StansAssets.Plugins.Editor;
using UnityEngine;

using UnityEditor;


namespace net.roomful.assets.Editor {

	[CustomEditor(typeof(PropThumbnail))]
	internal class PropThumbnailEditor : UnityEditor.Editor {


		public override void OnInspectorGUI() {

			EditorGUI.BeginChangeCheck(); {

				EditorGUILayout.Space ();
				EditorGUILayout.HelpBox ("Switch between different image ratios, to make sure your thumbnail will look good with any image", MessageType.Info);

				var toolbarSize = new[]{GUILayout.Height(30), GUILayout.Width(250)};
				Model.ImageIndex = GUILayout.Toolbar (Model.ImageIndex, ImagesCintent, toolbarSize);


				EditorGUILayout.Space ();
				EditorGUILayout.Space ();
				EditorGUILayout.HelpBox ("Scaling Options", MessageType.Info);


				Model.Settings.IsFixedRatio = IMGUILayout.ToggleFiled ("Fixed Ratio", Model.Settings.IsFixedRatio, IMGUIToggleStyle.ToggleType.EnabledDisabled);



				if(Model.Settings.IsFixedRatio) {
					Model.Settings.XRatio = EditorGUILayout.IntField ("X", Model.Settings.XRatio);
					Model.Settings.YRatio = EditorGUILayout.IntField ("Y", Model.Settings.YRatio);
				}



				Model.Settings.IsBoundToResourceIndex = IMGUILayout.ToggleFiled("Bound To Resource Index", Model.Settings.IsBoundToResourceIndex, IMGUIToggleStyle.ToggleType.EnabledDisabled);
				if (Model.Settings.IsBoundToResourceIndex) {
					Model.Settings.ResourceIndex = EditorGUILayout.IntField("Resource Index", Model.Settings.ResourceIndex);
                }

            

                EditorGUILayout.Space ();
				EditorGUILayout.Space ();
				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.Space ();


				if(Model.Frame == null) {
					var pressed = GUILayout.Button ("Add Stretched Frame", EditorStyles.miniButton, GUILayout.Width(130));
					if(pressed) {
						EditorMenu.AddStretchedFrame ();
					}

                    pressed = GUILayout.Button("Add Tiled Frame", EditorStyles.miniButton, GUILayout.Width(130));
                    if (pressed) {
                        EditorMenu.AddTiledFrame();
                    }
                } else {
					var pressed = GUILayout.Button ("Remove Frame", EditorStyles.miniButton, GUILayout.Width(120));
					if(pressed) {
						DestroyImmediate (Model.Frame);
					}
				}



				EditorGUILayout.EndHorizontal ();

            } if(EditorGUI.EndChangeCheck()) {
				var thumbnail = ImagesCintent [Model.ImageIndex].image as Texture2D;
				Model.SetThumbnail (thumbnail);
				Model.Update ();

                var frame = Model.GetComponent<AbstractPropFrame>();
                if(frame != null) {
                    frame.UpdateFrame();
                }

				Scene.Update ();
            }


			EditorGUILayout.Space ();
			EditorGUILayout.Space ();

		}

		private GUIContent[] _ImagesCintent = null;

		private GUIContent[] ImagesCintent {
			get {

				if(_ImagesCintent ==  null) {
					var conetnt = new List<GUIContent> ();

					var c = new GUIContent ("", sqaure);
					conetnt.Add (c);

					c = new GUIContent ("", landscape);
					conetnt.Add (c);

					c = new GUIContent ("", portrait);
					conetnt.Add (c);


                    c = new GUIContent("", landscape_2);
                    conetnt.Add(c);

                    _ImagesCintent = conetnt.ToArray ();
				}

				return _ImagesCintent;
					
			}
		}



		private Texture2D sqaure {
			get {
				var tex = Resources.Load ("logo_square") as Texture2D;
				return tex;
			}
		}


		private Texture2D landscape {
			get {
				var tex = Resources.Load ("logo_landscape") as Texture2D;
				return tex;
			}
		}

        private Texture2D landscape_2 {
            get {
                var tex = Resources.Load("logo_landscape_2") as Texture2D;
                return tex;
            }
        }


        

        private Texture2D portrait {
			get {
				var tex = Resources.Load ("logo_portrait") as Texture2D;
				return tex;
			}
		}


		private PropThumbnail Model => (PropThumbnail) target;
	}

}