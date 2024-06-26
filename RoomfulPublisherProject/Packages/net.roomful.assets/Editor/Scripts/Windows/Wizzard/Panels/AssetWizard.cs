using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Rotorz.ReorderableList;
using StansAssets.Plugins.Editor;
using UnityEngine.SceneManagement;

namespace net.roomful.assets.editor
{
    abstract class AssetWizard<TAsset> : WizardUIComponent, IAssetWizzard where TAsset : IAsset
    {
        TAsset m_CurrentAsset;
        GameObject m_CurrentAssetGameObject;
        protected readonly string[] m_StatusOptions;
        bool m_UpdateRequestInProgress;

        //--------------------------------------
        // Abstract Methods
        //--------------------------------------

        protected abstract void DrawGUI(bool guiState);

        protected abstract void Create();
        protected abstract void Upload();
        protected abstract void Download();

        protected abstract void UpdateMeta();

        public void OnGUI(bool guiState)
        {
            if (m_UpdateRequestInProgress)
            {
                DrawPreloaderAt(new Rect(270, 12, 20, 20));
            }

            using (new IMGUIEnable(!m_UpdateRequestInProgress))
            {
                DrawGUI(guiState);
            }
        }

        protected AssetWizard()
        {
            var statusOptions = new List<string>();
            statusOptions.Add("Work In Progress");
            statusOptions.Add("Published");

            m_StatusOptions = statusOptions.ToArray();
        }

        protected void DrawStatus()
        {
            EditorGUI.BeginChangeCheck();
            using (new IMGUIBeginHorizontal())
            {
                EditorGUILayout.LabelField("Status: ", GUILayout.Width(100));
                AssetTemplate.Status = (AssetStatus) EditorGUILayout.Popup((int)AssetTemplate.Status, m_StatusOptions, GUILayout.Width(240));
            }

            if (EditorGUI.EndChangeCheck())
            {
                m_UpdateRequestInProgress = true;
                var updateStatus = new UpdateStatus(AssetTemplate.Id, AssetTemplate.Status);
                updateStatus.Send(result =>
                {
                    m_UpdateRequestInProgress = false;
                    if (result.IsSucceeded)
                    {
                        EditorUtility.DisplayDialog(
                            "Completed", $"{AssetTemplate.Title} status updated to '{AssetTemplate.Status}'", "Ok");
                    }
                });
            }
        }
        
        protected void DrawNetwork()
        {
            EditorGUI.BeginChangeCheck();

            var network = RoomfulPublisher.Session.Networks.GetNetwork(AssetTemplate.NetworkId);
            if (network == null)
            {
                Debug.LogWarning("Failed to identify asset network: " + AssetTemplate.NetworkId);
                return;
            }
            var index = RoomfulPublisher.Session.Networks.Names.ToList().IndexOf(network.Name);
            using (new IMGUIBeginHorizontal())
            {
                EditorGUILayout.LabelField("Network: ", GUILayout.Width(100));
                index = EditorGUILayout.Popup(index, RoomfulPublisher.Session.Networks.Names.ToArray(), GUILayout.Width(240));
            }
            
            if (EditorGUI.EndChangeCheck())
            {
                
                m_UpdateRequestInProgress = true;
                var newNetwork = RoomfulPublisher.Session.Networks.Models[index];
                AssetTemplate.NetworkId = newNetwork.Id;
                
                var updateStatus = new UpdateOwnership(AssetTemplate.Id, newNetwork.Id);
                updateStatus.Send(result =>
                {
                    m_UpdateRequestInProgress = false;
                    if (result.IsSucceeded)
                    {
                        EditorUtility.DisplayDialog(
                            "Completed", $"{AssetTemplate.Title} has been transferred to '{newNetwork.Name}'", "Ok");
                    }
                });
            }
        }

        protected void DrawTitleFiled() {
            using (new IMGUIBeginHorizontal())
            {
                EditorGUILayout.LabelField("Title: ", GUILayout.Width(100));
                AssetTemplate.Title = EditorGUILayout.TextField(AssetTemplate.Title, GUILayout.Width(240));
            }
        }

        protected void DrawTags() {
            GUILayout.BeginVertical(GUILayout.Width(225));
            {
                ReorderableListGUI.Title("Asset Tags");
                try {
                    ReorderableListGUI.ListField(AssetTemplate.Tags, TagListItem, DrawEmptyTag);
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                }
            }
            GUILayout.EndVertical();
        }

        protected void DrawControlButtons() {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            var buttonRect1 = new Rect(460, 360, 120, 18);
            var buttonRect2 = new Rect(310, 360, 120, 18);
            var buttonRect3 = new Rect(460, 390, 120, 18);

            var buttonRect4 = new Rect(310, 390, 120, 18);

            if (Asset.GetTemplate().IsNew) {
                var upload = GUI.Button(buttonRect1, "Upload");
                if (upload) {
                    Upload();
                }
            }
            else {
                var upload = GUI.Button(buttonRect1, "Reupload");
                if (upload) {
                    Upload();
                }

                var refresh = GUI.Button(buttonRect2, "Refresh");
                if (refresh) {
                    Download();
                }

                var updateMeta = GUI.Button(buttonRect4, "Update Meta");
                if (updateMeta) {
                    UpdateMeta();
                }
            }

            var create = GUI.Button(buttonRect3, "Create New");
            if (create) {
                Create();
            }

            GUILayout.Space(40f);
            GUILayout.EndHorizontal();
        }

        //--------------------------------------
        // Get / Set
        //--------------------------------------

        public bool HasAsset => Asset != null;

        AssetTemplate AssetTemplate => Asset.GetTemplate();

        protected TAsset Asset {
            get {
                if (m_CurrentAssetGameObject != null) {
                    return m_CurrentAsset;
                }
                else {
                    return FindAsset();
                }
            }
        }

        //--------------------------------------
        // Private Methods
        //--------------------------------------

        string TagListItem(Rect position, string itemValue) {
            if (itemValue == null)
                itemValue = "new_tag";
            return EditorGUI.TextField(position, itemValue);
        }

        void DrawEmptyTag() {
            GUILayout.Label("No items in list.", EditorStyles.miniLabel);
        }

        TAsset FindAsset() {
            var scene = SceneManager.GetActiveScene();
            foreach (var gameObject in scene.GetRootGameObjects()) {
                var target = gameObject.GetComponent<TAsset>();
                if (target != null) {
                    m_CurrentAsset = target;
                    m_CurrentAssetGameObject = m_CurrentAsset.gameObject;
                    return target;
                }
            }

            return default;
        }
    }
}
