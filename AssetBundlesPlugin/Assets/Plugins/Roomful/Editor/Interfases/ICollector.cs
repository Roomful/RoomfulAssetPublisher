using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetBundles {
	public interface ICollector  {
		void Run(RF.AssetWizzard.PropAsset propAsset);
	}
}

