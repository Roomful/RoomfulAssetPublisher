using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard
{

    [ExecuteInEditMode]
    public class EnviromentAsset : MonoBehaviour
    {

        [SerializeField]
        private EnviromentTemplate _Template;
        public Texture2D Icon;


        public void SetTemplate(EnviromentTemplate tpl) {
            _Template = tpl;
        }


        //--------------------------------------
        // Unity Editor
        //--------------------------------------


        public void Update() {
            CheckhHierarchy();
        }


        //--------------------------------------
        // Get / Set
        //--------------------------------------

        public EnviromentTemplate Template {
            get {
                if (_Template == null) {
                    _Template = new EnviromentTemplate();
                }

                return _Template;
            }
        }

        //--------------------------------------
        // Private Methods
        //--------------------------------------


        private void CheckhHierarchy() {


            if (Icon == null) {
                Icon = Template.Icon.Thumbnail;
            }



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