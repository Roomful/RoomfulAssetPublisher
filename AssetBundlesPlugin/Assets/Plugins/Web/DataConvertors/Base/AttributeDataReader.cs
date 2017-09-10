using UnityEngine;
using System.Reflection;
using System.Collections.Generic;



namespace Moon.Network.Web
{

    public abstract class AttributesBasedDataReader : IDataReader {

        protected IRequest m_request;
        protected Dictionary<string, object> m_body = null;
        private bool m_ignoreNullValues = false;

        public void SetRequest(IRequest request) {
            m_request = request;
        }


        protected virtual void CreateBody() {
            m_body = new Dictionary<string, object>();


            FieldInfo[] fields = m_request.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            foreach (FieldInfo field in fields) {
                object[] attrs = field.GetCustomAttributes(true);
                foreach (object attr in attrs) {

                    if (attr is ParamAttribute) {
                        string name = (attr as ParamAttribute).Name;
                        if (string.IsNullOrEmpty(name)) {
                            name = field.Name;
                        }

                        object val = field.GetValue(m_request);
                        if(val == null && m_ignoreNullValues) {
                            continue;
                        }
                        AddToBody(name, val);
                    }
                }
            }
        }


        private void AddToBody(string key, object value) {
            m_body[key] = value;
        }


        public bool IgnoreNullValues {
            get {
                return m_ignoreNullValues;
            }

            set {
                m_ignoreNullValues = value;
            }
        }


        public Dictionary<string, object> Body {
            get {

                if(m_body == null) {
                    CreateBody();
                }


                return m_body;
            }
        }



        public abstract byte[] Data { get; }
       

    }
}
