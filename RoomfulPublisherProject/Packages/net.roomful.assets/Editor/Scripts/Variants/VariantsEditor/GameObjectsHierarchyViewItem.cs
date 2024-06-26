using UnityEngine;

namespace net.roomful.assets.editor
{
    internal sealed class GameObjectsHierarchyViewItem : TreeViewWithIconItem
    {

        private readonly string m_name;
        public Transform ItemTransform { get; }

        public GameObjectsHierarchyViewItem(int id, int depth, Transform t) : base(id, depth, t.name)
        {
            m_name = t.name;
            ItemTransform = t;
        }
        
        public void OnGUI(Rect rect)
        {
            rect = DrawIcon(rect, new Vector2(0,0));
            rect.position += new Vector2(10, 0);
            GUI.Label(rect, m_name);
        }
    }
}