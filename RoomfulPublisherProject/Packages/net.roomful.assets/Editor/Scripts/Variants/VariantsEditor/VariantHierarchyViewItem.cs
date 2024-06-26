using UnityEditor;
using UnityEngine;

namespace net.roomful.assets.editor
{
    internal sealed class VariantHierarchyViewItem : TreeViewWithIconItem
    {
        private readonly PropSkin m_skin;
        private readonly Renderer m_renderer;
        private readonly int m_materialIndex;
        public Transform ItemTransform { get; }
        private string skinReplaceType;



        public VariantHierarchyViewItem(int id, int depth, GameObject gameObject, string replaceType) : base(id, depth, gameObject.name)
        {
            ItemTransform = gameObject.transform;
            skinReplaceType = replaceType;
        }

        public VariantHierarchyViewItem(int id, int depth, string name, PropSkin skin, Renderer renderer, int index, Transform t) : base(id, depth, name)
        {
            m_skin = skin;
            m_materialIndex = index;
            m_renderer = renderer;
            ItemTransform = t;
        }

        public void OnGUI(Rect rect, Color color, bool colorized)
        {
            var oldColor = GUI.color;
            var grayColor = GUI.color;
            grayColor.a = 0.5f;
            GUI.color = color;
            rect = DrawIcon(rect, new Vector2(0,0));
            if (color.Equals(grayColor)) {
                rect.position += new Vector2(10, 0);
                GUI.Label(rect, ItemTransform.gameObject.name);
                GUI.color = oldColor;
            }
            else {
                GUI.color = oldColor;
                rect.position += new Vector2(10, 0);
                GUI.Label(rect, ItemTransform.gameObject.name);
            }

            var style = new GUIStyle();
            style.alignment = TextAnchor.MiddleRight;
            style.padding.right = 40;
            GUI.Label(rect, new GUIContent("Skin Replacement Type: " + skinReplaceType, "Change type by pressing Right Mouse Button"), style);

            if (colorized) {
                DrawIcon(rect, new Vector2((ItemTransform.gameObject.name.Length + 3)*5, 0), EditorGUIUtility.IconContent("PreTextureRGB").image);
            }
                    

            if (m_skin == null)
            {
                return;
            }
            /*
            m_skin.MaterialDictionary[m_renderer][m_materialIndex]
                = (Material)EditorGUI.ObjectField(rect, m_skin.MaterialDictionary[m_renderer][m_materialIndex], typeof(Material), false);
                */
        }
        

    }
}
