using RF.AssetWizzard.Editor;
using RF.AssetWizzard.Results;

namespace RF.AssetWizzard.Commands {

    public class LogoutCommand : AbstractCommand<BaseCommandResult> {

        protected override void ExecuteImpl() {
            AssetBundlesSettings.Instance.SetSessionId(string.Empty);
            BundleUtility.ClearLocalCache();
            FireComplete(new BaseCommandResult(true));
        }
    }
}
