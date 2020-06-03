using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace RF.AssetWizzard.Editor
{
    public sealed class VariantHierarchyView : TreeView
    {
        PropVariant m_Variant;
        Skin m_Skin;

        public VariantHierarchyView(TreeViewState state, PropVariant variant, Skin skin)
            : base(state)
        {
            m_Variant = variant;
            m_Skin = skin;

            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            TreeViewItem root = new TreeViewItem(-1, -1, "Root");

            for (int i = 0; i < m_Variant.Renderers.Count; i ++)
            {
                TreeViewItem item = new VariantHierarchyViewItem(i, 0);
                root.AddChild(item);
            }

            SetupDepthsFromParentsAndChildren(root);

            return root;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            EditorGUI.ObjectField(args.rowRect, "material", null, typeof(Material), false);
        }
    }
}
