using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard
{

    [ExecuteInEditMode]
    public class EnvironmentAsset : Asset<EnvironmentTemplate>
    {


        public void SetTemplate(EnvironmentTemplate tpl) {
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
        public void PrepareForUpload() {

            CleanUpSilhouette();
            PrepareCoponentsForUpload();
        }


        //--------------------------------------
        // Get / Set
        //--------------------------------------





        //--------------------------------------
        // Private Methods
        //--------------------------------------


        protected override void CheckhHierarchy() {

            base.CheckhHierarchy();


            List<Transform> UndefinedObjects = new List<Transform>();
            Transform[] allObjects = FindObjectsOfType<Transform>();
            foreach (Transform child in allObjects) {
                if (child == transform) {
                    continue;
                }

                if (child.parent != null) {
                    continue;
                }

                UndefinedObjects.Add(child);
            }

            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            transform.position = Vector3.zero;
        }


    }
}