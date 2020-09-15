﻿namespace net.roomful.assets.Network.Request {
	public class EnvironmentMetaDataCreate : AssetMetadataRequest
    {

        public const string RequestUrl = "/api/v0/asset/environment/create";

        public EnvironmentMetaDataCreate(EnvironmentTemplate template) : base(RequestUrl) {
            SetTemplate(template);
        }
       
    }
}