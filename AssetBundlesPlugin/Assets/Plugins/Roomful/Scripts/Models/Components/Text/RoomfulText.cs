using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RF.AssetBundles.Serialization;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RF.AssetWizzard {
	
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	public class RoomfulText : MonoBehaviour, IPropComponent
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
		public FontData FontData = FontData.defaultFontData;
		public Color Color = Color.white;
		public TextContent Source = new TextContent();

   
		private Bounds m_textBounds = new Bounds (Vector3.zero, Vector3.zero);
		private static float DEFAULT_TEXT_SCALE = 0.05f;

		public void Update () {
			Refersh ();
		}

		protected virtual void OnDrawGizmos () {
            

            if (Prop != null  && !Prop.DrawGizmos) {
                return;
            }
        

			SerializedBoundsIgnoreMarker sbim = GetComponent<SerializedBoundsIgnoreMarker> ();
			if (sbim == null) {
				GizmosDrawer.DrawCube (transform.position, transform.rotation, new Vector2(Width, Height), Color.white);
			} else {
				GizmosDrawer.DrawCube (transform.position, transform.rotation, new Vector2(Width, Height), Color.red);
			}

          //  GizmosDrawer.DrawCube(m_textBounds.center, transform.rotation, m_textBounds.size, Color.white);

        }


        public void Restore(SerializedText info) {
          
            PlaceHolderText = info.PlaceHolderText;
            Color = info.Color;
            FontData.font = info.Font;
            FontData.fontSize = info.FontSize;
            FontData.lineSpacing = info.LineSpacing;
            FontData.LineCharacterLimit = info.LineCharacterLimit;
            FontData.fontStyle = info.FontStyle;
            FontData.alignment = info.Alignment;
            FontData.horizontalOverflow = info.HorizontalOverflow;
            FontData.verticalOverflow = info.VerticalOverflow;


			Source.DataProvider = info.DataProvider;
			Source.ResourceIndex = info.ResourceIndex;
			Source.ResourceContentSource = info.ResourceContentSource;

            Refersh();

        }


        public void PrepareForUpalod() {

            var textInfo = gameObject.AddComponent<RF.AssetBundles.Serialization.SerializedText>();
            textInfo.PlaceHolderText = PlaceHolderText;
            textInfo.Color = Color;

			if(FontData.font != null) {
				textInfo.Font = FontData.font;
                textInfo.Font.material.shader = Shader.Find("GUI/Text Shader");
            }

           
            textInfo.FontSize = FontData.fontSize;
            textInfo.LineSpacing = FontData.lineSpacing;
            textInfo.LineCharacterLimit = FontData.LineCharacterLimit;
            textInfo.FontStyle = FontData.fontStyle;
            textInfo.Alignment = FontData.alignment;
            textInfo.HorizontalOverflow = FontData.horizontalOverflow;
            textInfo.VerticalOverflow = FontData.verticalOverflow;

			textInfo.DataProvider = Source.DataProvider;
			textInfo.ResourceIndex = Source.ResourceIndex;
			textInfo.ResourceContentSource = Source.ResourceContentSource;


#if UNITY_EDITOR
			if (textInfo.Font != null) {
				string fontFilePath = AssetDatabase.GetAssetPath(textInfo.Font);

                if (System.IO.File.Exists(fontFilePath)) {

                    //remove Assets/ string from a path. Yes I know that is not stable hack.
                    //If you know a better way, make it happend
                    fontFilePath = fontFilePath.Substring(7, fontFilePath.Length - 7);
                    byte[] data = SA.Common.Util.Files.ReadBytes(fontFilePath);

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



        public float Width {
			get {
				return RectTransform.sizeDelta.x * transform.lossyScale.x;
			}

		}

		public float Height {
			get {
				return RectTransform.sizeDelta.y * transform.lossyScale.y;
			}
		}


        public RectTransform RectTransform {
            get {
                var t = GetComponent<RectTransform>();
                //t.hideFlags = HideFlags.NotEditable;
                return t;
            }
        }

        public PropAsset Prop {
            get {
                Transform go = gameObject.transform;
                while (go != null) {
                    if (go.GetComponent<PropAsset>() != null) {
                        return go.GetComponent<PropAsset>();
                    }

                    go = go.parent;
                }

                return null;
            }
        }

        public Priority UpdatePriority {
            get {
                return Priority.High;
            }
        }

        private string WrapText(string text) {
      
            string[] words = text.Split(' ');

            StringBuilder result = new StringBuilder();

         
            string line = string.Empty;
            foreach(string word in words) {
                if(line.Equals(string.Empty)) {
                    line = word;
                    continue;
                }

                int charactedsCount = line.Length + word.Length;
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
			TextRenderer.fontSize = FontData.fontSize;
			TextRenderer.lineSpacing = FontData.lineSpacing;
			TextRenderer.fontStyle = FontData.fontStyle;
            TextRenderer.color = Color;
            TextRenderer.font = FontData.font;
            TextRenderer.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);

            if (TextRenderer.font != null) {

                Material m = new Material(TextRenderer.font.material);
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



            if (FontData.horizontalOverflow == SerializedTextWrapMode.Truncate) {
				while(m_textBounds.size.x > Width) {
                    DonwSizeTextScale(0.001f);
                    UpdateTextRendererBounds ();
				}
			}

			if(FontData.verticalOverflow == SerializedTextWrapMode.Truncate) {
				while(m_textBounds.size.y > Height) {
                    DonwSizeTextScale(0.001f);
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


        private void DonwSizeTextScale(float step) {
            var newScale = new Vector3(
                TextRenderer.transform.localScale.x - step,
                TextRenderer.transform.localScale.y - step,
                TextRenderer.transform.localScale.z - step
                );

            TextRenderer.transform.localScale = newScale;
        }

		private void ApplayAlligment(AlignmentVertical vertical, AlignmentHorizontal horizontal) {

            Quaternion oldRotation = transform.rotation;
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

            Vector2 anchor = new Vector2(x, y);


            float xPos = transform.position.x - Width / 2f + Width  * anchor.x;
            float yPos = transform.position.y - Height /2f + Height * anchor.y;

            TextRenderer.transform.localPosition = Vector3.zero;
            TextRenderer.transform.position = new Vector3(xPos, yPos, TextRenderer.transform.position.z);
                    

            UpdateTextRendererBounds();

            x = m_textBounds.center.x - m_textBounds.extents.x + m_textBounds.size.x * x;
            y = m_textBounds.center.y - m_textBounds.extents.y + m_textBounds.size.y * y;
          

            Vector3 pivotPoint = new Vector3(x, y, TextRenderer.transform.position.z);
            Vector3 diff = TextRenderer.transform.position - pivotPoint;
            TextRenderer.transform.position = TextRenderer.transform.position + diff;

            UpdateTextRendererBounds();

            transform.rotation = oldRotation;
        }


		private void UpdateTextRendererBounds () {

			Quaternion oldRotation = transform.rotation;
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

				text.gameObject.hideFlags = HideFlags.HideInHierarchy;

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
