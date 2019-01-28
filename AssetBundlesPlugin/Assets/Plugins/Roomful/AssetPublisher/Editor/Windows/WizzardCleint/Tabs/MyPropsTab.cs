#if UNITY_2018_3_OR_NEWER

using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.UIElements;
using System;
using RF.AssetWizzard.Network.Request;
using System.Collections.Generic;
using RF.AssetWizzard.Commands;
using UnityEngine.Experimental.UIElements.StyleEnums;

namespace RF.AssetWizzard.Editor
{
    public class MyPropsTab : BaseWizardTab, IWizzardTab
    {
        public override string Name {
            get {
                return "My Props";
            }
        }

        AssetListView m_listview;
        AssetInfoView m_assetInfoView;
        public MyPropsTab() {
            style.flexDirection = FlexDirection.Row;
            var loadMoreButton = new Button(LoadMoreButtonClickHandler);
            loadMoreButton.text = "Load more";
            m_listview = new AssetListView();
            m_listview.Add(loadMoreButton);
            m_listview.OnTabSelectCallback(ItemClickHandler);
            Add(m_listview);
            m_assetInfoView = new AssetInfoView();
            m_assetInfoView.SetEditCallback(EditItemClickHandler);
            m_assetInfoView.SetSendForRewiewCallback(SendItemForReviewClickHandler);
            m_assetInfoView.SetRemoveCallback(RemoveItemClickClickHandler);
            m_assetInfoView.SetCreateDraftCallback(CreateDraftClickHandler);
            Add(m_assetInfoView);
        }

        private void SendItemForReviewClickHandler(PropTemplate obj) {
            if (obj.ReleaseStatus == ReleaseStatus.released) {
                return;
            }
            new SendAssetForReviewCommand(obj.Id).Execute(SendAssetForReviewHandler);
            Debug.Log("SendItemForReviewClickHandler");
        }

        private void SendAssetForReviewHandler(SendForReviewResult obj) {
            Debug.Log("SendItemForReviewClickHandler success" + obj.Success);
            m_listview.UpdateItem(obj.AssetId, obj.NewReleaseStatus);
        }

        private void RemoveItemClickClickHandler(PropTemplate obj) {
            Debug.Log("RemoveItemClickClickHandler");
            new RemoveAssetCommand(obj.Id).Execute(RemoveAssetHandler);
        }

        private void RemoveAssetHandler(RemoveAssetResult obj) {
            Debug.Log("RemoveAssetHandler success" + obj.Success);
            m_listview.RemoveItem(obj.AssetId);
            m_assetInfoView.ClearTemplate();
        }

        private void CreateDraftClickHandler(PropTemplate obj) {
            Debug.Log("CreateDraftClickHandler");
            new CreatePropDraftFromReleaseCommand(obj.Id).Execute(CreatePropDraftHandler);
        }

        private void CreatePropDraftHandler(AssetRelatedCommandResult<PropTemplate> obj) {
            Debug.Log("CreatePropDraftHandler success" + obj.Success);
            if (obj.Success) {
                m_listview.AddAsset(obj.Asset);
            }
        }

        private void EditItemClickHandler(PropTemplate obj) {
            Debug.Log("EditItemClickHandler");
            BundleService.Download(obj);
        }

        private void ItemClickHandler(PropTemplate obj) {
            Debug.Log("ItemClickHandler " + obj.Title);
            m_assetInfoView.SetAsset(obj);
        }

        private void LoadMoreButtonClickHandler() {
            var req = new GetOwnPropsList(m_listview.ItemsCount, 10);
            req.PackageCallbackText = PropLoadedHandler;
            req.Send();
        }

        private void PropLoadedHandler(string resp) {
            JSONData json = new JSONData(SA.Common.Data.Json.Deserialize(resp));
            List<object> allAssetsList = json.GetValue<List<object>>("assets");
            foreach (object assetData in allAssetsList) {
                PropTemplate at = new PropTemplate(SA.Common.Data.Json.Serialize(assetData));
                m_listview.AddAsset(at);
            }
        }
    }
}

#endif
