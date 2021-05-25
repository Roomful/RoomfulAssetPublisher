using UnityEngine;
using UnityEditor;

namespace SA.Common.Editor
{
    public interface SA_iGUIElement
    {
        void OnGui(Rect rect, SA_InputEvent e);
    }
}
