using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.assets.serialization
{
    public class SerializedAnimatorController : MonoBehaviour, IRecreatableOnLoad
    {
        public string ControllerName;

        [HideInInspector] public byte[] SerializedData;
        [HideInInspector] public SerializedAvatar SerializedAvatar;

        [HideInInspector] public SerializedAnimationClip[] SerializedClips;
        [HideInInspector] public SerializedMotionData[] SerializedMotions;

        private void Awake() {
            // var animator = GetComponent<Animator>();
            // if (animator != null)
            //    animator.cullingMode = AnimatorCullingMode.CullCompletely;
        }

#if UNITY_EDITOR
        public void Serialize(UnityEditor.Animations.AnimatorController controller, Avatar avatar) {
            UnityEditor.AssetDatabase.SaveAssets();
            ControllerName = controller.name;
            var path = UnityEditor.AssetDatabase.GetAssetPath(controller);
            SerializedData = System.IO.File.ReadAllBytes(path);
            var clipList = new List<SerializedAnimationClip>();
            if (avatar != null) {
                SerializedAvatar = SerializeAvatar(avatar);
            }
            
            foreach (var ac in controller.animationClips) {
                clipList.Add(SerializeAnimationClip(ac));
            }

            var motions = new List<SerializedMotionData>();
            foreach (var lay in controller.layers) {
                foreach (var sm in lay.stateMachine.states) {
                    if (sm.state.motion != null) {
                        var animState = sm.state.motion;
                        if (animState != null) {
                            if (animState is AnimationClip) {
                                motions.Add(new SerializedMotionData() {
                                    Layer = lay.name,
                                    State = sm.state.name,
                                    AnimationName = animState.name
                                });
                            }
                            else if (animState is UnityEditor.Animations.BlendTree blendTree) {
                                var blendTreeChildren = blendTree.children;
                                for (int i = 0; i < blendTreeChildren.Length; i++) {
                                    motions.Add(new SerializedMotionData() {
                                        Layer = lay.name,
                                        State = sm.state.name,
                                        AnimationName = blendTreeChildren[i].motion.name,
                                        IsInsideBlendTree = true,
                                        BlendTreeName = blendTree.name,
                                        BlendTreeChildIndex = i,
                                        BlendTreeChildPosition = blendTreeChildren[i].position,
                                        BlendTreeChildMirrored = blendTreeChildren[i].mirror,
                                        BlendTreeChildTimeScale = blendTreeChildren[i].timeScale,
                                    });
                                }
                            }
                        }
                    }
                }
            }

            SerializedClips = clipList.ToArray();
            SerializedMotions = motions.ToArray();
        }

        public bool HasAvatar() {
            return SerializedAvatar != null && SerializedAvatar.AvatarData.Length > 0;
        }

        private static SerializedAnimationClip SerializeAnimationClip(AnimationClip ac) {
            var sac = new SerializedAnimationClip();
            sac.AnimationClipName = ac.name;

            var animClip = ScriptableObject.Instantiate(ac);
            var tempPath = "Assets/Plugins/Roomful/Editor/" + ac.name + ".anim";
            UnityEditor.AssetDatabase.CreateAsset(animClip, tempPath);

            sac.ClipData = System.IO.File.ReadAllBytes(tempPath);
            System.IO.File.Delete(tempPath);
            return sac;
        }

        private SerializedAvatar SerializeAvatar(Avatar avatar) {
            var avatarPath = UnityEditor.AssetDatabase.GetAssetPath(avatar);
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
