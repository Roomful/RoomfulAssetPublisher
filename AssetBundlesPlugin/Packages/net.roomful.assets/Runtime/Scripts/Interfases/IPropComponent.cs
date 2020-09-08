using UnityEngine;

namespace net.roomful.assets {

	public interface IPropComponent  {

		void PrepareForUpalod();
		void RemoveSilhouette();

		void Update();


        Priority UpdatePriority { get; }
        GameObject gameObject { get; }

    }

}
