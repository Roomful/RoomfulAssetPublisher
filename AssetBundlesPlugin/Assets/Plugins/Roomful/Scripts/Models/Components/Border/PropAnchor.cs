using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RF.AssetBundles.Serialization;

namespace RF.AssetWizzard
{

    [ExecuteInEditMode]
	public class PropAnchor : MonoBehaviour, IPropComponent {


        public GameObject Parent;

        [Header("Anchoring")]

        public Vector3 Anchor = new Vector3(0.5f, 0.5f, 0.5f);
        public Vector3 Offset = Vector3.zero;


		public bool UseRendererAnchor = true;
		public Vector3 RendererAnchor = new Vector3(0.5f, 0.5f, 0.5f);

        [Header("Size Scale")]

        public bool EnableXScale = false;
        public float XSize = 1f;


        public bool EnableYScale = false;
        public float YSize = 1f;



        public void Update() {
            Anchor.x = Mathf.Clamp(Anchor.x, 0f, 1f);
            Anchor.y = Mathf.Clamp(Anchor.y, 0f, 1f);
            Anchor.z = Mathf.Clamp(Anchor.z, 0f, 1f);


			RendererAnchor.x = Mathf.Clamp(RendererAnchor.x, 0f, 1f);
			RendererAnchor.y = Mathf.Clamp(RendererAnchor.y, 0f, 1f);
			RendererAnchor.z = Mathf.Clamp(RendererAnchor.z, 0f, 1f);

            XSize = Mathf.Clamp(XSize, 0.001f, 1f);
            YSize = Mathf.Clamp(YSize, 0.001f, 1f);


		
            if (Parent != null) {


				Bounds parentBounds = Scene.GetBounds (Parent);


				transform.position = parentBounds.center;





				float xPos = parentBounds.center.x - parentBounds.extents.x + parentBounds.size.x * Anchor.x;
				float yPos = parentBounds.center.y - parentBounds.extents.y + parentBounds.size.y * Anchor.y;
				float zPos = parentBounds.center.z - parentBounds.extents.z + parentBounds.size.z * Anchor.z;





                transform.position = new Vector3(xPos, yPos, zPos);
				Bounds bounds = Scene.GetBounds (gameObject, true);

				if(UseRendererAnchor) {

					float x = bounds.center.x - bounds.extents.x + bounds.size.x * RendererAnchor.x;
					float y = bounds.center.y - bounds.extents.y + bounds.size.y * RendererAnchor.y;
					float z = bounds.center.z - bounds.extents.z + bounds.size.z * RendererAnchor.z;


					Vector3 anchorPoint = new Vector3 (x, y, z);

					Vector3 diff = transform.position - anchorPoint;
					transform.position = transform.position + diff;


				}





                transform.localPosition = transform.localPosition + Offset;


                if (EnableXScale) {
                    var text = GetComponent<RoomfulText>();
                    if(text != null) {
						text.RectTransform.sizeDelta = new Vector2(parentBounds.size.x * XSize, text.RectTransform.sizeDelta.y);
                    }
                }

                if (EnableYScale) {
                    var text = GetComponent<RoomfulText>();
                    if (text != null) {
						text.RectTransform.sizeDelta = new Vector2(text.RectTransform.sizeDelta.x, parentBounds.size.y * YSize) ;
                    }
                }

            }

        }


		public void PrepareForUpalod() {
			
		}

		public void RemoveSilhouette() {
			
		}



       


    }
}