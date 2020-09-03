using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace RF.AssetWizzard.Editor
{
    public sealed class VariantHierarchyViewItem : TreeViewWithIconItem
    {
        readonly Skin m_Skin;
        readonly Renderer m_Renderer;
        readonly int m_MaterialIndex;

        public VariantHierarchyViewItem(int id, int depth, string name, Skin skin, Renderer renderer, int index)
            : base(id, depth, name)
        {
            m_Skin = skin;
            m_MaterialIndex = index;
            m_Renderer = renderer;
        }

        public void OnGUI(Rect rect)
        {
            rect = DrawIcon(rect, new Vector2(0,0));
            rect.position += new Vector2(10, 0);
            m_Skin.MaterialDictionary[m_Renderer][m_MaterialIndex] = (Material)EditorGUI.ObjectField(rect, displayName, m_Skin.MaterialDictionary[m_Renderer][m_MaterialIndex],
                typeof(Material), false);

        }
    }
}
