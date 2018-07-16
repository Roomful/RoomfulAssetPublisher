using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetBundles.Serialization {
    public class SerializedAnimatorController : MonoBehaviour {
        public string ControllerName;

        [HideInInspector] public byte[] SerializedData;
        [HideInInspector] public SerializedAvatar SerializedAvatar;

        [HideInInspector] public SerializedAnimationClip[] SerializedClips;
#if UNITY_EDITOR
        public void Serialize(UnityEditor.Animations.AnimatorController controller, Avatar avatar) {
            ControllerName = controller.name;
            string path = UnityEditor.AssetDatabase.GetAssetPath(controller);
            SerializedData = System.IO.File.ReadAllBytes(path);
            
            List<SerializedAnimationClip> clipList = new List<SerializedAnimationClip>();
            if (avatar != null) {
                SerializedAvatar = SerializeAvatar(avatar);
            }
            else {
                foreach (AnimationClip ac in controller.animationClips) {
                    clipList.Add(SerializeAnimationClip(ac));
                }
            }
            SerializedClips = clipList.ToArray();
            
        }

        private static SerializedAnimationClip SerializeAnimationClip(AnimationClip ac) {
            SerializedAnimationClip sac = new SerializedAnimationClip();
            sac.AnimationClipName = ac.name;

            string clipPath = UnityEditor.AssetDatabase.GetAssetPath(ac);
            sac.ClipData = System.IO.File.ReadAllBytes(clipPath);
            return sac;
        }

        private SerializedAvatar SerializeAvatar(Avatar avatar) {
            var avatarData = new SerializedAvatar();
            avatarData.AvatarName = avatar.name;
            string avatarPath = UnityEditor.AssetDatabase.GetAssetPath(avatar);
            avatarData.AvatarData = System.IO.File.ReadAllBytes(avatarPath);
            return avatarData;
        }

#endif
    }
}