﻿using UnityEngine;
using System.Collections;

namespace RF.AssetWizzard.Results {

    public class RemoveAssetResult : BaseCommandResult {
        public string AssetId { get; private set; }

        public RemoveAssetResult() : base(false) {

        }

        public RemoveAssetResult(string assetId) : base(true) {
            AssetId = assetId;
        }
    }
}