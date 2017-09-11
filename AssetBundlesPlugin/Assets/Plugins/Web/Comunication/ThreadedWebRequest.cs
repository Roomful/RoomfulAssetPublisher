using System.Threading;
using UnityEngine;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net;


namespace Moon.Network.Web
{

    public class ThreadedWebRequest //<T> where T : IWebRequest, new()
    {

        private IRequest m_request;
        private Thread m_thread;
        private string m_url;

        public ThreadedWebRequest(string url, IRequest request) {
            m_request = request;
            m_url = url;
        }

        
        public Thread Run(Action<IRequest, IRequestCallback> callback) {

          

            m_thread = new Thread(() => {
 
                try {
                   
                  //  this.

                    WebClientImpl webRequest = new WebClientImpl();

                    webRequest.SetURL(m_url + m_request.Path);
                    webRequest.SetData(m_request.DataReader.Data);

                    foreach (var pair in m_request.DataReader.Body) {
                        webRequest.AddParam(pair.Key, ParamToString(pair.Value));
                    }

                    foreach (var header in m_request.Headers) {
                        webRequest.AddHeader(header.Key, header.Value);
                    }

                    webRequest.Send(m_request.Method, (responce) => {
                        IRequestCallback result = m_request.CreateRequestCallbackObject();
                        result.SetResponce(responce);



                       // UnityMainThreadDispatcher.Instance.Enqueue(() => {
                            callback(m_request, result);
                     //   });
                        

                    });


                } catch (Exception ex) {
                    Debug.LogError(ex.Message);
                    Debug.LogError(ex.StackTrace);
                }

             

            });
            m_thread.Start();
            return m_thread;
        }

  
        public Thread Thread {
            get {
                return m_thread;
            }
        }


        private string ParamToString(object value) {
            if(value == null) {
                return string.Empty;
            } else {
                return value.ToString();
            }
        }



    }
}


