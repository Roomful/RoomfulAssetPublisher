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
                a.runtimeAnimatorController = controller;
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
                    foreach (var lay in controller.layers) {
                        foreach (var sm in lay.stateMachine.states) {
                            if (sm.state.motion != null) {
                                AnimationClip ac = sm.state.motion as AnimationClip;
                                if (AssetDatabase.IsAssetExist(asset, ac)) {
                                    sm.state.motion = AssetDatabase.LoadAsset<AnimationClip>(asset, ac.name);
                                }
                            }
                        }
                    }
                }

                

                GameObject.DestroyImmediate(sac);
            }
        }
    }
}