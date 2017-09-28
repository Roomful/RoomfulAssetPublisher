using System;
using UnityEngine;
using RF.AssetBundles.Serialization;

namespace RF.AssetWizzard
{
	[Serializable]
	public class TextContent  {


		public SerializedDataProvider DataProvider = SerializedDataProvider.Prop;

		public int ResourceIndex = 0;
		public SerializedResourceTextContentSource ResourceContentSource = SerializedResourceTextContentSource.Title;


	
	}
}