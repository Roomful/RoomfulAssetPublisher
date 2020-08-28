using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
#endif
using RF.AssetBundles.Serialization;

namespace RF.AssetWizzard.Editor {
    public class AnimatorCollector : BaseCollector {
        public override void Run(IAsset asset) {
            SerializedAnimatorController[] animators = asset.gameObject.GetComponentsInChildren<SerializedAnimatorController>(true);

            foreach (SerializedAnimatorController sac in animators) {
                AssetDatabase.SaveAnimatorController(asset, sac);
                Animator a = sac.GetComponent<Animator>();
                AnimatorController controller = AssetDatabase.LoadAsset<AnimatorController>(asset, sac.ControllerName);
                
                if (sac.HasAvatar()) {
                    Avatar avatar = AssetDatabase.LoadAsset<Avatar>(asset, asset.GetTemplate().Title);
                    if (avatar != null) {
                        a.avatar = avatar;
                    }
                }
                else if (sac.SerializedClips != null) {
                    foreach (SerializedAnimationClip ac in sac.SerializedClips) {
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
                GameObject.DestroyImmediate(sac);
            }
        }
    }
}