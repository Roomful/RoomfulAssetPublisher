using System.Collections;
using System.Collections.Generic;
using SA.Common.Data;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request {
	public abstract class GetAssetsList : BaseWebPackage {

		public const string PackUrl = "/api/v0/asset/list";
		private string Title = string.Empty;
		private string Id = string.Empty;
		private List<string> Tags = new List<string>();
		private int Offset = 0;
		private int Size = 10;



		public GetAssetsList(int offset, int size, List<string> tags, string url):base(url) {
			Offset = offset;
			Size = size;

			Tags.AddRange (tags);
		}

		public GetAssetsList(int offset, int size, string title, string url):base(url) {
			Offset = offset;
			Size = size;
			Title = title;
		}

		public void SetId(string id) {
			Id = id;
		}

		public override Dictionary<string, object> GenerateData () {
			Dictionary<string, object> OriginalJSON =  new Dictionary<string, object>();

			if(Tags.Count > 0) {
				OriginalJSON.Add ("tags", Tags);
			}

			if(!Title.Equals(string.Empty)) {
				OriginalJSON.Add ("title", Title);
			}

			if(!Id.Equals(string.Empty)) {
				OriginalJSON.Add ("id", Id);
			}


			OriginalJSON.Add ("offset", Offset);
			OriginalJSON.Add ("size", Size);

			Debug.Log(Json.Serialize(OriginalJSON));
			return OriginalJSON;
		}
	}
}
