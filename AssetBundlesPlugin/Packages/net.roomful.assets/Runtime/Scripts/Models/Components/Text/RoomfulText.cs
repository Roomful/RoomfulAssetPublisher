using UnityEngine;
using net.roomful.assets.serialization;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace net.roomful.assets {
	
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	internal class RoomfulText : MonoBehaviour, IPropComponent
    {
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
		public string PlaceHolderText = "Hello Roomful";
		public FontData FontData = FontData.DefaultFontData;
		public Color Color = Color.white;
		public TextContent Source = new TextContent();

   
		private Bounds m_textBounds = new Bounds (Vector3.zero, Vector3.zero);
		private static readonly float DEFAULT_TEXT_SCALE = 0.05f;

		public void Update () {
			Refersh ();
		}

		protected virtual void OnDrawGizmos () {
            

            if (Prop != null  && !Prop.DrawGizmos) {
                return;
            }
        

			var sbim = GetComponent<SerializedBoundsIgnoreMarker> ();
			if (sbim == null) {
				GizmosDrawer.DrawCube (transform.position, transform.rotation, new Vector2(Width, Height), Color.white);
			} else {
				GizmosDrawer.DrawCube (transform.position, transform.rotation, new Vector2(Width, Height), Color.red);
			}
        }


        public void Restore(SerializedText info) {
          
            PlaceHolderText = info.PlaceHolderText;
            Color = info.Color;
            FontData.Font = info.Font;
            FontData.FontSize = info.FontSize;
            FontData.LineSpacing = info.LineSpacing;
            FontData.LineCharacterLimit = info.LineCharacterLimit;
            FontData.FontStyle = info.FontStyle;
            FontData.Alignment = info.Alignment;
            FontData.HorizontalOverflow = info.HorizontalOverflow;
            FontData.VerticalOverflow = info.VerticalOverflow;


			Source.DataProvider = info.DataProvider;
			Source.ResourceIndex = info.ResourceIndex;
			Source.ResourceContentSource = info.ResourceContentSource;

            Refersh();

        }


        public void PrepareForUpload() {

            var textInfo = gameObject.AddComponent<SerializedText>();
            textInfo.PlaceHolderText = PlaceHolderText;
            textInfo.Color = Color;

			if(FontData.Font != null) {
				textInfo.Font = FontData.Font;
                textInfo.Font.material.shader = Shader.Find("GUI/Text Shader");
            }

           
            textInfo.FontSize = FontData.FontSize;
            textInfo.LineSpacing = FontData.LineSpacing;
            textInfo.LineCharacterLimit = FontData.LineCharacterLimit;
            textInfo.FontStyle = FontData.FontStyle;
            textInfo.Alignment = FontData.Alignment;
            textInfo.HorizontalOverflow = FontData.HorizontalOverflow;
            textInfo.VerticalOverflow = FontData.VerticalOverflow;

			textInfo.DataProvider = Source.DataProvider;
			textInfo.ResourceIndex = Source.ResourceIndex;
			textInfo.ResourceContentSource = Source.ResourceContentSource;


#if UNITY_EDITOR
			if (textInfo.Font != null) {
				var fontFilePath = AssetDatabase.GetAssetPath(textInfo.Font);

                if (System.IO.File.Exists(fontFilePath)) {

                    //remove Assets/ string from a path. Yes I know that is not stable hack.
                    //If you know a better way, make it happend
                    fontFilePath = fontFilePath.Substring(7, fontFilePath.Length - 7);
                    var data = SA.Common.Util.Files.ReadBytes(fontFilePath);

                    textInfo.FontFileContent = data;
                    textInfo.FullFontName = System.IO.Path.GetFileName(fontFilePath);
                }
			}
#endif
			DestroyImmediate(TextRenderer.gameObject);
            DestroyImmediate(this);
        }

        public void RemoveSilhouette() {
            //do nothing
        }



        public float Width => RectTransform.sizeDelta.x * transform.lossyScale.x;

        public float Height => RectTransform.sizeDelta.y * transform.lossyScale.y;

        public RectTransform RectTransform {
            get {
                var t = GetComponent<RectTransform>();
                //t.hideFlags = HideFlags.NotEditable;
                return t;
            }
        }

        private PropAsset Prop {
            get {
                var go = gameObject.transform;
                while (go != null) {
                    if (go.GetComponent<PropAsset>() != null) {
                        return go.GetComponent<PropAsset>();
                    }

                    go = go.parent;
                }

                return null;
            }
        }

        public PropComponentUpdatePriority UpdatePriority => PropComponentUpdatePriority.High;

        private string WrapText(string text) {
      
            var words = text.Split(' ');

            var result = new StringBuilder();

         
            var line = string.Empty;
            foreach(var word in words) {
                if(line.Equals(string.Empty)) {
                    line = word;
                    continue;
                }

                var charactedsCount = line.Length + word.Length;
                if(charactedsCount >= FontData.LineCharacterLimit) {
                    result.Append(line);
                    result.Append(System.Environment.NewLine);

                    line = word;
                } else {
                    line = line + " " + word;
                }
            }

            result.Append(line);


            return result.ToString();

        }

        private void Refersh() {

            
 
			TextRenderer.text = WrapText(PlaceHolderText);
			TextRenderer.fontSize = FontData.FontSize;
			TextRenderer.lineSpacing = FontData.LineSpacing;
			TextRenderer.fontStyle = FontData.FontStyle;
            TextRenderer.color = Color;
            TextRenderer.font = FontData.Font;
            TextRenderer.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);

            if (TextRenderer.font != null) {

                var m = new Material(TextRenderer.font.material);
                m.shader = Shader.Find("Roomful/Text");
                m.SetColor("_Color", Color);

                TextRenderer.GetComponent<MeshRenderer>().sharedMaterial = m;
			}
           
            TextRenderer.characterSize = 1;
			TextRenderer.tabSize = 4;
			TextRenderer.offsetZ = 0;
			TextRenderer.richText = true;
			TextRenderer.transform.localScale = Vector3.one * DEFAULT_TEXT_SCALE;
			TextRenderer.anchor = TextAnchor.MiddleCenter;

			UpdateTextRendererBounds ();



            if (FontData.HorizontalOverflow == SerializedTextWrapMode.Truncate) {
				if(m_textBounds.size.x > Width) {
                    var dif = Width / m_textBounds.size.x;
                    TextRenderer.transform.localScale = TextRenderer.transform.localScale * dif;
                    UpdateTextRendererBounds ();
				}
			}

			if(FontData.VerticalOverflow == SerializedTextWrapMode.Truncate) {
                if (m_textBounds.size.y > Height) {
                    var dif = Height / m_textBounds.size.y;
                    TextRenderer.transform.localScale = TextRenderer.transform.localScale * dif;
                    UpdateTextRendererBounds ();
				}
			}


			switch(FontData.Alignment) {
			case TextAnchor.LowerCenter:
				ApplyAligment (AlignmentVertical.Lower, AlignmentHorizontal.Center);
				break;
			case TextAnchor.LowerLeft:
				ApplyAligment (AlignmentVertical.Lower, AlignmentHorizontal.Left);
				break;
			case TextAnchor.LowerRight:
				ApplyAligment (AlignmentVertical.Lower, AlignmentHorizontal.Right);
				break;

			case TextAnchor.MiddleCenter:
				ApplyAligment (AlignmentVertical.Middle, AlignmentHorizontal.Center);
				break;
			case TextAnchor.MiddleLeft:
				ApplyAligment (AlignmentVertical.Middle, AlignmentHorizontal.Left);
				break;
			case TextAnchor.MiddleRight:
				ApplyAligment (AlignmentVertical.Middle, AlignmentHorizontal.Right);
				break;

			case TextAnchor.UpperCenter:
				ApplyAligment (AlignmentVertical.Upper, AlignmentHorizontal.Center);
				break;
			case TextAnchor.UpperLeft:
				ApplyAligment (AlignmentVertical.Upper, AlignmentHorizontal.Left);
				break;
			case TextAnchor.UpperRight:
				ApplyAligment (AlignmentVertical.Upper, AlignmentHorizontal.Right);
				break;
			}


			UpdateTextRendererBounds ();

		}


		private void ApplyAligment(AlignmentVertical vertical, AlignmentHorizontal horizontal) {

            var oldRotation = transform.rotation;
            transform.rotation = Quaternion.identity;

            float x = 0;
            float y = 0;

			switch(horizontal) {
			    case AlignmentHorizontal.Right: 
				    TextRenderer.alignment = TextAlignment.Right;
                    x = 0;
                    break;
			    case AlignmentHorizontal.Center:
				    TextRenderer.alignment = TextAlignment.Center;
                    x = 0.5f;
                    break;
			    case AlignmentHorizontal.Left:
				    TextRenderer.alignment = TextAlignment.Left;
                    x = 1;
                    break;
			}


			switch(vertical) {
			    case AlignmentVertical.Upper:
                    y = 1;
                    break;
			    case AlignmentVertical.Middle:
                    y = 0.5f;
                    break;
			    case AlignmentVertical.Lower:
                    y = 0;
                    break;
			}

            var anchor = new Vector2(x, y);


            var xPos = transform.position.x - Width / 2f + Width  * anchor.x;
            var yPos = transform.position.y - Height /2f + Height * anchor.y;

            TextRenderer.transform.localPosition = Vector3.zero;
            TextRenderer.transform.position = new Vector3(xPos, yPos, TextRenderer.transform.position.z);
                    

            UpdateTextRendererBounds();

            x = m_textBounds.center.x - m_textBounds.extents.x + m_textBounds.size.x * x;
            y = m_textBounds.center.y - m_textBounds.extents.y + m_textBounds.size.y * y;
          

            var pivotPoint = new Vector3(x, y, TextRenderer.transform.position.z);
            var diff = TextRenderer.transform.position - pivotPoint;
            TextRenderer.transform.position = TextRenderer.transform.position + diff;

            UpdateTextRendererBounds();

            transform.rotation = oldRotation;
        }


		private void UpdateTextRendererBounds () {

			var oldRotation = transform.rotation;
			transform.rotation = Quaternion.identity;

			m_textBounds = TextRenderer.GetComponent<Renderer> ().bounds;


			transform.rotation = oldRotation;
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

				text.gameObject.hideFlags = HideFlags.None;

				return text;
			}
		}

		private void CreateTextRenderer() {
			var obj = new GameObject ("Text");
			obj.AddComponent<TextMesh> ();
			obj.transform.parent = transform;
			obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;

            obj.transform.SetAsFirstSibling ();

		}
	}
}
