using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
#endif

using RF.AssetBundles.Serialization;
namespace RF.AssetWizzard {
    public class AnimatorCollector : ICollector {
        
        public void Run(RF.AssetWizzard.PropAsset propAsset) {
            
            SerializedAnimatorController[] animators = propAsset.GetComponentsInChildren<SerializedAnimatorController>(true);
            
            foreach (SerializedAnimatorController sac in animators) {
                PropDataBase.SaveAnimatorController(propAsset, sac);
                
                if (sac.SerializedClips != null) {
                    foreach (SerializedAnimationClip ac in sac.SerializedClips) {
                        PropDataBase.SaveAnimationClipByData(propAsset, ac);
                    }
                }
                
                AnimatorController control = PropDataBase.LoadAsset<AnimatorController>(propAsset, sac.ControllerName);

                foreach (var lay in control.layers) {
                    foreach (var sm in lay.stateMachine.states) {
                        if (sm.state.motion != null) {
                            AnimationClip ac = sm.state.motion as AnimationClip;

                            if (PropDataBase.IsAssetExist<AnimationClip>(propAsset, ac)) {
                                sm.state.motion = PropDataBase.LoadAsset<AnimationClip>(propAsset, ac.name);
                            }

                        }
                    }
                }

                Animator a = sac.GetComponent<Animator>();
                a.runtimeAnimatorController = PropDataBase.LoadAsset<AnimatorController>(propAsset, sac.ControllerName);
                
                GameObject.DestroyImmediate(sac);
            }
        }
    }
}