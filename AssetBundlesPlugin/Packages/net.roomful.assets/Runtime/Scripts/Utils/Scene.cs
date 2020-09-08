using System.Collections.Generic;
using UnityEngine;

using net.roomful.assets.serialization;

namespace net.roomful.assets
{
	public static class Scene  {

		public static void Update() {

            var components =  Object.FindObjectsOfType<Component>();
            var propComponents = new List<IPropComponent>();

 
			foreach(var component in components) {
				if(component is IPropComponent) {
                    if (!IsAnchored(component)) {
                        propComponents.Add(component as IPropComponent);
                    }  
                }
			}

            propComponents.Sort(new PriorityComparer());

            foreach(var c in propComponents) {
                c.Update();
            }
            

        }

        private static IAsset m_activeAsset = null;
        public static IAsset ActiveAsset {
            get {
                if(m_activeAsset ==  null) {
                    m_activeAsset = FindObjectWithType<IAsset>();
                }

                return m_activeAsset;
            }
        }


        public static T FindObjectWithType<T>() {
            var allFindedObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject));
            foreach (GameObject gameObject in allFindedObjects) {
                var target = gameObject.GetComponent<T>();

                if (target != null) {
                    return target;
                }
            }
            return default(T);
        }



        public static bool IsAnchored(Component comp) {

            var testedObject = comp.transform;
            while (testedObject != null) {
                var anchor = testedObject.GetComponent<PropAnchor>();
                if (anchor != null) {
                    if(anchor.GetInstanceID() != comp.GetInstanceID()) {
                        return true;
                    }  
                }
                testedObject = testedObject.parent;
            }


            return false;
        }




        public  static Bounds GetBounds(Transform transfrom, bool includeIgnoredLayers = false) {
			return GetBounds (transfrom.gameObject, includeIgnoredLayers);
		}

		public  static Bounds GetBounds(GameObject go, bool includeIgnoredLayers = false) {
			
			var hasBounds = false;
			var bounds = new Bounds(Vector3.zero, Vector3.zero);

			var ChildrenRenderer = go.GetComponentsInChildren<Renderer>();
		//	Quaternion oldRotation = go.transform.rotation;
		//	go.transform.rotation = Quaternion.identity;

			foreach (var child in ChildrenRenderer) {

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


			var TextRenderer = go.GetComponentsInChildren<RoomfulText>();
			foreach (var text in TextRenderer) {

				if (IsIgnored(text.transform) && !includeIgnoredLayers) {
					continue;
				}

				var b = new Bounds (text.transform.position, new Vector3 (text.Width, text.Height, 0f));
				if (!hasBounds) {
					bounds = b;
					hasBounds = true;
				} else {
					bounds.Encapsulate(b);
				}

			}


		//	go.transform.rotation = oldRotation;


			return bounds;

		}

		public static bool IsIgnored(Transform go) {

			var testedObject = go;
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
