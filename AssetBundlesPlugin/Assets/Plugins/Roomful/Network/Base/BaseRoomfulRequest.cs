using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Moon.Network.Web;


namespace RF.AssetWizzard.Network 
{
	
	public abstract class BaseRoomfulRequest :  Request<BaseRoomfulRequestCallback> {


		public BaseRoomfulRequest(string path) {

           
            SetPath (path);
			SetMethod(Method.POST);

			SetDataReader(new JSONDataReader());
			SetCommunicator (RoomfulComunicator.Instance);
        }

     

	}
}
