﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetBundles.Serialization {
    public class SerializedAnimatorController : MonoBehaviour {
        public string ControllerName;

        [HideInInspector] public byte[] SerializedData;
        [HideInInspector] public SerializedAvatar SerializedAvatar;

        [HideInInspector] public SerializedAnimationClip[] SerializedClips;
        [HideInInspector] public SerializedMotionData[] SerializedMotions;
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
            

            var motions = new List<SerializedMotionData>();
            foreach (var lay in controller.layers) {
                foreach (var sm in lay.stateMachine.states) {
                    if (sm.state.motion != null) {
                        AnimationClip ac = sm.state.motion as AnimationClip;
                        motions.Add(new SerializedMotionData() {
                            Layer = lay.name,
                            State = sm.state.name,
                            AnimationName = ac.name
                        });
                    }
                }
            }
            SerializedMotions = motions.ToArray();

        }

        public bool HasAvatar() {
            return SerializedAvatar != null && SerializedAvatar.AvatarData.Length > 0;
        }

        private static SerializedAnimationClip SerializeAnimationClip(AnimationClip ac) {
            SerializedAnimationClip sac = new SerializedAnimationClip();
            sac.AnimationClipName = ac.name;

            string clipPath = UnityEditor.AssetDatabase.GetAssetPath(ac);
            sac.ClipData = System.IO.File.ReadAllBytes(clipPath);
            return sac;
        }

        private SerializedAvatar SerializeAvatar(Avatar avatar) {
            string avatarPath = UnityEditor.AssetDatabase.GetAssetPath(avatar);
            if (System.IO.File.Exists(avatarPath)) {
                var avatarData = new SerializedAvatar();
                avatarData.AvatarName = avatar.name;
                avatarData.AvatarData = System.IO.File.ReadAllBytes(avatarPath);
                return avatarData;
            }

            return null;
        }

#endif
    }
}