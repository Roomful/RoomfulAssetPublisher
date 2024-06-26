using StansAssets.Plugins.Editor;
using UnityEditor;
using UnityEngine;

namespace net.roomful.assets.editor
{
    abstract class WizardUIComponent
    {
        int m_RotationAnimatorAngle = 0;

        public void DrawPreloaderAt(Rect rect) {
            var preloader = IconManager.GetIcon(Icon.loader);

            m_RotationAnimatorAngle++;

            if (m_RotationAnimatorAngle > 360) {
                m_RotationAnimatorAngle = 0;
            }

            GUIUtility.RotateAroundPivot(m_RotationAnimatorAngle, rect.center);
            GUI.DrawTexture(rect, preloader);
            GUI.matrix = Matrix4x4.identity;
        }

        protected bool YesNoFiled(string title, bool value, int width1, int width2) {
            var initialValue = IMGUIToggleStyle.YesNoBool.Yes;
            if (!value) {
                initialValue = IMGUIToggleStyle.YesNoBool.No;
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(title, GUILayout.Width(width1));

            initialValue = (IMGUIToggleStyle.YesNoBool) EditorGUILayout.EnumPopup(initialValue, GUILayout.Width(width2));
            if (initialValue == IMGUIToggleStyle.YesNoBool.Yes) {
                value = true;
            }
            else {
                value = false;
            }

            EditorGUILayout.EndHorizontal();

            return value;
        }
    }
}