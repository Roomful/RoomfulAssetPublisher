﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RF.AssetBundles.Serialization;

namespace RF.AssetWizzard
{
	public static class Scene  {

		public static void Update() {
			MonoBehaviour[] components = GameObject.FindObjectsOfType<MonoBehaviour> ();
			foreach(var component in components) {
				if(component is IPropComponent) {
					(component as IPropComponent).Update ();
				}

				if(component is SerializedBoundsIgnoreMarker) {
					(component as SerializedBoundsIgnoreMarker).Update ();
				}


			}
		}




		public  static Bounds GetBounds(Transform transfrom, bool includeIgnoredLayers = false) {
			return GetBounds (transfrom.gameObject, includeIgnoredLayers);
		}

		public  static Bounds GetBounds(GameObject go, bool includeIgnoredLayers = false) {
			
			bool hasBounds = false;
			var bounds = new Bounds(Vector3.zero, Vector3.zero);

			Renderer[] ChildrenRenderer = go.GetComponentsInChildren<Renderer>();
			Quaternion oldRotation = go.transform.rotation;
			go.transform.rotation = Quaternion.identity;

			foreach (Renderer child in ChildrenRenderer) {

				if (IsIgnored(child.transform) && !includeIgnoredLayers) {
					continue;
				}

				if (!hasBounds) {
					bounds = child.bounds;
					hasBounds = true;
				} else {
					bounds.Encapsulate(child.bounds);
				}
			}


			RoomfulText[] TextRenderer = go.GetComponentsInChildren<RoomfulText>();
			foreach (var text in TextRenderer) {

				if (IsIgnored(text.transform) && !includeIgnoredLayers) {
					continue;
				}

				Bounds b = new Bounds (text.transform.position, new Vector3 (text.Width, text.Height, 0f));
				if (!hasBounds) {
					bounds = b;
					hasBounds = true;
				} else {
					bounds.Encapsulate(b);
				}

			}


			go.transform.rotation = oldRotation;


			return bounds;

		}

		public static bool IsIgnored(Transform go) {

			Transform testedObject = go;
			while (testedObject != null) {
				if (testedObject.GetComponent<SerializedBoundsIgnoreMarker>() != null) {
					return true;
				}
				testedObject = testedObject.parent;
			}


			return false;
		}



	}
}
