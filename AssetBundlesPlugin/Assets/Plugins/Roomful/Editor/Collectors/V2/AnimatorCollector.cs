using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
#endif

using RF.AssetBundles.Serialization;
namespace RF.AssetWizzard.Editor
{
    public class AnimatorCollector : ICollector {
        
        public void Run(IAsset asset) {
            
            SerializedAnimatorController[] animators = asset.gameObject.GetComponentsInChildren<SerializedAnimatorController>(true);
            
            foreach (SerializedAnimatorController sac in animators) {
                AssetDatabase.SaveAnimatorController(asset, sac);
                
                if (sac.SerializedClips != null) {
                    foreach (SerializedAnimationClip ac in sac.SerializedClips) {
                        AssetDatabase.SaveAnimationClipByData(asset, ac);
                    }
                }
                
                AnimatorController control = AssetDatabase.LoadAsset<AnimatorController>(asset, sac.ControllerName);

                foreach (var lay in control.layers) {
                    foreach (var sm in lay.stateMachine.states) {
                        if (sm.state.motion != null) {
                            AnimationClip ac = sm.state.motion as AnimationClip;

                            if (AssetDatabase.IsAssetExist<AnimationClip>(asset, ac)) {
                                sm.state.motion = AssetDatabase.LoadAsset<AnimationClip>(asset, ac.name);
                            }

                        }
                    }
                }

                Animator a = sac.GetComponent<Animator>();
                a.runtimeAnimatorController = AssetDatabase.LoadAsset<AnimatorController>(asset, sac.ControllerName);
                
                GameObject.DestroyImmediate(sac);
            }
        }
    }
}