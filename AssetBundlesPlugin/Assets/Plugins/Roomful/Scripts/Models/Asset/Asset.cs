using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RF.AssetBundles.Serialization;

namespace RF.AssetWizzard
{
    public abstract class Asset<T> : MonoBehaviour, IAsset where T : Template, new()
    {

        [SerializeField]
        [HideInInspector]
        protected T _Template;

        public Texture2D Icon;
        private bool m_drawGizmos = true;

        //--------------------------------------
        // Abstract Methods
        //--------------------------------------

        public abstract void PrepareForUpload();

        //--------------------------------------
        // Public Methods
        //--------------------------------------

        public Template GetTemplate() {
            return Template;
        }

        public Texture2D GetIcon() {
            return Icon;
        }


        //--------------------------------------
        // Protected Methods
        //--------------------------------------

        protected virtual void CheckhHierarchy() {
            if (Icon == null) {
                Icon = Template.Icon.Thumbnail;
            }
        }

        protected void CleanUpSilhouette() {

            foreach (var c in Components) {
                c.RemoveSilhouette();
            }
        }

        protected virtual void PrepareCoponentsForUpload() {

            foreach (var c in Components) {
                c.PrepareForUpalod();
            }


            Animator[] animators = transform.GetComponentsInChildren<Animator>();
            for (int i = animators.Length - 1; i >= 0; i--) {
                if (animators[i].runtimeAnimatorController == null) {
                    DestroyImmediate(animators[i]);
                } else {
                    #if UNITY_EDITOR
                    SerializedAnimatorController sac = animators[i].gameObject.AddComponent<SerializedAnimatorController>();
                    sac.Serialize(animators[i].runtimeAnimatorController as UnityEditor.Animations.AnimatorController);

                    #endif
                }
            }

          
            Renderer[] renderers = transform.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers) {
                try {
                    if (renderer != null) {
                        foreach (Material mat in renderer.sharedMaterials) {
                            if (mat != null) {
                                var md = renderer.gameObject.AddComponent<SerializedMaterial>();
                                md.Serialize(mat);
                            }
                        }
                        renderer.sharedMaterials = new Material[0];
                    }
                } catch (System.Exception ex) {
                    Debug.LogError("Failed to Serialize Material", renderer.gameObject);
                    Debug.LogError(ex.StackTrace);
                    throw (new System.Exception("Serialisation Failed"));
                }
            }
           

            Projector[] projectors = transform.GetComponentsInChildren<Projector>();
            foreach (Projector p in projectors) {
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
        }

        //--------------------------------------
        // Get / Set
        //--------------------------------------

        public T Template {
            get {
                if (_Template == null) {
                    _Template = new T();
                }

                return _Template;
            }
        }

        public bool DrawGizmos {
            get {
                return m_drawGizmos;
            }
 
            set {
                m_drawGizmos = value;
            }
        }

        public IPropComponent[] Components {
            get {
                return GetComponentsInChildren<IPropComponent>();
            }
        }


        public Component Component {
            get {
                return this;
            }
        }

        public bool IsEmpty {
            get {
                Renderer[] renderers = GetComponentsInChildren<Renderer>();
                return renderers.Length == 0;
            }
        }

        public GameObject Environment {
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