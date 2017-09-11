using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Moon.Network.Web;


namespace RF.AssetWizzard.Network 
{

	public class AssetsListsCallback : BaseRoomfulCallback {

        [Param]
        public string Id;

        [Param]
        public string Name;


        [Param]
        public string Descr;


        public override void OnResult(byte[] data) {

			List<object> allAssetsList = SA.Common.Data.Json.Deserialize(DataAsString) as List<object>;

			foreach(object assetData in allAssetsList) {

				object tpl = new AssetTemplate ();
				Parser.Parse (assetData, ref  tpl);

				Debug.Log ((tpl as AssetTemplate).Id);
				Debug.Log ((tpl as AssetTemplate).Created);
				Debug.Log ((tpl as AssetTemplate).Updated);

			}
		}

    
	}

}
