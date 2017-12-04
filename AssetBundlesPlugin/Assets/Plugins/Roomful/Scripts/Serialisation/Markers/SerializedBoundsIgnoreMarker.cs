using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RF.AssetWizzard;

namespace RF.AssetBundles.Serialization
{
    [ExecuteInEditMode]
    [AddComponentMenu("Roomful/Bounds Ignore Marker")]
    public class SerializedBoundsIgnoreMarker : MonoBehaviour, IRecreatableOnLoad
    {

        protected  void OnDrawGizmos() {
        	if(Scene.ActiveAsset == null) {
                return;
            }

            if (!Scene.ActiveAsset.DrawGizmos) {
                return;
            }
            Bounds m_size = Scene.GetBounds(gameObject, true);

            RoomfulText rt = GetComponent<RoomfulText> ();

			if (rt == null) {
				GizmosDrawer.DrawCube(m_size.center, Quaternion.identity, m_size.size, Color.red);
			}
		}
    }
}
