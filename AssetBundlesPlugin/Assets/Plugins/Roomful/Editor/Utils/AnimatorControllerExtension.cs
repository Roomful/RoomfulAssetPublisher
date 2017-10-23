using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace RF.Utils {
    public static class AnimatorControllerExtension {

        private static Type realType;
        private static MethodInfo method_GetEffectiveAnimatorController;
        private static FieldInfo field_OnAnimatorControllerDirty;

        public static void InitType() {
            if (realType == null) {
                realType = typeof(AnimatorController);

                method_GetEffectiveAnimatorController = realType.GetMethod("GetEffectiveAnimatorController", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                field_OnAnimatorControllerDirty = realType.GetField("OnAnimatorControllerDirty", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            }
        }

        public static AnimatorController GetEffectiveAnimatorController(Animator animator) {
            InitType();
            object val = (AnimatorController)(method_GetEffectiveAnimatorController.Invoke(null, new object[] { animator }));
            return (AnimatorController)val;
        }

        public static void AppendOnAnimatorControllerDirtyCallback(this AnimatorController controller, System.Action callback) {
            InitType();
            System.Action oldCallback = (System.Action)field_OnAnimatorControllerDirty.GetValue(controller);
            System.Action newCallback = (System.Action)Delegate.Combine(oldCallback, new System.Action(callback));

            field_OnAnimatorControllerDirty.SetValue(controller, newCallback);
        }

        public static void RemoveOnAnimatorControllerDirtyCallback(this AnimatorController controller, System.Action callback) {
            InitType();
            System.Action oldCallback = (System.Action)field_OnAnimatorControllerDirty.GetValue(controller);
            System.Action newCallback = (System.Action)Delegate.Remove(oldCallback, new System.Action(callback));

            field_OnAnimatorControllerDirty.SetValue(controller, newCallback);
        }
    }
}