using UnityEngine;

namespace net.roomful.assets.serialization
{
    [ExecuteInEditMode]
    [AddComponentMenu("Roomful/Disabled Area Marker")]
    public class SerializedDisabledAreaMarker : MonoBehaviour, IRecreatableOnLoad
    {

        protected  void OnDrawGizmos() {
        	if(Scene.ActiveAsset == null) {
                return;
            }

            if (!Scene.ActiveAsset.DrawGizmos) {
                return;
            }
            var m_size = Scene.GetBounds(gameObject, true);

            var rt = GetComponent<RoomfulText> ();

			if (rt == null) {
                GizmosDrawer.DrawCube(m_size.center, Quaternion.identity, m_size.size, Color.magenta);
			}
		}
    }
}
