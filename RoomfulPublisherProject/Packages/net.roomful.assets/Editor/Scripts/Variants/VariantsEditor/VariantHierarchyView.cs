using System;
using System.Collections.Generic;
using System.Linq;
using net.roomful.assets.serialization;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Object = UnityEngine.Object;

namespace net.roomful.assets.editor
{
    internal sealed class VariantHierarchyView : TreeView
    {
        private readonly PropVariant m_variant;
        private readonly PropSkin m_skin;
        private List<GameObject> m_customizableSkinGameObjects;
        private int m_id;
        private List<Object> m_currentGameObjects;
        private readonly List<VariantHierarchyViewItem> m_items = new List<VariantHierarchyViewItem>();
        private readonly List<VariantHierarchyViewItem> m_treeViewList = new List<VariantHierarchyViewItem>();

        private int m_selectedId;
        public Action OnCustomizableSkinGameObjectChange = () => {};

        public VariantHierarchyView(TreeViewState state, PropVariant variant, PropSkin skin, List<GameObject> customizableSkinGameObjects) : base(state)
        {
            m_variant = variant;
            m_skin = skin;
            m_customizableSkinGameObjects = customizableSkinGameObjects;
            m_items.Clear();
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            var itemsNew = new List<VariantHierarchyViewItem>();
            var root = new TreeViewItem(-1, -1, "Root");
            ChildrenVariant(root, 0, m_variant.GameObjects, itemsNew);
            m_items.Clear();
            m_items.AddRange(itemsNew.Distinct().ToList());
            SetupDepthsFromParentsAndChildren(root);

            return root;
        }

        private void ChildrenVariant(TreeViewItem root, int depth, IEnumerable<GameObject> children, List<VariantHierarchyViewItem> itemsNew) {
            foreach (var gameObject in children) {
                if (itemsNew.Any(i => i.ItemTransform.gameObject.Equals(gameObject))) {
                    continue;
                }
                
                var customizedSkin = gameObject.GetComponent<SerializedCustomizableSkinGameObject>();
                var colorizedSkin = gameObject.GetComponent<SerializedColorizedSkinObject>();
                string type;
                if (customizedSkin != null)
                    type = customizedSkin.Type.ToString();
                else if (colorizedSkin != null)
                    type = "Colorized Skin";
                else
                    type = "None";
                
                var item = new VariantHierarchyViewItem(GetID(), depth, gameObject, type);
                var foundItems = m_items.Where(i => i.ItemTransform.Equals(item.ItemTransform));
                if (foundItems.Any()) {
                    var expanded = GetExpanded().Any(id => id == foundItems.First().id);
                    SetExpanded(item.id, expanded);
                }
                var transform = gameObject.transform;

                if (transform.childCount > 0) {
                    var grandchildren = new List<GameObject>();
                    for (var i = 0; i < transform.childCount; i++) {
                        grandchildren.Add(transform.GetChild(i).gameObject);
                    }
                    ChildrenVariant(item, depth++, grandchildren, itemsNew);
                }
                root.AddChild(item);
                itemsNew.Add(item);
            }
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var item = args.item as TreeViewWithIconItem;
            if (item is VariantHierarchyViewItem variantHierarchyViewItem)
            {
                var indent = GetContentIndent(item);
                var itemRect = new Rect(args.rowRect.x + indent, args.rowRect.y, args.rowRect.width - indent, args.rowRect.height);
                args.label = "";
                if (variantHierarchyViewItem.ItemTransform != null) {
                    var color = CheckColor(variantHierarchyViewItem);
                    var colorized = ViewItemIsColorized(variantHierarchyViewItem);
                    variantHierarchyViewItem.OnGUI(itemRect, color, colorized);
                }
                //  m_variant.ApplySkin(m_skin);
                if (Selection.objects.Length > 0)
                {
                    var selectedSceneObjects = Selection.objects.ToList();
                    if (m_currentGameObjects == null || !(m_currentGameObjects.Count == selectedSceneObjects.Count && !m_currentGameObjects.Except(selectedSceneObjects).Any()))
                    {
                        m_currentGameObjects = selectedSceneObjects;
                        foreach (var selectedItem in m_currentGameObjects)
                        {
                            foreach (var treeViewItem in m_items)
                            {
                                if (selectedItem.name.Equals(treeViewItem.ItemTransform.name))
                                {
                                    m_treeViewList.Add(treeViewItem);
                                    break;
                                }
                            }
                        }
                        m_treeViewList.ForEach(treeItem => {
                            SetExpanded(treeItem.id, true);
                            SetSelection(new List<int>{treeItem.id});
                        });
                        m_treeViewList.Clear();
                    }
                }
            }
            else
            {
                args.rowRect = item.DrawIcon(args.rowRect, new Vector2(15, 0));
            }

            base.RowGUI(args);
        }

