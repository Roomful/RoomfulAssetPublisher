using System;
using UnityEngine;
using UnityEditor;

namespace SA.Foundation.Editor
{
    [Serializable]
    public class SA_HyperLabel : SA_HyperButton
    {
        [SerializeField] GUIContent m_content;

        [SerializeField] GUIStyle m_style;
        [SerializeField] GUIStyle m_mouseOverStyle;

        [SerializeField] bool m_overrdieGuiColor = false;

       
        public SA_HyperLabel(GUIContent content) : this(content, EditorStyles.label) {}

        public SA_HyperLabel(GUIContent content, GUIStyle style) {
            m_content = content;
            m_style = new GUIStyle(style);
            m_mouseOverStyle = new GUIStyle(style);
        }



		public void SetColor(Color color) {
            m_style.normal.textColor = color;
        }

        public void SetMouseOverColor(Color color) {
            m_mouseOverStyle.normal.textColor = color;
        }



        public bool DrawWithCalcSize() {
            float width = CalcSize().x + 5f;
            return Draw(GUILayout.Width(width));
        }


        protected override void OnNormal(params GUILayoutOption[] options) {
            if(m_overrdieGuiColor) {
                using(new SA_GuiChangeColor(m_style.normal.textColor)) {
                    using (new SA_GuiChangeContentColor(m_style.normal.textColor)) {
                        EditorGUILayout.LabelField(m_content, m_style, options);
                    }
                        
                }
            } else {
                EditorGUILayout.LabelField(m_content, m_style, options);
            }
        }


        protected override void OnMouseOver(params GUILayoutOption[] options) {
            Color c = GUI.color;
            GUI.color = m_mouseOverStyle.normal.textColor;
            EditorGUILayout.LabelField(m_content, m_mouseOverStyle, options);
            GUI.color = c;
        }

        public Vector2 CalcSize() {
            return m_style.CalcSize(m_content);
        }

		public void SetContent(GUIContent content) {
			m_content = content;
		}

		public void SetStyle(GUIStyle style) {
			m_style = new GUIStyle(style);
		}

        public void GuiColorOverrdie(bool value) {
            m_overrdieGuiColor = value;
        }
	
        public GUIContent Content {
            get {
                return m_content;
            }
        }

        public Color Color {
            get {
                return m_style.normal.textColor;
            }
        }
    }
}