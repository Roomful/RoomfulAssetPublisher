using UnityEditor.IMGUI.Controls;

namespace RF.AssetWizzard.Editor
{
    public sealed class VariantHierarchyViewItem : TreeViewItem
    {
        public VariantHierarchyViewItem(int id, int depth)
            : base(id, depth, "item")
        {

        }
    }
}
