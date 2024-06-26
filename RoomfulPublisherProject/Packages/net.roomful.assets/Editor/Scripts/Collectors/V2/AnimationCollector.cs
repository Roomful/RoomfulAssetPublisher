using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace net.roomful.assets.editor
{
    internal class AnimationCollector : BaseCollector
    {
        public override void Run(IAssetBundle asset) {
            var anims = asset.gameObject.GetComponentsInChildren<Animation>(true);

            foreach (var a in anims) {
                var defaultAnimName = string.Empty;
                var animationsNames = new List<string>();

                if (a.clip != null) {
                    var clip = a.clip;
                    defaultAnimName = clip.name;
                    AssetDatabase.SaveAsset(asset, clip);
                }

                var animations = AnimationUtility.GetAnimationClips(a.gameObject);

                foreach (var clip in animations) {
                    animationsNames.Add(clip.name);
                    AssetDatabase.SaveAsset(asset, clip);
                }

                a.clip = null;
                foreach (var acName in animationsNames) {
                    a.RemoveClip(acName);
                }

                if (!string.IsNullOrEmpty(defaultAnimName)) {
                    a.clip = AssetDatabase.LoadAsset<AnimationClip>(asset, defaultAnimName);
                }

                foreach (var acName in animationsNames) {
                    a.AddClip(AssetDatabase.LoadAsset<AnimationClip>(asset, acName), acName);
                }
            }

            //if (mainAnimator != null) {
            //    Debug.Log("Parameters:");
            //    for (int i = 0; i < mainAnimator.parameterCount; i++) {
            //        string n = mainAnimator.GetParameter(i).name;
            //        string t = mainAnimator.GetParameter(i).type.ToString();

            //        string log = "Parameter: " + n + ", type: " + t + ", default: ";

            //        switch (mainAnimator.GetParameter(i).type) {
            //            case AnimatorControllerParameterType.Bool:
            //                log += mainAnimator.GetParameter(i).defaultBool.ToString();
            //                break;
            //            case AnimatorControllerParameterType.Trigger:
            //                log += "Trigger has no init value";
            //                break;
            //            case AnimatorControllerParameterType.Int:
            //                log += mainAnimator.GetParameter(i).defaultInt.ToString();

            //                break;
            //            case AnimatorControllerParameterType.Float:
            //                log += mainAnimator.GetParameter(i).defaultFloat.ToString();
            //                break;
            //        }

            //        Debug.Log(log);
            //    }

            //    Debug.Log("Transitions:");

            //    for (int i = 0; i < mainAnimator.layerCount; i++) {

            //    }
            //}
        }
    }
}