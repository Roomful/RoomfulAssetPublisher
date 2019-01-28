using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard {

	public interface IPropComponent  {

		void PrepareForUpload();
		void RemoveSilhouette();

		void Update();


        Priority UpdatePriority { get; }
        GameObject gameObject { get; }

    }

}
