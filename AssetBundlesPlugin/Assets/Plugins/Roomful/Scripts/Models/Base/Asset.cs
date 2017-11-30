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
                c.PrepareForUpalod();
            }
        }

        protected void PrepareCoponentsForUpload() {

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

            Renderer[] renderers = transform.GetComponentsInChildren<Renderer>();// GetAllRenderers(transform.gameObject);
            foreach (Renderer renderer in renderers) {
                if (renderer != null) {

                    foreach (Material mat in renderer.sharedMaterials) {
                        if (mat != null) {
                            var md = renderer.gameObject.AddComponent<SerializedMaterial>();
                            md.Serialize(mat);
                        }
                    }

                    renderer.sharedMaterials = new Material[0];
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

    }
}