using System.Collections.Generic;
using RF.AssetWizzard.Models;
using RF.AssetWizzard.Network.Request;
using RF.AssetWizzard.Results;

namespace RF.AssetWizzard.Commands {
    
    public class GetUserTemplateCommand : BaseNetworkCommand<GetUserTemplateResult> {
        
        protected override BaseWebPackage GetRequest() {
            return new GetUserTemplate();
        }

        protected override void SuccessHandler(string response) {
            var originalJson = SA.Common.Data.Json.Deserialize(response) as Dictionary<string, object>;
            var data = new JSONData(originalJson);
            var dataInfo = data.GetValue<Dictionary<string, object>>("data");
            
            var userDataInfo = new JSONData(dataInfo);
            var userInfo = userDataInfo.GetValue<Dictionary<string, object>>("user");
           
            FireComplete(new GetUserTemplateResult(new UserTemplate(userInfo)));
        }

        protected override void ErrorHandler(long obj) {
            FireComplete(new GetUserTemplateResult());
        }
    }
}