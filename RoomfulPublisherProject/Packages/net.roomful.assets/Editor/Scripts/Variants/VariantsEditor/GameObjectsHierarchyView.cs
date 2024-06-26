using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace net.roomful.assets.editor
{
    internal sealed class GameObjectsHierarchyView : TreeView
    {
        private readonly List<GameObject> m_selectedGameObjects;
        private int m_id;
        private int m_depth;

        public GameObjectsHierarchyView(TreeViewState state, List<GameObject> selectedGameObjects) : base(state)
        {
            m_selectedGameObjects = selectedGameObjects;
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            var unionList = new List<Transform>();
            foreach (var selectedGameObject in m_selectedGameObjects)
            {
                unionList.AddRange(GetHierarchyForObject(selectedGameObject.transform).ToList());
            }
            unionList = unionList.Select(x => x).Distinct().ToList();//remove duplicates transforms

            var root = new TreeViewItem(-1, -1, "Root");
            m_depth = 0;
            GameObjectsHierarchyViewItem current = null;
            for (var i = 0; i < unionList.Count; i++)
            {
                m_depth++;
                var child = new GameObjectsHierarchyViewItem(GetID(), m_depth, unionList[i]);
                if (i == 0)
                {
                    root.AddChild(child);
                }
                else
                {
                    if (unionList[i].parent.Equals(current.ItemTransform))
                    {
                        current.AddChild(child);
                    }
                    else
                    {
                        var item = GetTreeViewByName(root, unionList[i].parent.name);
                        item?.AddChild(child); 
                    }

                }
                current = child;
            }
           
            SetupDepthsFromParentsAndChildren(root);
            return root;
        }

        private TreeViewItem m_parentTreeView;
        private TreeViewItem GetTreeViewByName(TreeViewItem item, string name)
        {
            var parent = item.children.Find(j => j.displayName.Equals(name));
            if (parent == null)
            {
                if (!item.hasChildren)
                {
                    return m_parentTreeView;
                }
                item.children.ForEach(c =>
                {
                    GetTreeViewByName(c, name);
                });
            }

            return m_parentTreeView ?? (m_parentTreeView = parent);
        }

        private static IEnumerable<Transform> GetHierarchyForObject(Transform t)
        {
            var hierarchy = new List<Transform>();
            while (t != null)
            {
                hierarchy.Add(t);
                t = t.parent;
            }

            hierarchy.Reverse();
            return hierarchy;
        }

        protected override void RowGUI(RowGUIArgs args) 
        {
            if (m_selectedGameObjects.Count == 0)
            {
                return;
            }

            var oldGuiColor = GUI.color;
            var item = args.item as TreeViewWithIconItem;
            if (item is GameObjectsHierarchyViewItem gameObjectsHierarchyViewItem)
            {
                if (m_selectedGameObjects.Contains(gameObjectsHierarchyViewItem.ItemTransform.gameObject))
                {
                    GUI.color = Color.green;
                }

                var indent = GetContentIndent(item);
                var itemRect = new Rect(args.rowRect.x + indent, args.rowRect.y, args.rowRect.width - indent, args.rowRect.height);
                gameObjectsHierarchyViewItem.OnGUI(itemRect); 
                GUI.color = oldGuiColor;
            }
            else
            {
                args.rowRect = item.DrawIcon(args.rowRect, new Vector2(15, 0));
            }

            if (args.item.depth > 0)
            {
                args.label = "";
            }
        }

        protected override void SelectionChanged(IList<int> selectedIds)
        {
            var list = selectedIds.Select(selectedId => FindItem(selectedId, rootItem) as GameObjectsHierarchyViewItem).ToList();
            //Selection.objects = list.Select(i => i.ItemTransform).ToArray();
            Selection.activeGameObject = list[0].ItemTransform.gameObject;
            EditorGUIUtility.PingObject(Selection.activeGameObject);
            SceneView.lastActiveSceneView.FrameSelected();
            base.SelectionChanged(selectedIds);
        }

        protected override bool CanMultiSelect(TreeViewItem item) => false;

        private int GetID() => m_id++;
    }
}
