using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetBundles.Serialization {
    public class SerializedAnimatorController : MonoBehaviour {

        public string ControllerName;
        public byte[] SerializedData;

        public SerializedAnimationClip[] SerializedClips;
#if UNITY_EDITOR
        public void Serialize(UnityEditor.Animations.AnimatorController controller) {
            
            ControllerName = controller.name;

            List<SerializedAnimationClip> clipList = new List<SerializedAnimationClip>();
            
            foreach (AnimationClip ac in controller.animationClips) {
                SerializedAnimationClip sac = new SerializedAnimationClip();
                sac.AnimationClipName = ac.name;

                string clipPath = UnityEditor.AssetDatabase.GetAssetPath(ac);
                sac.ClipData = System.IO.File.ReadAllBytes(clipPath);

                clipList.Add(sac);

            }

            SerializedClips = clipList.ToArray();
            
            string path = UnityEditor.AssetDatabase.GetAssetPath(controller);
            SerializedData = System.IO.File.ReadAllBytes(path);
        }
#endif
    }
}
