using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Moon.Network.Web;



namespace RF.AssetWizzard.Network 
{

	public class BaseRoomfulCallback : RequestCallback {

        public BaseRoomfulCallback() {
            SetDataWriter(new JSONDataWriter());
        }

        public override void OnResult(byte[] data) {
            Debug.Log(Request.GetType().Name + "::IN::" + DataAsString);
		}

		public override void OnError(Exception e) {
			Debug.Log("Status Code: " + Responce.StatusCode);
			Debug.Log(e.Message);
		}

	}
}
