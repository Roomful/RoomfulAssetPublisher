using UnityEditor;
using UnityEngine;

namespace RF.AssetWizzard.Editor
{

    public abstract class Panel : IPanel
    {

        private int m_rotationAnimatorAgnle = 0;
        private readonly EditorWindow m_window;

        public Panel(EditorWindow window) {
            m_window = window;
        }


        public abstract void OnGUI();


        public EditorWindow Window {
            get {
                return m_window;
            }
        }




        protected void DrawPreloaderAt(Rect rect) {
            Texture2D preloader = IconManager.GetIcon(Icon.loader);

            m_rotationAnimatorAgnle++;

            if (m_rotationAnimatorAgnle > 360) {
                m_rotationAnimatorAgnle = 0;
            }

            GUIUtility.RotateAroundPivot(m_rotationAnimatorAgnle, rect.center);
            GUI.DrawTexture(rect, preloader);
            GUI.matrix = Matrix4x4.identity;
        }


        protected bool YesNoFiled(string title, bool value, int width1, int width2) {

            SA.Common.Editor.SA_YesNoBool initialValue = SA.Common.Editor.SA_YesNoBool.Yes;
            if (!value) {
                initialValue = SA.Common.Editor.SA_YesNoBool.No;
            }
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(title, GUILayout.Width(width1));

            initialValue = (SA.Common.Editor.SA_YesNoBool)EditorGUILayout.EnumPopup(initialValue, GUILayout.Width(width2));
            if (initialValue == SA.Common.Editor.SA_YesNoBool.Yes) {
                value = true;
            } else {
                value = false;
            }
            EditorGUILayout.EndHorizontal();

            return value;
        }

       
    }
}