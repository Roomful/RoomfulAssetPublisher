using net.roomful.assets;
using net.roomful.assets.serialization;
using UnityEngine;

namespace Scripts.Extensions
{
    public static class SerializedAnchorExtension
    {
        
        /// <summary>
        /// In case we are targeting PropThumbnail
        /// We will target it's Canvas instead
        /// In future we might want to have a setting for that
        /// </summary>
        public static GameObject GetSmartParent(this SerializedAnchor anchor) {
            var thumbnail = anchor.Parent.GetComponent<PropThumbnail>();
            if(thumbnail != null) {
                return thumbnail.Canvas.gameObject;
            }

            return anchor.Parent;
        } 
    }
}