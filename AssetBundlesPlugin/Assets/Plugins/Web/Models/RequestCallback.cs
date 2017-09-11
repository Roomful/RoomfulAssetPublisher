using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Moon.Network.Web {

	public abstract class RequestCallback : IRequestCallback  {


        public IRequest m_request;

        protected IDataWriter m_dataWriter = null;
        protected WebResponce m_responce;
      

        public virtual void SetResponce(WebResponce reponce) {
            m_responce = reponce;
            if (reponce.IsSucceeded) {

                if(m_dataWriter != null) {
                    m_dataWriter.Parse(reponce.Data);
                }

                OnResult(reponce.Data);
            } else {
                OnError(reponce.Erorr);
            }
        }

        public abstract void OnResult(byte[] data);
        public abstract void OnError(System.Exception e);


        public void SetDataWriter(IDataWriter dataWriter) {
            m_dataWriter = dataWriter;
            m_dataWriter.SetResult(this);
        }

       
        public byte[] Data {
            get {
                return Responce.Data;
            }
        }

        public string DataAsString {
            get {
                if(Responce.Data != null) {
                    return System.Text.Encoding.UTF8.GetString(Responce.Data);
                } else {
                    return string.Empty;
                }
            }
        }


        public WebResponce Responce {
            get {
                return m_responce;
            }
        }

        public IRequest Request {
            get {
                return m_request;
            } 

            set {
                m_request = value;
            }
        }

    }
}
