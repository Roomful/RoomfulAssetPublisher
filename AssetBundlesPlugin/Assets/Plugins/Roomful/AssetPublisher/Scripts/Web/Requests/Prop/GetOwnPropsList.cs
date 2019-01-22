﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request {
	public class GetOwnPropsList : GetAssetsList
    {
        private const string REQUEST_URL = "/api/v0/assetpublisher/prop/listOwn";

		public GetOwnPropsList(int offset, int size, List<ReleaseStatus> statuses):base(offset, size, statuses) {}
		public GetOwnPropsList(int offset, int size):base(offset, size) {}

        public override string Url {
            get {
                return REQUEST_URL;
            }
        }

        
    }
}
