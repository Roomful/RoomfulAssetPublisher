using UnityEditor;
using UnityEngine;

namespace net.roomful.assets.Editor
{
    internal sealed class VariantHierarchyViewItem : TreeViewWithIconItem
    {
        private readonly Skin m_skin;
        private readonly Renderer m_renderer;
        private readonly int m_materialIndex;

        public VariantHierarchyViewItem(int id, int depth, string name, Skin skin, Renderer renderer, int index)
            : base(id, depth, name)
        {
            m_skin = skin;
            m_materialIndex = index;
            m_renderer = renderer;
        }

        public void OnGUI(Rect rect)
        {
            rect = DrawIcon(rect, new Vector2(0,0));
            rect.position += new Vector2(10, 0);
            
            m_skin.MaterialDictionary[m_renderer][m_materialIndex] 
                = (Material)EditorGUI.ObjectField(rect, m_skin.MaterialDictionary[m_renderer][m_materialIndex], typeof(Material), false);

        }
    }
}
