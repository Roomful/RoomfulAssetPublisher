using RF.AssetWizzard.Models;

namespace RF.AssetWizzard.Results {

    public class GetUserTemplateResult : BaseCommandResult {

        public UserTemplate User { get; }

        public GetUserTemplateResult() : base(false) {}

        public GetUserTemplateResult(UserTemplate user) : base(true) {
            User = user;
        }
    }
}