using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Moon.Network.Web;

namespace RF.AssetWizzard.Network 
{

	public class RoomfulComunicator : SA.Common.Pattern.NonMonoSingleton<RoomfulComunicator>, IServerCommunicator {


		public void Send(IRequest request) {


            request.Headers.Add(WebServer.HeaderSessionId, AssetBundlesSettings.Instance.SessionId);

            var p = new ThreadedWebRequest(AssetBundlesSettings.WEB_SERVER_URL, request);
			p.Run(() => {

            });

		}

	}
}