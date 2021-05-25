using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace net.roomful.api
{
    /// <summary>
    /// This is result of Copy/Paste menu pick from native platform.
    /// For android and iOS platforms.
    /// </summary>
    [Serializable]
    public class CopyPasteMenuResult
    {
        [SerializeField] public string m_MenuActionType;
        [SerializeField] public string m_PasteString;

        public CopyPasteMenuItem Item => EnumUtility.ParseEnum<CopyPasteMenuItem>(m_MenuActionType);

        /// <summary>
        /// This string that we get from native platforms buffer.
        /// </summary>
        public string PasteString => m_PasteString;
    }
}

