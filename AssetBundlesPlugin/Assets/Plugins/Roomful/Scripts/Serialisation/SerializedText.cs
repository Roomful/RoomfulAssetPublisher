using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetBundles.Serialisation
{
    public class SerializedText : MonoBehaviour
    {

        public Font Font;
        public int FontSize;
        public float LineSpacing;
        public FontStyle FontStyle;
        public TextAnchor Alignment;
        public SerializedTextWrapMode HorizontalOverflow;
        public SerializedTextWrapMode VerticalOverflow;

        public string PlaceHolderText = "Hello Roomful";
        public Color Color = Color.white;

    }
}




