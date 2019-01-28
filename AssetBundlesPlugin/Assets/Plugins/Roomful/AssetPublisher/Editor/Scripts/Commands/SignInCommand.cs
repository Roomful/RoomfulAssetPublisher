using System;
using System.Collections.Generic;
using RF.AssetWizzard.Network.Request;

namespace RF.AssetWizzard.Commands {

    public class SignInCommand : BaseNetworkCommand<BaseCommandResult> {
        private string m_mail;
        private string m_password;

        public SignInCommand(string mail, string password) {
            m_mail = mail;
            m_password = password;
        }

        protected override void ErrorHandler(long obj) {
            FireComplete(new BaseCommandResult(false));
        }

        protected override BaseWebPackage GetRequest() {
            return new Signin(m_mail, m_password);
        }

        protected override void SuccessHandler(string response) {
            Dictionary<string, object> originalJson = SA.Common.Data.Json.Deserialize(response) as Dictionary<string, object>;
            AssetBundlesSettings.Instance.SetSessionId(originalJson["session_token"].ToString());
            FireComplete(new BaseCommandResult(true));
        }

    }
}
