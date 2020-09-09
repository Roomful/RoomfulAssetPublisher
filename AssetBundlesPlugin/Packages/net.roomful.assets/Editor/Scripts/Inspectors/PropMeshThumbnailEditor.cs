using System.Collections.Generic;
using StansAssets.Plugins.Editor;
using UnityEngine;
using UnityEditor;

namespace net.roomful.assets.Editor
{
    [CustomEditor(typeof(PropMeshThumbnail))]
    internal class PropMeshThumbnailEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI() {
            EditorGUI.BeginChangeCheck();
            {
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("Switch between different image ratio, to make sure your thumbnail will look good with any image", MessageType.Info);

                var toolbarSize = new[] { GUILayout.Height(30), GUILayout.Width(250) };
                Model.ImageIndex = GUILayout.Toolbar(Model.ImageIndex, ImagesContent, toolbarSize);
            }
            if (EditorGUI.EndChangeCheck()) {
                Model.Thumbnail = ImagesContent[Model.ImageIndex].image as Texture2D;
                Model.Update();
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Options", MessageType.Info);

            Model.Settings.IsBoundToResourceIndex = IMGUILayout.ToggleFiled("Bound To Resource Index", Model.Settings.IsBoundToResourceIndex, IMGUIToggleStyle.ToggleType.EnabledDisabled);
            if (Model.Settings.IsBoundToResourceIndex) {
                Model.Settings.ResourceIndex = EditorGUILayout.IntField("Resource Index", Model.Settings.ResourceIndex);
                Model.Settings.IsLogo = IMGUILayout.ToggleFiled("Is Logo", Model.Settings.IsLogo, IMGUIToggleStyle.ToggleType.EnabledDisabled);
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

        private GUIContent[] m_imagesContent = null;

        private GUIContent[] ImagesContent {
            get {
                if (m_imagesContent == null) {
                    var content = new List<GUIContent>();

                    var c = new GUIContent("", Square);
                    content.Add(c);

                    c = new GUIContent("", Landscape);
                    content.Add(c);

                    c = new GUIContent("", Portrait);
                    content.Add(c);
                    m_imagesContent = content.ToArray();
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

        private Texture2D Portrait {
            get {
                var tex = Resources.Load("logo_portrait") as Texture2D;
                return tex;
            }
        }

        private PropMeshThumbnail Model => (PropMeshThumbnail) target;
    }
}