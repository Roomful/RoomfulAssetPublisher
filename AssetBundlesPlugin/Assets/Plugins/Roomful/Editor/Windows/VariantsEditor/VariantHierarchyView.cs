using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace RF.AssetWizzard.Editor
{
    public sealed class VariantHierarchyView : TreeView
    {
        readonly PropVariant m_Variant;
        readonly Skin m_Skin;

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
                Renderer renderer = m_Variant.Renderers[i];
                TreeViewItem item = new VariantHierarchyViewItem(i, 0, renderer.name, m_Skin);
                root.AddChild(item);
            }

            SetupDepthsFromParentsAndChildren(root);

            return root;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            VariantHierarchyViewItem item = args.item as VariantHierarchyViewItem;
            if (item != null) item.OnGUI(args.rowRect);
        }

        protected override bool CanMultiSelect(TreeViewItem item)
        {
            return false;
        }
    }
}
