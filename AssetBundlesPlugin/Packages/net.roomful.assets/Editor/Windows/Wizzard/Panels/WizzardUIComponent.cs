using StansAssets.Plugins.Editor;
using UnityEditor;
using UnityEngine;


namespace RF.AssetWizzard.Editor
{
    public abstract class WizzardUIComponent  {

        private int m_rotationAnimatorAgnle = 0;


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

            IMGUIToggleStyle.YesNoBool initialValue = IMGUIToggleStyle.YesNoBool.Yes;
            if (!value) {
                initialValue = IMGUIToggleStyle.YesNoBool.No;
            }
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(title, GUILayout.Width(width1));

            initialValue = (IMGUIToggleStyle.YesNoBool)EditorGUILayout.EnumPopup(initialValue, GUILayout.Width(width2));
            if (initialValue == IMGUIToggleStyle.YesNoBool.Yes) {
                value = true;
            } else {
                value = false;
            }
            EditorGUILayout.EndHorizontal();

            return value;
        }

    }
}
