using System.Collections.Generic;

namespace RF.AssetWizzard.Network.Request {
    public class Signin : BaseWebPackage {

        private const string REQUEST_URL = "/auth/signin";

        private string m_mail = string.Empty;
        private string m_password = string.Empty;

        public Signin(string mail, string password) {
            m_mail = mail;
            m_password = password;
        }

        public override string Url {
            get {
                return REQUEST_URL;
            }
        }

        public override Dictionary<string, object> GetRequestData() {
            Dictionary<string, object> fields = new Dictionary<string, object>();
            fields.Add("email", m_mail);
            fields.Add("password", m_password);
            return fields;
        }
    }
}