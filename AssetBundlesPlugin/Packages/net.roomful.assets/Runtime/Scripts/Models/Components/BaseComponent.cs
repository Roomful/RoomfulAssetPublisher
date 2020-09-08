using UnityEngine;


namespace net.roomful.assets
{

    public class BaseComponent : MonoBehaviour
    {


        public void RemoveSilhouette() {
            DestroyImmediate(Silhouette.gameObject);
        }


        public Transform Silhouette {
            get {
                var silhouette = Prop.GetLayer(HierarchyLayers.Silhouette).Find(gameObject.GetInstanceID().ToString());
                if (silhouette == null) {
                    silhouette = new GameObject(gameObject.GetInstanceID().ToString()).transform;
                    silhouette.parent = Prop.GetLayer(HierarchyLayers.Silhouette);
                }

                silhouette.position = transform.position;

                silhouette.localRotation = transform.rotation;
                silhouette.localScale = transform.lossyScale;

                return silhouette;
            }
        }

        public PropAsset Prop => FindObjectOfType<PropAsset>();
    }
}