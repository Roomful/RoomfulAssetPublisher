using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace RF.AssetWizzard.Editor
{
    public sealed class VariantHierarchyViewItem : TreeViewItem
    {
        readonly Skin m_Skin;

        public VariantHierarchyViewItem(int id, int depth, string name, Skin skin)
            : base(id, depth, name)
        {
            m_Skin = skin;
        }

        public void OnGUI(Rect rect)
        {
            m_Skin.Materials[id] = (Material)EditorGUI.ObjectField(rect, displayName, m_Skin.Materials[id],
                typeof(Material), false);
        }
    }
}
