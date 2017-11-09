using System;
using UnityEngine;
using RF.AssetBundles.Serialization;

namespace RF.AssetWizzard
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

        public static FontData defaultFontData
		{
			get
			{
                return new FontData {
                    m_FontSize = 50,
                    m_LineSpacing = 1f,
                    m_LineCharacterLimit = 30,
                    m_FontStyle = FontStyle.Normal,
					m_Alignment = TextAnchor.UpperLeft,
					m_HorizontalOverflow = SerializedTextWrapMode.Truncate,
					m_VerticalOverflow = SerializedTextWrapMode.Truncate,
				};
			}
		}

		public Font font
		{
			get
			{
				return this.m_Font;
			}
			set
			{
				this.m_Font = value;
			}
		}

		public int fontSize
		{
			get
			{
				return this.m_FontSize;
			}
			set
			{
				this.m_FontSize = value;
			}
		}

		public FontStyle fontStyle
		{
			get
			{
				return this.m_FontStyle;
			}
			set
			{
				this.m_FontStyle = value;
			}
		}

	

		public TextAnchor alignment
		{
			get
			{
				return this.m_Alignment;
			}
			set
			{
				this.m_Alignment = value;
			}
		}
			

		public SerializedTextWrapMode horizontalOverflow
		{
			get
			{
				return this.m_HorizontalOverflow;
			}
			set
			{
				this.m_HorizontalOverflow = value;
			}
		}

		public SerializedTextWrapMode verticalOverflow
		{
			get
			{
				return this.m_VerticalOverflow;
			}
			set
			{
				this.m_VerticalOverflow = value;
			}
		}

		public float lineSpacing
		{
			get
			{
				return this.m_LineSpacing;
			}
			set
			{
				this.m_LineSpacing = value;
			}
		}


        public int LineCharacterLimit {
            get {
                return this.m_LineCharacterLimit;
            }
            set {
                this.m_LineCharacterLimit = value;
            }
        }


        void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			m_FontSize = Mathf.Clamp(this.m_FontSize, 0, 300);
		}

	}
}