        protected override void SelectionChanged(IList<int> selectedIds) {
            var list = selectedIds.Select(selectedId => FindItem(selectedId, rootItem) as VariantHierarchyViewItem).ToList();
            Selection.activeGameObject = list[0].ItemTransform.gameObject;
            EditorGUIUtility.PingObject(Selection.activeGameObject);
            SceneView.lastActiveSceneView.FrameSelected();
            base.SelectionChanged(selectedIds);
        }
        protected override bool CanMultiSelect(TreeViewItem item) => true;

        private int GetID() => m_id++;

        protected override void ContextClickedItem(int id) {

            base.ContextClickedItem(id);
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent($"Mounted Point"), false, OnCustomizableSkinGameObjectTypeSelected);
            menu.AddItem(new GUIContent($"Replace Material"), false, OnCustomizableSkinGameObjectReplaceMaterialSelected);
            menu.AddItem(new GUIContent($"Colorized Skin"), false, OnColorizedSkinObjectChange);
            menu.AddItem(new GUIContent($"None"), false, OnCustomizableSkinGameObjectTypeNone);
            
            menu.ShowAsContext();
        }

        private List<GameObject> GetSelectedObjects()
        {
            List<GameObject> items = new List<GameObject>();
            foreach (var obj in GetSelection())
                items.Add((FindItem(obj, rootItem) as VariantHierarchyViewItem).ItemTransform.gameObject);

            return items;
        }
        
        private void OnCustomizableSkinGameObjectReplaceMaterialSelected()
        {
            foreach (var go in GetSelectedObjects()) {
                var customizedSkin = go.GetComponent<SerializedCustomizableSkinGameObject>();

                if (customizedSkin == null)
                    customizedSkin = go.AddComponent(typeof(SerializedCustomizableSkinGameObject)) as SerializedCustomizableSkinGameObject;
                customizedSkin.Type = CustomizableSkinGameObjectType.ReplaceMaterial;
                OnCustomizableSkinGameObjectChange.Invoke();
            }
        }

        private void OnCustomizableSkinGameObjectTypeSelected()
        {
            foreach (var go in GetSelectedObjects()) {
                var customizedSkin = go.GetComponent<SerializedCustomizableSkinGameObject>();

                if (customizedSkin == null)
                    customizedSkin = go.AddComponent(typeof(SerializedCustomizableSkinGameObject)) as SerializedCustomizableSkinGameObject;
                customizedSkin.Type = CustomizableSkinGameObjectType.MountedPoint;
                OnCustomizableSkinGameObjectChange.Invoke();
            }
        }

        private void OnColorizedSkinObjectChange()
        {
            foreach (var go in GetSelectedObjects()) {
                var colorizedSkin = go.GetComponent<SerializedColorizedSkinObject>();

                if (colorizedSkin != null)
                    GameObject.DestroyImmediate(colorizedSkin);
                else
                    go.AddComponent(typeof(SerializedColorizedSkinObject));
            }
        }

        private void OnCustomizableSkinGameObjectTypeNone()
        {
            foreach (var go in GetSelectedObjects()) {
                var customizedSkin = go.GetComponent<SerializedCustomizableSkinGameObject>();
                var colorizedSkin = go.GetComponent<SerializedColorizedSkinObject>();

                if (customizedSkin != null)
                    GameObject.DestroyImmediate(customizedSkin);
                if (colorizedSkin != null)
                    GameObject.DestroyImmediate(colorizedSkin);
                OnCustomizableSkinGameObjectChange.Invoke();
            }
        }

        private Color CheckColor(VariantHierarchyViewItem variantHierarchyViewItem) {
            var oldColor = GUI.color;
            oldColor.a = 0.5f;
            var gameobject = variantHierarchyViewItem.ItemTransform.gameObject;
            var customizedSkin = gameobject.GetComponent<SerializedCustomizableSkinGameObject>();
            if (customizedSkin != null && customizedSkin.Type == CustomizableSkinGameObjectType.MountedPoint)
            {
                return oldColor;
            }
            if (m_customizableSkinGameObjects.Contains(variantHierarchyViewItem.ItemTransform.gameObject))
            {
                return Color.green;
            }

            return oldColor;
        }

        private bool ViewItemIsColorized(VariantHierarchyViewItem variantHierarchyViewItem) {
            var oldColor = GUI.color;
            oldColor.a = 0.5f;
            var gameobject = variantHierarchyViewItem.ItemTransform.gameObject;
            var colorizedSkin = gameobject.GetComponent<SerializedColorizedSkinObject>();
            return (colorizedSkin != null);
        }

        internal void ChangedCustomizableSkinGameObjects(List<GameObject> customizableSkinGameObjects) {
            m_customizableSkinGameObjects = customizableSkinGameObjects;
            Reload();
        }
    }
}
