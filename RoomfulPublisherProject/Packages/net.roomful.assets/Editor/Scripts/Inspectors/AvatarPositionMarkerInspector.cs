using System.Collections.Generic;
using net.roomful.api;
using net.roomful.api.avatars;
using net.roomful.assets;
using net.roomful.assets.editor;
using net.roomful.assets.serialization;
using UnityEditor;
using UnityEngine;

namespace RF.AssetBundles.Serialization
{
    enum AvatarGender
    {
        Male = 0,
        Female = 1
    }
    
    [CustomEditor(typeof(AvatarPositionMarker), true)]
    [CanEditMultipleObjects]
    public class AvatarPositionMarkerInspector : Editor
    {
        Animator m_Animator;
        double m_LastTimestamp;

        bool m_ShowAvatar = true;
        AvatarGender m_AvatarGender;
        
        public override void OnInspectorGUI() {
            if (targets.Length > 1) {
                return;
            }
            
            EditorGUI.BeginChangeCheck();
            DrawDefaultInspector();
            GUILayout.Space(15f);
            m_ShowAvatar = GUILayout.Toggle(m_ShowAvatar, "Show Avatar");
            m_AvatarGender = (AvatarGender) EditorGUILayout.EnumPopup("Avatar Gender", m_AvatarGender);
            if (EditorGUI.EndChangeCheck() && target is AvatarPositionMarker marker) {
                DestroyHelper(marker);
                if(m_ShowAvatar)
                    CreateHelper(marker);
            }
        }

        void OnEnable() {
            foreach (var t in targets) {
                if (!(t is AvatarPositionMarker marker)) {
                    continue;
                }
                
                if(m_ShowAvatar)
                    CreateHelper(marker);
            }
        }

        void OnDisable() {
            foreach (var t in targets) {
                if (!(t is AvatarPositionMarker marker)) {
                    continue;
                }
                DestroyHelper(marker);
            }
        }

        void CreateHelper(AvatarPositionMarker marker) {
            switch (marker.PositionType)
            {
                case AvatarPositionType.Sitting:
                case AvatarPositionType.Standing:
                    var go = PrefabManager.CreatePrefab(marker.PositionType == AvatarPositionType.Sitting ? "Avatar/Avatar_Sitting" : "Avatar/Avatar_Standing");
                    go.AddComponent<AvatarPositionMarkerHelper>();
                    var t = go.transform;
                    t.SetParent(marker.transform, false);
                    t.localPosition = Vector3.zero;
                    var parentScale = marker.transform.lossyScale;
                    t.localScale = new Vector3(1.0f / parentScale.x, 1.0f / parentScale.y, 1.0f / parentScale.z);
                    t.localRotation = Quaternion.identity;

                    if (marker.PositionType != AvatarPositionType.Sitting) {
                        return;
                    }
                    
                    var pelvis = FindBoneWithName(t, "Pelvis");
                    if (pelvis == null) {
                        return;
                    }
                    t.localPosition = marker.transform.position - pelvis.position;
                    break;
                
                case AvatarPositionType.SourcedFromProp:
                    var poseData = marker.gameObject.GetComponent<AvatarPoseData>();

                    AvatarAnimationData animationData = null;
                    string avatarPath = null;
                    
                    if (poseData != null)
                    {
                        switch (m_AvatarGender)
                        {
                            case AvatarGender.Female:
                                animationData = poseData.FemaleAnimationClip;
                                avatarPath = "Avatar/RPM_Avatar_Female";
                                break;
                        
                            case AvatarGender.Male:
                                animationData = poseData.MaleAnimationClip;
                                avatarPath = "Avatar/RPM_Avatar_Male";
                                break;
                        }
                    }
                    
                    if (animationData?.AnimationClip != null)
                    {
                        var avatar = PrefabManager.CreatePrefab(avatarPath);
                        avatar.AddComponent<AvatarPositionMarkerHelper>();
                        avatar.transform.SetParent(marker.transform, false);
                        var parentScaleB = marker.transform.lossyScale;
                        avatar.transform.localScale = new Vector3(1.0f / parentScaleB.x, 1.0f / parentScaleB.y, 1.0f / parentScaleB.z);
                        avatar.transform.localRotation = Quaternion.identity;

                        var position = marker.Position;
                        var positionOffset = animationData.PositionOffset;
                        var markerForward = marker.Forward.TerminateYAxis();
                        position += markerForward * -positionOffset.x;
                        position += poseData.transform.up * positionOffset.y;
                        avatar.transform.position = position;

                        m_Animator = avatar.GetComponent<Animator>();
                        var runtimeAnimatorOverride = m_Animator.runtimeAnimatorController as AnimatorOverrideController;
                        var overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>(runtimeAnimatorOverride.overridesCount);
                        runtimeAnimatorOverride.GetOverrides(overrides);
                        overrides[0] = new KeyValuePair<AnimationClip, AnimationClip>(overrides[0].Key, animationData.AnimationClip);
                        runtimeAnimatorOverride.ApplyOverrides(overrides);

                        EditorApplication.update += EditorUpdate;
                        m_LastTimestamp = EditorApplication.timeSinceStartup;
                    }
                    break;
            }
        }

        void DestroyHelper(AvatarPositionMarker marker)
        {
            if (m_Animator != null)
            {
                EditorApplication.update -= EditorUpdate;
                m_Animator = null;
            }

            if (marker != null)
            {
                foreach (var helper in marker.GetComponentsInChildren<AvatarPositionMarkerHelper>()) {
                    DestroyImmediate(helper.gameObject);
                }
            }
        }

        static Transform FindBoneWithName(Transform parent, string name) {
            for (var i = 0; i < parent.childCount; i++) {
                var child = parent.GetChild(i);
                if (child.name.Equals(name)) {
                    return child;
                }

                child = FindBoneWithName(child, name);
                if (child != null) {
                    return child;
                }
            }

            return null;
        }
        
        void EditorUpdate()
        {
            if (m_Animator != null)
            {
                var currentTimestamp = EditorApplication.timeSinceStartup;
                var deltaTime = (float) (currentTimestamp - m_LastTimestamp);
                m_Animator.Update(deltaTime);
                m_LastTimestamp = currentTimestamp;
            }
        }
    }

}
