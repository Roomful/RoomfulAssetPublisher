﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RF.AssetWizzard;

namespace RF.AssetBundles.Serialization
{
    [ExecuteInEditMode]
    [AddComponentMenu("Roomful/Bounds Ignore Marker")]
    public class SerializedBoundsIgnoreMarker : MonoBehaviour, IRecreatableOnLoad
    {
        private Bounds m_size = new Bounds(Vector3.zero, Vector3.zero);

        protected  void OnDrawGizmos() {
        	if(Prop == null) {
                return;
            }

            if (!Prop.DrawGizmos) {
                return;
            }

			RoomfulText rt = GetComponent<RoomfulText> ();

			if (rt == null) {
				GizmosDrawer.DrawCube(m_size.center, transform.rotation, m_size.size, Color.red);
			}
		}

		public void Update() {
			m_size = Scene.GetBounds (gameObject, true);
        }

     

        public PropAsset Prop {
            get {
                Transform go = gameObject.transform;
                while(go != null) {
                    if(go.GetComponent<PropAsset>() != null) {
                        return go.GetComponent<PropAsset>();
                    }

                    go = go.parent;
                }

                return null;
            }
        }
    }
}
