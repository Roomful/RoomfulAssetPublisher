using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RF.AssetWizzard {


	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	public class RoomfulText : MonoBehaviour {


		public enum AlignmentVertical {
			Upper,
			Middle,
			Lower
		}


		public enum AlignmentHorizontal {
			Left,
			Center,
			Right
		}



		[TextArea(3, 10)]
		public string Text = "Hello Roomful";
		public FontData FontData = FontData.defaultFontData;
		public bool DrawGizmos = true;
		public Color Color = Color.white;


		private Bounds m_textBounds = new Bounds (Vector3.zero, Vector3.zero);



		void Update () {
			Refersh ();

		}

		protected virtual void OnDrawGizmos () {
			if(!DrawGizmos) {
				return;
			}

			GizmosDrawer.DrawCube (transform.position, transform.rotation, RectTransform.rect.size, Color.white);
		}



		public float Width {
			get {
				return RectTransform.sizeDelta.x;
			}

			set {
				RectTransform.sizeDelta = new Vector2 (value, Height);
			}
		}

		public float Height {
			get {
				return RectTransform.sizeDelta.y;
			}

			set {
				RectTransform.sizeDelta = new Vector2 (Width, value);
			}
		}



		private void Refersh() {



			TextRenderer.text = Text;
			TextRenderer.fontSize = FontData.fontSize;
			TextRenderer.lineSpacing = FontData.lineSpacing;
			TextRenderer.fontStyle = FontData.fontStyle;
			TextRenderer.font = FontData.font;
			TextRenderer.color = Color;



			//not editable defaults:
			TextRenderer.characterSize = 1;
			TextRenderer.tabSize = 4;
			TextRenderer.offsetZ = 0;
			TextRenderer.richText = true;
			TextRenderer.transform.localScale = Vector3.one * 0.1f;
			TextRenderer.anchor = TextAnchor.MiddleCenter;



			UpdateTextRendererBounds ();


			if(FontData.horizontalOverflow == FontData.WrapMode.Truncate) {
				while(m_textBounds.size.x > Width) {
					TextRenderer.fontSize--;
					UpdateTextRendererBounds ();
				}
			}

			if(FontData.verticalOverflow == FontData.WrapMode.Truncate) {
				while(m_textBounds.size.y > Height) {
					TextRenderer.fontSize--;
					UpdateTextRendererBounds ();
				}
			}


			switch(FontData.alignment) {
			case TextAnchor.LowerCenter:
				ApplayAlligment (AlignmentVertical.Lower, AlignmentHorizontal.Center);
				break;
			case TextAnchor.LowerLeft:
				ApplayAlligment (AlignmentVertical.Lower, AlignmentHorizontal.Left);
				break;
			case TextAnchor.LowerRight:
				ApplayAlligment (AlignmentVertical.Lower, AlignmentHorizontal.Right);
				break;

			case TextAnchor.MiddleCenter:
				ApplayAlligment (AlignmentVertical.Middle, AlignmentHorizontal.Center);
				break;
			case TextAnchor.MiddleLeft:
				ApplayAlligment (AlignmentVertical.Middle, AlignmentHorizontal.Left);
				break;
			case TextAnchor.MiddleRight:
				ApplayAlligment (AlignmentVertical.Middle, AlignmentHorizontal.Right);
				break;

			case TextAnchor.UpperCenter:
				ApplayAlligment (AlignmentVertical.Upper, AlignmentHorizontal.Center);
				break;
			case TextAnchor.UpperLeft:
				ApplayAlligment (AlignmentVertical.Upper, AlignmentHorizontal.Left);
				break;
			case TextAnchor.UpperRight:
				ApplayAlligment (AlignmentVertical.Upper, AlignmentHorizontal.Right);
				break;
			}


			UpdateTextRendererBounds ();

		}


		private void ApplayAlligment(AlignmentVertical vertical, AlignmentHorizontal horizontal) {

			float x = 0f;
			float y = 0f;

			switch(horizontal) {
			case AlignmentHorizontal.Left:
				TextRenderer.alignment = TextAlignment.Left;
				x = -Width / 2f + m_textBounds.size.x / 2f;
				break;
			case AlignmentHorizontal.Center:
				TextRenderer.alignment = TextAlignment.Center;
				x = 0;
				break;
			case AlignmentHorizontal.Right:
				TextRenderer.alignment = TextAlignment.Right;
				x = Width / 2f -  m_textBounds.size.x / 2f;
				break;
			}


			switch(vertical) {
			case AlignmentVertical.Upper:

				y = Height / 2f - m_textBounds.size.y / 2f;
				break;
			case AlignmentVertical.Middle:
				y = 0;
				break;
			case AlignmentVertical.Lower:
				y = -Height / 2f +  m_textBounds.size.y / 2f;
				break;
			}

			TextRenderer.transform.localPosition = new Vector3 (x, y, 0);
		}


		private void UpdateTextRendererBounds () {

			Quaternion oldRotation = transform.rotation;
			transform.rotation = Quaternion.identity;

			m_textBounds = TextRenderer.GetComponent<Renderer> ().bounds;


			transform.rotation = oldRotation;
		}





		private RectTransform RectTransform {
			get {
				var t = GetComponent<RectTransform> ();
				t.hideFlags = HideFlags.NotEditable;
				return t;
			}
		}

		private TextMesh TextRenderer {
			get {

				if(transform.childCount == 0) {
					CreateTextRenderer ();
				}

				var text = transform.GetChild (0).GetComponent<TextMesh> ();
				if(text == null) {
					CreateTextRenderer ();
					text = transform.GetChild (0).GetComponent<TextMesh> ();
				}

				text.gameObject.hideFlags = HideFlags.HideInHierarchy;

				return text;
			}
		}

		private void CreateTextRenderer() {
			var obj = new GameObject ("Text");
			obj.AddComponent<TextMesh> ();
			obj.transform.parent = transform;
			obj.transform.localPosition = Vector3.zero;
			obj.transform.SetAsFirstSibling ();

		}



	}

}
