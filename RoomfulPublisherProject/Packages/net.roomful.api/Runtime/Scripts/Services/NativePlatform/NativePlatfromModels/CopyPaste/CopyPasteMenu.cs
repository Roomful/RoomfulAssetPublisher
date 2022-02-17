using System;
using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.api
{
    /// <summary>
    /// This class is for showing copy paste menu on android and iOS.
    /// </summary>
    [Serializable]
    public class CopyPasteMenu
    {
        [SerializeField] private List<string> m_MenuItems;
        [SerializeField] private string m_Text;
        [SerializeField] private float m_xPos;
        [SerializeField] private float m_yPos;

        /// <summary>
        /// Creates new copy paste menu at defined position.
        /// </summary>
        /// <param name="text">The copy text.</param>
        /// <param name="xPos">x coord menu position.</param>
        /// <param name="yPos">y coord menu position.</param>
        public CopyPasteMenu(string text, float xPos, float yPos) {
            m_Text = text;
            m_xPos = xPos;
            m_yPos = yPos;
            m_MenuItems = new List<string>() {
                CopyPasteMenuItem.Copy.ToString(),
                CopyPasteMenuItem.Paste.ToString(),
                CopyPasteMenuItem.Cut.ToString(),
            };
        }

        public void Show(Action<CopyPasteMenuResult> callback) {
            var data = JsonUtility.ToJson(this);
            Roomful.Native.ShowCopyPasteMenu(data, callback);
        }
    }
}
