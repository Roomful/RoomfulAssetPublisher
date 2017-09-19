﻿using System.Collections;
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
		public string FullFontName;

        public string PlaceHolderText = "Hello Roomful";
        public Color Color = Color.white;


		public SerializedDataProvider DataProvider = SerializedDataProvider.Prop;
		public int ResourceIndex = 0;
		public SerializedResourceTextContentSource ResourceContentSource = SerializedResourceTextContentSource.Title;

		public byte[] FontFileContent;

    }
}




