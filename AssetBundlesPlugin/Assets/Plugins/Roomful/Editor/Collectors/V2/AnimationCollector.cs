using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using RF.AssetBundles.Serialization;
namespace RF.AssetWizzard  { 
    public class AnimationCollector : ICollector {
        
        public void Run(RF.AssetWizzard.PropAsset propAsset) {
            Animation[] anims = propAsset.GetComponentsInChildren<Animation>();

            foreach (Animation a in anims) {
                string defaultAnimName = string.Empty;
                List<string> animationsNames = new List<string>();

                if (a.clip != null) {
                    defaultAnimName = a.clip.name;
                    PropDataBase.SaveAsset<AnimationClip>(propAsset, a.clip);
                }

                AnimationClip[] animations = AnimationUtility.GetAnimationClips(a.gameObject);

                foreach (AnimationClip clip in animations) {
                    animationsNames.Add(clip.name);
                    PropDataBase.SaveAsset<AnimationClip>(propAsset, clip);
                }
                
                a.clip = null;
                foreach (string acName in animationsNames) {
                    a.RemoveClip(acName);
                }

                if (!string.IsNullOrEmpty(defaultAnimName)) {
                    a.clip = PropDataBase.LoadAsset<AnimationClip>(propAsset, defaultAnimName);
                }

                foreach (string acName in animationsNames) {
                    a.AddClip(PropDataBase.LoadAsset<AnimationClip>(propAsset, acName), acName);
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
