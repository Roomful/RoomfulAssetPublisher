﻿using System.Collections.Generic;
using UnityEngine;
using net.roomful.assets.serialization;

namespace net.roomful.assets
{
    abstract class Asset<T> : MonoBehaviour, IAsset where T : AssetTemplate, new()
    {
        protected T m_Template;

        public Texture2D Icon;

        public string Title => GetTemplate().Title;

        
        //--------------------------------------
        // Abstract Methods
        //--------------------------------------

        public abstract void PrepareForUpload();

        //--------------------------------------
        // Public Methods
        //--------------------------------------

        public AssetTemplate GetTemplate() {
            return Template;
        }

        public Texture2D GetIcon() {
            return Icon;
        }


        //--------------------------------------
        // Protected Methods
        //--------------------------------------

        protected virtual void CheckHierarchy() {
            if (Icon == null) {
                Icon = Template.Icon.Thumbnail;
            }
        }

        protected virtual void PrepareComponentsForUpload() {

            foreach (var c in Components) {
                c.PrepareForUpload();
            }


            var animators = transform.GetComponentsInChildren<Animator>(true);
            for (var i = animators.Length - 1; i >= 0; i--) {
                if (animators[i].runtimeAnimatorController == null) {
                    DestroyImmediate(animators[i]);
                } else {
                    #if UNITY_EDITOR
                    var avatar = animators[i].avatar;
                    var runtimeController = animators[i].runtimeAnimatorController;
                    var sac = animators[i].gameObject.AddComponent<SerializedAnimatorController>();
                    sac.Serialize(runtimeController as UnityEditor.Animations.AnimatorController, avatar);
                    
                    #endif
                }
            }

          
            var renderers = transform.GetComponentsInChildren<Renderer>(true);
            foreach (var rnd in renderers) {
              //  try {
                    if (rnd != null) {
                        foreach (var mat in rnd.sharedMaterials) {
                            if (mat != null) {
                                var md = rnd.gameObject.AddComponent<SerializedMaterial>();
                                md.Serialize(mat);
                            }
                        }
                        rnd.sharedMaterials = new Material[0];
                    }
            //    } catch (System.Exception ex) {
              //      Debug.LogError("Failed to Serialize Material", rnd.gameObject);
             //       Debug.LogError(ex.StackTrace);
         //           throw (new System.Exception("Serialisation Failed"));
            //    }
            }
           

            var projectors = transform.GetComponentsInChildren<Projector>(true);
            foreach (var p in projectors) {
                try {
                    if (p.material != null) {
                        var md = p.gameObject.AddComponent<SerializedMaterial>();
                        md.Serialize(p.material);
                    }
                    p.material = null;

                } catch (System.Exception ex) {
                    Debug.LogError("Failed to Serialize Projector", p.gameObject);
                    Debug.LogError(ex.StackTrace);
                    throw (new System.Exception("Serialisation Failed"));
                }
            }
            
            var userClickMarkers = transform.GetComponentsInChildren<UserClickMarker>(true);
            
            for (var i = userClickMarkers.Length - 1; i >= 0; i--)
            {
                foreach (Transform child in userClickMarkers[i].transform)
                {
                    DestroyImmediate(child.gameObject);
                }
                DestroyImmediate(userClickMarkers[i]);
            }
        }

        //--------------------------------------
        // Get / Set
        //--------------------------------------

        public T Template => m_Template ?? (m_Template = new T());

        public bool DrawGizmos { get; set; } = true;

        IEnumerable<IPropPublihserComponent> Components => GetComponentsInChildren<IPropPublihserComponent>();

        public Component Component => this;

        public bool IsEmpty {
            get {
                var renderers = GetComponentsInChildren<Renderer>();
                return renderers.Length == 0;
            }
        }

        public virtual GameObject Environment {
            get {

                var rig = GameObject.Find("Environment");
                if (rig == null) {
                    rig = PrefabManager.CreatePrefab("Environment");
                }

                rig.transform.SetSiblingIndex(0);

                return rig;
            }
        }
    }
}