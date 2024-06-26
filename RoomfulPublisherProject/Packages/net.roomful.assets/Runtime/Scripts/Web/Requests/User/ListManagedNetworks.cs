using System.Collections.Generic;

namespace net.roomful.assets
{
    
    /// <summary>
    /// Api returns list of networks where user have permissions to manage assets.
    /// https://github.com/Roomful/RoomfulUnity1/wiki/RF_API-Assets#list-asset-managed-networks
    /// </summary>
    class ListManagedNetworks : BaseWebPackage  {

        public const string PackUrl = "/api/v0/asset/listManagedNetworks";
        
        public ListManagedNetworks():base(PackUrl, RequestMethods.POST) {
            
        }
        
        public override Dictionary<string, object> GenerateData () {
            var originalJson =  new Dictionary<string, object>();
            return originalJson;
        }
    }
}
