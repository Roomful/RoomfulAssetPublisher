using System.Collections.Generic;
using net.roomful.assets.serialization;
using StansAssets.Plugins.Editor;
using UnityEngine;
using UnityEditor;

namespace net.roomful.assets.editor
{
    [CustomEditor(typeof(PropThumbnail))]
    internal class PropThumbnailEditor : Editor
    {
        public override void OnInspectorGUI() {
            EditorGUI.BeginChangeCheck();
            {
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("(For preview purposes)\nSwitch between different ratios, to see how images will look like based on selected Scale Mode", MessageType.Info);

                var toolbarSize = new[] { GUILayout.Height(30), GUILayout.Width(250) };
                Model.ImageIndex = GUILayout.Toolbar(Model.ImageIndex, ImagesContent, toolbarSize);

                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("Scaling Options", MessageType.Info);

                Model.Settings.ScaleMode = (ThumbnailScaleMode) IMGUILayout.EnumPopup("Scale Mode", Model.Settings.ScaleMode);
                if (Model.Settings.ScaleMode == ThumbnailScaleMode.DoNotScale) {
                    Model.Settings.XRatio = EditorGUILayout.IntField("X", Model.Settings.XRatio);
                    Model.Settings.YRatio = EditorGUILayout.IntField("Y", Model.Settings.YRatio);
                }
                
                Model.Settings.IsBoundToUserAvatar = IMGUILayout.ToggleFiled("Bound To User Id", Model.Settings.IsBoundToUserAvatar, IMGUIToggleStyle.ToggleType.YesNo);
                Model.Settings.IsBoundToResourceIndex = IMGUILayout.ToggleFiled("Bound To Resource Index", Model.Settings.IsBoundToResourceIndex, IMGUIToggleStyle.ToggleType.YesNo);
                if (Model.Settings.IsBoundToResourceIndex) {
                    Model.Settings.ResourceIndex = IMGUILayout.IntField("Resource Index", Model.Settings.ResourceIndex);
                }

                Model.Settings.IsLogo = IMGUILayout.ToggleFiled("Is Logo", Model.Settings.IsLogo, IMGUIToggleStyle.ToggleType.YesNo);


                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space();

                if (Model.Frame == null) {
                    var pressed = GUILayout.Button("Add Stretched Frame", EditorStyles.miniButton, GUILayout.Width(130));
                    if (pressed) {
                        EditorMenu.AddStretchedFrame();
                    }

                    pressed = GUILayout.Button("Add Tiled Frame", EditorStyles.miniButton, GUILayout.Width(130));
                    if (pressed) {
                        EditorMenu.AddTiledFrame();
                    }
                }
                else {
                    var pressed = GUILayout.Button("Remove Frame", EditorStyles.miniButton, GUILayout.Width(120));
                    if (pressed) {
                        DestroyImmediate(Model.Frame);
                    }
                }

                EditorGUILayout.EndHorizontal();
            }
            if (EditorGUI.EndChangeCheck()) {
                var thumbnail = ImagesContent[Model.ImageIndex].image as Texture2D;
                Model.SetThumbnail(thumbnail);
                Model.Update();

                var frame = Model.GetComponent<AbstractPropFrame>();
                if (frame != null) {
                    frame.UpdateFrame();
                }

                Scene.Update();
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

        private GUIContent[] m_imagesContent = null;

        private GUIContent[] ImagesContent {
            get {
                if (m_imagesContent == null) {
                    var contents = new List<GUIContent>();

                    var c = new GUIContent("", Square);
                    contents.Add(c);

                    c = new GUIContent("", Landscape);
                    contents.Add(c);

                    c = new GUIContent("", Portrait);
                    contents.Add(c);

                    c = new GUIContent("", Landscape2);
                    contents.Add(c);

                    m_imagesContent = contents.ToArray();
                }

                return m_imagesContent;
            }
        }

        private Texture2D Square {
            get {
                var tex = Resources.Load("logo_square") as Texture2D;
                return tex;
            }
        }

        private Texture2D Landscape {
            get {
                var tex = Resources.Load("logo_landscape") as Texture2D;
                return tex;
            }
        }

        private Texture2D Landscape2 {
            get {
                var tex = Resources.Load("logo_landscape_2") as Texture2D;
                return tex;
            }
        }

        private Texture2D Portrait {
            get {
                var tex = Resources.Load("logo_portrait") as Texture2D;
                return tex;
            }
        }

        private PropThumbnail Model => (PropThumbnail) target;
    }
}
