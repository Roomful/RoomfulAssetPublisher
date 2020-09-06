using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using net.roomful.assets.serialization;

namespace net.roomful.assets
{

    [ExecuteInEditMode]
	public class PropAnchor : MonoBehaviour, IPropComponent {
        

        public void Update() {

            Settings.Anchor.x = Mathf.Clamp(Settings.Anchor.x, 0f, 1f);
            Settings.Anchor.y = Mathf.Clamp(Settings.Anchor.y, 0f, 1f);
            Settings.Anchor.z = Mathf.Clamp(Settings.Anchor.z, 0f, 1f);


            Settings.RendererPivot.x = Mathf.Clamp(Settings.RendererPivot.x, 0f, 1f);
            Settings.RendererPivot.y = Mathf.Clamp(Settings.RendererPivot.y, 0f, 1f);
            Settings.RendererPivot.z = Mathf.Clamp(Settings.RendererPivot.z, 0f, 1f);

            Settings.XSize = Mathf.Clamp(Settings.XSize, 0.001f, 1f);
            Settings.YSize = Mathf.Clamp(Settings.YSize, 0.001f, 1f);


		
            if (Settings.SmartParent != null) {

   
                Bounds parentBounds = Scene.GetBounds (Settings.SmartParent);
				transform.position = parentBounds.center;

				float xPos = parentBounds.center.x - parentBounds.extents.x + parentBounds.size.x * Settings.Anchor.x;
				float yPos = parentBounds.center.y - parentBounds.extents.y + parentBounds.size.y * Settings.Anchor.y;
				float zPos = parentBounds.center.z - parentBounds.extents.z + parentBounds.size.z * Settings.Anchor.z;


                transform.position = new Vector3(xPos, yPos, zPos);
				Bounds bounds = Scene.GetBounds (gameObject, true);

				if(Settings.UseRendererPivot) {

					float x = bounds.center.x - bounds.extents.x + bounds.size.x * Settings.RendererPivot.x;
					float y = bounds.center.y - bounds.extents.y + bounds.size.y * Settings.RendererPivot.y;
					float z = bounds.center.z - bounds.extents.z + bounds.size.z * Settings.RendererPivot.z;


					Vector3 anchorPoint = new Vector3 (x, y, z);

					Vector3 diff = transform.position - anchorPoint;
					transform.position = transform.position + diff;


				}


                transform.localPosition = transform.localPosition + (Settings.Offset / CurrentProp.Scale);


                if (Settings.EnableXScale) {
                    var text = GetComponent<RoomfulText>();
                    if(text != null) {
						float x = parentBounds.size.x * Settings.XSize / text.transform.lossyScale.x;
						text.RectTransform.sizeDelta = new Vector2(x, text.RectTransform.sizeDelta.y);
                    }
                }

                if (Settings.EnableYScale) {
                    var text = GetComponent<RoomfulText>();
                    if (text != null) {
						float y = parentBounds.size.y * Settings.YSize / text.transform.lossyScale.y;
						text.RectTransform.sizeDelta = new Vector2(text.RectTransform.sizeDelta.x, y);
                    }
                }

                UpdateChildComponents();
            }

        }

        private void UpdateChildComponents() {
            Component[] components = transform.GetComponentsInChildren<Component>();
            List<IPropComponent> propComponents = new List<IPropComponent>();


            foreach (var component in components) {
                if (component is IPropComponent && (component.GetInstanceID() != GetInstanceID())) {
                    propComponents.Add(component as IPropComponent);
                }
            }

            propComponents.Sort(new PriorityComparer());

            foreach (var c in propComponents) {
                c.Update();
            }
        }


		public void PrepareForUpalod() {
            DestroyImmediate(this);
		}

		public void RemoveSilhouette() {
			
		}


        public Priority UpdatePriority {
            get {
                return Priority.Lowest;
            }
        }

        public SerializedAnchor Settings {
            get {

                var settings = GetComponent<SerializedAnchor>();
                if (settings == null) {
                    settings = gameObject.AddComponent<SerializedAnchor>();
                }

               // settings.hideFlags = HideFlags.HideInInspector;

                return settings;
            }
        }


        private PropAsset CurrentProp {
            get {
                return GameObject.FindObjectOfType<PropAsset>();
            }
        }

    }
}