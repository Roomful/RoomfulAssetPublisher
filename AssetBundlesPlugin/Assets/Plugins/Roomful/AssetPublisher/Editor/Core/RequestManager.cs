using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace RF.AssetWizzard.Editor {

	public static class RequestManager  {

        
		public static void RemoveAsset(Template tpl) {
			Network.Request.RemoveAsset removeRequest = new Network.Request.RemoveAsset (tpl.Id);

			removeRequest.PackageCallbackData = (removeCallback) => {
				AssetBundlesSettings.Instance.RemoveSavedTemplate(tpl);
			};

			removeRequest.Send ();
		}



	}

}
