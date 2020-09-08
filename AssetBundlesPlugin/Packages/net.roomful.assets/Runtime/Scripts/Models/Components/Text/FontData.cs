using System;
using UnityEngine;
using net.roomful.assets.serialization;

namespace net.roomful.assets
{
	[Serializable]
	public class FontData : ISerializationCallbackReceiver
	{


		


		[SerializeField]
		private Font m_Font;

		[SerializeField]
		private int m_FontSize;

		[SerializeField]
		private FontStyle m_FontStyle;


		[SerializeField]
		private TextAnchor m_Alignment;


		[SerializeField]
		private SerializedTextWrapMode m_HorizontalOverflow;

		[SerializeField]
		private SerializedTextWrapMode m_VerticalOverflow;

		[SerializeField]
		private float m_LineSpacing;

        [SerializeField]
        private int m_LineCharacterLimit;

        public static FontData defaultFontData => new FontData {
	        m_FontSize = 50,
	        m_LineSpacing = 1f,
	        m_LineCharacterLimit = 30,
	        m_FontStyle = FontStyle.Normal,
	        m_Alignment = TextAnchor.UpperLeft,
	        m_HorizontalOverflow = SerializedTextWrapMode.Truncate,
	        m_VerticalOverflow = SerializedTextWrapMode.Truncate,
        };

        public Font font
		{
			get => m_Font;
			set => m_Font = value;
        }

		public int fontSize
		{
			get => m_FontSize;
			set => m_FontSize = value;
		}

		public FontStyle fontStyle
		{
			get => m_FontStyle;
			set => m_FontStyle = value;
		}

	

		public TextAnchor alignment
		{
			get => m_Alignment;
			set => m_Alignment = value;
		}
			

		public SerializedTextWrapMode horizontalOverflow
		{
			get => m_HorizontalOverflow;
			set => m_HorizontalOverflow = value;
		}

		public SerializedTextWrapMode verticalOverflow
		{
			get => m_VerticalOverflow;
			set => m_VerticalOverflow = value;
		}

		public float lineSpacing
		{
			get => m_LineSpacing;
			set => m_LineSpacing = value;
		}


        public int LineCharacterLimit {
            get => m_LineCharacterLimit;
            set => m_LineCharacterLimit = value;
        }


        void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			m_FontSize = Mathf.Clamp(m_FontSize, 0, 300);
		}

	}
}