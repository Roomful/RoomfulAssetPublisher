using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace net.roomful.assets.Editor
{
    internal sealed class VariantHierarchyView : TreeView
    {
        private readonly PropVariant m_variant;
        private readonly Skin m_skin;
        private int m_id;

        public VariantHierarchyView(TreeViewState state, PropVariant variant, Skin skin)
            : base(state)
        {
            m_variant = variant;
            m_skin = skin;

            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem(-1, -1, "Root");

            foreach (var renderer in m_variant.Renderers) {
                var item = new TreeViewWithIconItem(GetID(), 0, "   " + renderer.name);

                root.AddChild(item);

                for (var j = 0; j < renderer.sharedMaterials.Length; j++)
                {
                    TreeViewItem materialItem = new VariantHierarchyViewItem(GetID(), 1, "Element " + j, m_skin, renderer, j);
                    item.AddChild(materialItem);
                }
            }

            SetupDepthsFromParentsAndChildren(root);

            return root;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var item = args.item as TreeViewWithIconItem;

            if (item is VariantHierarchyViewItem variantHierarchyViewItem)
            {
                var indent = GetContentIndent(item);
                var itemRect = new Rect(args.rowRect.x + indent, args.rowRect.y, args.rowRect.width - indent, args.rowRect.height);
                variantHierarchyViewItem.OnGUI(itemRect);
            }
            else
            {
                args.rowRect = item.DrawIcon(args.rowRect, new Vector2(15, 0));
            }

            if (args.item.depth > 0)
            {
                args.label = "";
            } 
                
            base.RowGUI(args);
        }

        

        protected override bool CanMultiSelect(TreeViewItem item)
        {
            return false;
        }

        private int GetID()
        {
            return m_id++;
        }
    }
}
