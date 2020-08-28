using System;
using UnityEngine;
using UnityEditor;
using Rotorz.ReorderableList;
using UnityEditor.SceneManagement;


namespace RF.AssetWizzard.Editor
{

    public abstract class AssetWizzard<A> : WizzardUIComponent, IAssetWizzard where A : IAsset {


        private A m_currentAsset;
        private GameObject m_currentAssetGameObject;

        //--------------------------------------
        // Abstract Methods
        //--------------------------------------

        public abstract void OnGUI(bool GUIState);

        public abstract void Create();
        public abstract void Upload();
        public abstract void Download();

        //--------------------------------------
        // Public Methods
        //--------------------------------------

        public void DrawTitleFiled(bool GUIState) {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Title: ", GUILayout.Width(100));
            GUI.enabled = false;
            Template.Title = EditorGUILayout.TextField(Template.Title, GUILayout.Width(240));
            GUI.enabled = GUIState;
            GUILayout.EndHorizontal();
        }

        public void DrawTags() {
            GUILayout.BeginVertical(GUILayout.Width(225));
            {
                ReorderableListGUI.Title("Asset Tags");
                try {
                    ReorderableListGUI.ListField(Template.Tags, TagListItem, DrawEmptyTag);
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                }
            }
            GUILayout.EndVertical();
        }

        public void DrawControlButtons() {

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            Rect buttonRect1 = new Rect(460, 360, 120, 18);
            Rect buttonRect2 = new Rect(310, 360, 120, 18);
            Rect buttonRect3 = new Rect(460, 390, 120, 18);
          

            if (Asset.GetTemplate().IsNew) {
                bool upload = GUI.Button(buttonRect1, "Upload");
                if (upload) {
                    Upload();
                }

            } else {
                bool upload = GUI.Button(buttonRect1, "Reupload");
                if (upload) {
                    Upload();
                }

                bool refresh = GUI.Button(buttonRect2, "Refresh");
                if (refresh) {
                    Download();
                }
            } 
           

            bool create = GUI.Button(buttonRect3, "Create New");
            if (create) {
                Create();
            }


            GUILayout.Space(40f);
            GUILayout.EndHorizontal();
        }

       

        //--------------------------------------
        // Get / Set
        //--------------------------------------


        public bool HasAsset {
            get {
                return Asset != null;
            }
        }

        protected Template Template  {
            get {
                return Asset.GetTemplate();
            }
        }


        protected A Asset {
            get {
                if(m_currentAssetGameObject != null) {
                    return m_currentAsset;
                } else {
                    return FindAsset();
                }
              
            }
        }



        //--------------------------------------
        // Private Methods
        //--------------------------------------


        private string TagListItem(Rect position, string itemValue) {
            if (itemValue == null)
                itemValue = "new_tag";
            return EditorGUI.TextField(position, itemValue);
        }

        private void DrawEmptyTag() {
            GUILayout.Label("No items in list.", EditorStyles.miniLabel);
        }



        private A FindAsset() {
            var scene = EditorSceneManager.GetActiveScene();
            foreach(GameObject gameObject in scene.GetRootGameObjects()) {
                A target = gameObject.GetComponent<A>();
                if (target != null) {
                    m_currentAsset = target;
                    m_currentAssetGameObject = m_currentAsset.gameObject;
                    return target;
                }
            }

            return default(A);
        }

    }
}