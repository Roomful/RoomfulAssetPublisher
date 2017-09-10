using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Net;
using System.Reflection;

namespace Moon.Network.Web {

    public class ServerCommunicator : IServerCommunicator {




        public ServerCommunicator() {
         
        }


        public void Send(IRequest request) {


            Debug.Log("ServerCommunicator send");

            var p = new ThreadedWebRequest("https://google.com", request);
            p.Run(() => {
                Debug.Log("Done");
            });

        }



        public string BaseURL {
            get {
                return "https://requestb.in/11mnuv51";
            }
        }

	}

}