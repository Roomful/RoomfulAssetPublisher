﻿namespace net.roomful.assets {
    internal class StyleMetaDataCreate : AssetMetadataRequest
    {

        public const string RequestUrl = "/api/v0/asset/style/create";

        public StyleMetaDataCreate(StyleAssetTemplate assetTemplate) : base(RequestUrl) {
            SetTemplate(assetTemplate);
        }
       
    }
}
