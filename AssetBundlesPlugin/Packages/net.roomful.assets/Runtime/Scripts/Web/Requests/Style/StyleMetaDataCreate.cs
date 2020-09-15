﻿namespace net.roomful.assets.Network.Request {
    internal class StyleMetaDataCreate : AssetMetadataRequest
    {

        public const string RequestUrl = "/api/v0/asset/style/create";

        public StyleMetaDataCreate(StyleTemplate template) : base(RequestUrl) {
            SetTemplate(template);
        }
       
    }
}