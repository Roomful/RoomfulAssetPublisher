using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard {

	public interface IPropComponent  {

		void PrepareForUpalod();
		void RemoveSilhouette();

		void Update();
	}

}
