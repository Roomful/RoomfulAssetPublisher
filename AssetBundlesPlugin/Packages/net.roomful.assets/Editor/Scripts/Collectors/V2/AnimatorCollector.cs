using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Animations;
#endif
using net.roomful.assets.serialization;

namespace net.roomful.assets.Editor
{
    public class AnimatorCollector : BaseCollector
    {
        public override void Run(IAsset asset) {
            var animators = asset.gameObject.GetComponentsInChildren<SerializedAnimatorController>(true);

            foreach (var sac in animators) {
                AssetDatabase.SaveAnimatorController(asset, sac);
                var a = sac.GetComponent<Animator>();
                var controller = AssetDatabase.LoadAsset<AnimatorController>(asset, sac.ControllerName);

                if (sac.HasAvatar()) {
                    var avatar = AssetDatabase.LoadAsset<Avatar>(asset, asset.GetTemplate().Title);
                    if (avatar != null) {
                        a.avatar = avatar;
                    }
                }
                else if (sac.SerializedClips != null) {
                    foreach (var ac in sac.SerializedClips) {
                        AssetDatabase.SaveAnimationClipByData(asset, ac);
                    }

                    if (sac.SerializedMotions != null) {
                        foreach (var layer in controller.layers) {
                            foreach (var sm in layer.stateMachine.states) {
                                foreach (var motionData in sac.SerializedMotions) {
                                    if (motionData.Layer.Equals(layer.name) && motionData.State.Equals(sm.state.name)) {
                                        var animation = AssetDatabase.LoadAsset<AnimationClip>(asset, motionData.AnimationName);
                                        if (animation == null) {
                                            Debug.LogError("Cannot find animation for motion " + motionData.AnimationName);
                                        }
                                        else {
                                            sm.state.motion = animation;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                a.runtimeAnimatorController = controller;
                Object.DestroyImmediate(sac);
            }
        }
    }
}