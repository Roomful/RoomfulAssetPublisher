using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Moon.Network.Web {

	public abstract class Request<T> : IRequest  where T : IRequestCallback, new(){


		private event Action<T> m_callback = delegate {};

		private Method m_method = Method.UNDEFINED;
        private string m_path = string.Empty;
        private IDataReader m_dataReader = null;
		private IServerCommunicator m_communicator = null;
		private Dictionary<string, string> m_headers =  new Dictionary<string, string>();




		public void Send (Action<T> callback = null) {
			if(m_communicator == null) {
				Debug.LogError ("No comunicator was set for " +  this.GetType().FullName + " request");
			}

			if(callback != null) {
				m_callback += callback;
			}

			m_communicator.Send (this);
		}

		public void AddCallback(Action<T> callback) {
			m_callback += callback;
		}


        public void AddHeader(System.Net.HttpRequestHeader key, string value = null) {
            AddHeader(key.ToString(), value);
        }

		public void AddHeader(string key, string value = null) {
			m_headers [key] = value;
		}
   

		public void SetCommunicator(IServerCommunicator communicator) {
			m_communicator = communicator;
		}


		public void SetMethod(Method method) {
            m_method = method;
        }

        public void SetPath(string path) {
            m_path = path;
        }

        public void SetDataReader(IDataReader dataReader) {
            m_dataReader = dataReader;
            m_dataReader.SetRequest(this);
        }



        public string Path {
            get {
                return m_path;
            }
        }


        public Method Method {
            get {
                return m_method;
            }
        }



        public IDataReader DataReader {
            get {
                return m_dataReader;
            }
        }

        public Dictionary<string, string> Headers {
            get {
                return m_headers;
            }
        }

		public IRequestCallback CreateRequestCallbackObject() {
            var callbackObjects = new T();
            callbackObjects.Request = this;

            return callbackObjects;
        }

        public void Finish(IRequestCallback result) {
            m_callback((T) result);
        }

	}
}
