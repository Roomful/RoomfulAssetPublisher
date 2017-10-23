using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetBundles.Serialization {
    public class SerializedAnimatorController : MonoBehaviour {

        public string ControllerName;
        public byte[] SerializedData;

        public AnimationClip[] AnimationsClips;
#if UNITY_EDITOR
        public void Serialize(UnityEditor.Animations.AnimatorController controller) {
            
            ControllerName = controller.name;
            string path = UnityEditor.AssetDatabase.GetAssetPath(controller);
            SerializedData = System.IO.File.ReadAllBytes(path);
            
            AnimationsClips = controller.animationClips;
        }
#endif
    }
}
