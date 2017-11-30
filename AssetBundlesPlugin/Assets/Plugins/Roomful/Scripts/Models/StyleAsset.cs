using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RF.AssetBundles.Serialization;

namespace RF.AssetWizzard
{

    [ExecuteInEditMode]
    public class StyleAsset : Asset<StyleTemplate>
    {


        //--------------------------------------
        // Initialization
        //--------------------------------------

        public void Start() {
          
        }

        public void SetTemplate(StyleTemplate tpl) {
            _Template = tpl;
        }


        //--------------------------------------
        // Unity Editor
        //--------------------------------------


        public void Update() {
            CheckhHierarchy();
        }


        //--------------------------------------
        // Public Methods
        //--------------------------------------

        [ContextMenu("Prepare For Upload")]
        public override void PrepareForUpload() {

            CleanUpSilhouette();
            PrepareCoponentsForUpload();
        }





        //--------------------------------------
        // Get / Set
        //--------------------------------------


        public SerializedStyle Settings {
            get {

                var settings = GetComponent<SerializedStyle>();
                if (settings == null) {
                    settings = gameObject.AddComponent<SerializedStyle>();
                }

                settings.hideFlags = HideFlags.HideInInspector;

                return settings;
            }
        }



        //--------------------------------------
        // Private Methods
        //--------------------------------------


        protected override void CheckhHierarchy() {

            base.CheckhHierarchy();

            transform.Reset();

          

        }


    }
}