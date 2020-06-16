using UnityEditor.IMGUI.Controls;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace RF.AssetWizzard.Editor
{
    public sealed class VariantHierarchyView : TreeView
    {
        readonly PropVariant m_Variant;
        readonly Skin m_Skin;
        int m_Id;

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

            for (int i = 0; i < m_Variant.Renderers.Count; i++)
            {
                Renderer renderer = m_Variant.Renderers[i];
                TreeViewWithIconItem item = new TreeViewWithIconItem(GetID(), 0, "   " + renderer.name);

                root.AddChild(item);

                for (int j = 0; j < m_Variant.Renderers[i].materials.Length; j++)
                {
                    TreeViewItem materialItem = new VariantHierarchyViewItem(GetID(), 1, "Element " + j, m_Skin, renderer, j);
                    item.AddChild(materialItem);
                }
            }

            SetupDepthsFromParentsAndChildren(root);

            return root;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            TreeViewWithIconItem item = args.item as TreeViewWithIconItem;

            if (item.GetType() == typeof(VariantHierarchyViewItem))
            {
                float indent = GetContentIndent(item);
                Rect itemRect = new Rect(args.rowRect.x + indent, args.rowRect.y, args.rowRect.width - indent, args.rowRect.height);
                ((VariantHierarchyViewItem)item).OnGUI(itemRect);
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
            return m_Id++;
        }
    }
}
