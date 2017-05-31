using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RF.AssetWizzard.Network.Request {
	public class LoadRecourceThumbnail : BaseWebPackage {

		private const string PackUrl = "/api/v0/resource/";
		private const RequestMethods PackMethodName = RequestMethods.GET;



		public LoadRecourceThumbnail (Resource res) : base (PackUrl, PackMethodName) {
			AddToUrl (res.Id);
		}



		public override Dictionary<string, object> GenerateData () {
			Dictionary<string, object> OriginalJSON =  new Dictionary<string, object>();

			return OriginalJSON;
		}
	}
}