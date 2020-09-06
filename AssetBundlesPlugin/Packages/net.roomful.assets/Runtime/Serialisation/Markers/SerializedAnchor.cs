using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using net.roomful.assets;

namespace net.roomful.assets.serialization
{

    public class SerializedAnchor : MonoBehaviour, IRecreatableOnLoad
    {

        public GameObject Parent;

        [Header("Anchoring")]

        public Vector3 Anchor = new Vector3(0.5f, 0.5f, 0.5f);
        public Vector3 Offset = Vector3.zero;


        public bool UseRendererPivot = true;
        public Vector3 RendererPivot = new Vector3(0.5f, 0.5f, 0.5f);

        [Header("Size Scale")]

        public bool EnableXScale = false;
        public float XSize = 1f;


        public bool EnableYScale = false;
        public float YSize = 1f;


        /// <summary>
        /// In case we are targeting PropThumbnail
        /// We will target it's Canvas instead
        /// In future we might want to have a setting for that
        /// </summary>
        public GameObject SmartParent {
            get {
                PropThumbnail thumbnail = Parent.GetComponent<PropThumbnail>();
                if(thumbnail != null) {
                    return thumbnail.Canvas.gameObject;
                }

                return Parent;
               
            }
        }

    }
}