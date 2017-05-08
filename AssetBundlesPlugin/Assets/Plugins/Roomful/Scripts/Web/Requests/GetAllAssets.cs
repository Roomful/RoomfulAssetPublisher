using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request {
	public class GetAllAssets : BaseWebPackage {

		public const string PackUrl = "/api/v0/asset/list";
		private List<string> Tags = new List<string>();
		private int Offset = 0;
		private int Size = 10;

		public GetAllAssets():base(PackUrl) {

		}

		public GetAllAssets(int offset, int size):base(PackUrl) {
			Offset = offset;
			Size = size;
		}

		public GetAllAssets(int offset, int size, List<string> tags):base(PackUrl) {
			Offset = offset;
			Size = size;

			Tags.AddRange (tags);
		}

		public override Dictionary<string, object> GenerateData () {
			Dictionary<string, object> OriginalJSON =  new Dictionary<string, object>();

			OriginalJSON.Add ("tags", Tags);
			OriginalJSON.Add ("Offset", Offset);
			OriginalJSON.Add ("size", Size);

			return OriginalJSON;
		}
	}
}
