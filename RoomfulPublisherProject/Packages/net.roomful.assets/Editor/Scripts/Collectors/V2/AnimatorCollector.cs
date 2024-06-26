using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Animations;
#endif
using net.roomful.assets.serialization;

namespace net.roomful.assets.editor
{
    internal class AnimatorCollector : BaseCollector
    {
        public override void Run(IAssetBundle asset) {
            var animators = asset.gameObject.GetComponentsInChildren<SerializedAnimatorController>(true);

            foreach (var sac in animators) {
                AssetDatabase.SaveAnimatorController(asset, sac);
                var a = sac.GetComponent<Animator>();
                var controller = AssetDatabase.LoadAsset<AnimatorController>(asset, sac.ControllerName);
                a.runtimeAnimatorController = controller;

                if (sac.HasAvatar()) {
                    var avatar = AssetDatabase.LoadAsset<Avatar>(asset, asset.Title);
                    if (avatar != null) {
                        a.avatar = avatar;
                    }
                }
                if (sac.SerializedClips != null) {
                    foreach (var ac in sac.SerializedClips) {
                        AssetDatabase.SaveAnimationClipByData(asset, ac);
                    }
                    if (sac.SerializedMotions != null) {
                        foreach (var layer in controller.layers) {
                            foreach (var animState in layer.stateMachine.states) {
                                var animStateMotion = animState.state.motion;
                                if (animStateMotion != null) {
                                    if (animStateMotion is AnimationClip) {
                                        AssetDatabase.SaveAsset(asset, animState.state.motion as AnimationClip);
                                    }
                                    else if (animStateMotion is BlendTree blendTree) {
                                        var animsAmount = blendTree.children.Length;
                                        for (var i = animsAmount - 1; i >= 0; i--) {
                                            var child = blendTree.children[i];
                                            if (child.motion != null) {
                                                AssetDatabase.SaveAsset(asset, child.motion as AnimationClip);
                                            }
                                            // Sadly it is not possible to assign new AnimationClip to motion
                                            // inside BlendTree, so we will delete and recreate children later
                                            blendTree.RemoveChild(i);
                                        }
                                    }
                                }
                                
                                foreach (var serializedMotion in sac.SerializedMotions) {
                                    if (serializedMotion.Layer == layer.name && serializedMotion.State == animState.state.name) {
                                        var animClip = AssetDatabase.LoadAsset<AnimationClip>(asset, serializedMotion.AnimationName);
                                        if (animStateMotion is BlendTree blendTree && serializedMotion.IsInsideBlendTree) {
                                            if (serializedMotion.BlendTreeName == blendTree.name) {
                                                blendTree.AddChild(animClip, serializedMotion.BlendTreeChildPosition);
                                            }
                                        }
                                        else
                                            animState.state.motion = animClip;
                                    }
                                }
                            }
                        }
                    }
                }

                Object.DestroyImmediate(sac);
            }
        }
    }
}