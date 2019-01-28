using System;
using System.Collections.Generic;
using RF.AssetWizzard.Network.Request;
using RF.AssetWizzard.Editor;

namespace RF.AssetWizzard.Commands {

    public class LogoutCommand : AbstractCommand<BaseCommandResult> {

        protected override void ExecuteImpl() {
            AssetBundlesSettings.Instance.SetSessionId(string.Empty);
            BundleUtility.ClearLocalCache();
            FireComplete(new BaseCommandResult(true));
        }

    }
}
