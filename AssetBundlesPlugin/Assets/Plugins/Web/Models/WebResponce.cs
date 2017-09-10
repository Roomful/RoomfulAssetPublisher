using System;
using System.Net;

namespace Moon.Network.Web
{
    public class WebResponce
    {

        private byte[] m_data;
        private Exception m_error;

        public WebResponce(byte[] data, Exception error) {
            m_data = data;
            m_error = error;
        }


        public byte[] Data {
            get {
                return m_data;
            }
        }

        public Exception Erorr {
            get {
                return m_error;
            }
        }


        public bool IsSucceeded {
            get {
                return m_error == null;
            }
        }

        public bool IsFiled {
            get {
                return !IsSucceeded;
            }
        }

        public HttpStatusCode StatusCode {
            get {
                if (IsSucceeded) {
                    return HttpStatusCode.OK;
                }

                if (m_error is WebException) {

                    if ((m_error as WebException).Response != null) {
                        HttpWebResponse r = (HttpWebResponse)(m_error as WebException).Response;
                        return r.StatusCode;
                    } else {
                        return HttpStatusCode.HttpVersionNotSupported;
                    }
                } else {
                    return HttpStatusCode.HttpVersionNotSupported;
                }

            }
        }

    }
}