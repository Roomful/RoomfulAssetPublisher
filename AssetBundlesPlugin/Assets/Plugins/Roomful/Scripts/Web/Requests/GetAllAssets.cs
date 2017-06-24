using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request {
	public class GetAllAssets : BaseWebPackage {

		public const string PackUrl = "/api/v0/asset/list";
		private string Title = string.Empty;
		private List<string> Tags = new List<string>();
		private int Offset = 0;
		private int Size = 10;



	

		public GetAllAssets(int offset, int size, List<string> tags):base(PackUrl) {
			Offset = offset;
			Size = size;

			Tags.AddRange (tags);
		}

		public GetAllAssets(int offset, int size, string title):base(PackUrl) {
			Offset = offset;
			Size = size;
			Title = title;
		}

		public override Dictionary<string, object> GenerateData () {
			Dictionary<string, object> OriginalJSON =  new Dictionary<string, object>();

			if(Tags.Count > 0) {
				OriginalJSON.Add ("tags", Tags);
			}

			if(!Title.Equals(string.Empty)) {
				OriginalJSON.Add ("title", Title);
			}

			OriginalJSON.Add ("Offset", Offset);
			OriginalJSON.Add ("size", Size);

			return OriginalJSON;
		}
	}
}
