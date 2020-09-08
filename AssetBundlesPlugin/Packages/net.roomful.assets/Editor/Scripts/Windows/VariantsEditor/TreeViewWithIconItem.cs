using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace net.roomful.assets.Editor
{
    public class TreeViewWithIconItem : TreeViewItem
    {
        readonly Texture m_Icon = EditorGUIUtility.IconContent("GameObject Icon").image;
        Rect m_Rect;
        readonly Vector2 m_IconSize = new Vector2(15, 15);
        public TreeViewWithIconItem(int id, int depth, string name)
            : base(id, depth, name)
        {

        }

        public Rect DrawIcon(Rect rect, Vector2 offset)
        {
            m_Rect = rect;
            m_Rect.size = m_IconSize;
            m_Rect.position += offset;
            GUI.DrawTexture(m_Rect, m_Icon, ScaleMode.ScaleToFit);
            rect.position += new Vector2(5, 0);
            return rect;
        }
    }
}

