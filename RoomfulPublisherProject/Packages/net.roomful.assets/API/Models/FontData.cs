using System;
using UnityEngine;
using net.roomful.assets.serialization;

namespace net.roomful.assets
{
    [Serializable]
    public class FontData : ISerializationCallbackReceiver
    {
        [SerializeField] private Font m_Font;
        [SerializeField] private int m_FontSize;
        [SerializeField] private FontStyle m_FontStyle;
        [SerializeField] private TextAnchor m_Alignment;
        [SerializeField] private SerializedTextWrapMode m_HorizontalOverflow;
        [SerializeField] private SerializedTextWrapMode m_VerticalOverflow;
        [SerializeField] private float m_LineSpacing;
        [SerializeField] private int m_LineCharacterLimit;

        public static FontData DefaultFontData => new FontData {
            m_FontSize = 50,
            m_LineSpacing = 1f,
            m_LineCharacterLimit = 30,
            m_FontStyle = FontStyle.Normal,
            m_Alignment = TextAnchor.UpperLeft,
            m_HorizontalOverflow = SerializedTextWrapMode.Truncate,
            m_VerticalOverflow = SerializedTextWrapMode.Truncate,
        };

        public Font Font {
            get => m_Font;
            set => m_Font = value;
        }

        public int FontSize {
            get => m_FontSize;
            set => m_FontSize = value;
        }

        public FontStyle FontStyle {
            get => m_FontStyle;
            set => m_FontStyle = value;
        }

        public TextAnchor Alignment {
            get => m_Alignment;
            set => m_Alignment = value;
        }

        public SerializedTextWrapMode HorizontalOverflow {
            get => m_HorizontalOverflow;
            set => m_HorizontalOverflow = value;
        }

        public SerializedTextWrapMode VerticalOverflow {
            get => m_VerticalOverflow;
            set => m_VerticalOverflow = value;
        }

        public float LineSpacing {
            get => m_LineSpacing;
            set => m_LineSpacing = value;
        }

        public int LineCharacterLimit {
            get => m_LineCharacterLimit;
            set => m_LineCharacterLimit = value;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }

        void ISerializationCallbackReceiver.OnAfterDeserialize() {
            m_FontSize = Mathf.Clamp(m_FontSize, 0, 300);
        }
    }
}