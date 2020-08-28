using UnityEngine;

namespace RF.AssetWizzard {

	public interface IPropComponent  {

		void PrepareForUpalod();
		void RemoveSilhouette();

		void Update();


        Priority UpdatePriority { get; }
        GameObject gameObject { get; }

    }

}
