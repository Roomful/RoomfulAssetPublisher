#if UNITY_2018_3_OR_NEWER

using UnityEngine;
using UnityEngine.Experimental.UIElements;
using RF.AssetWizzard.Network.Request;
using System.Collections.Generic;
using RF.AssetWizzard.Commands;
using RF.AssetWizzard.Results;
using UnityEditor;
using UnityEngine.Experimental.UIElements.StyleEnums;

namespace RF.AssetWizzard.Editor {
    
    public class MyAssetsTab : BaseWizardTab, IWizzardTab {
        
        public override string Name => "My Assets";

        private AssetListView m_listView;
        private AssetInfoView m_assetInfoView;
        private VisualContainer m_myPropsContainer;
        
        //vkladki buttons +  removefrom hierarchy
        public MyAssetsTab() {
            m_listView = new AssetListView();
            m_listView.OnTabSelectCallback(ItemClickHandler);
            
            m_assetInfoView = new AssetInfoView();
            m_assetInfoView.SetEditCallback(EditItemClickHandler);
            m_assetInfoView.SetSendForRewiewCallback(SendItemForReviewClickHandler);
            m_assetInfoView.SetRemoveCallback(RemoveItemClickClickHandler);
            m_assetInfoView.SetCreateDraftCallback(CreateDraftClickHandler);

            var splitter = new VisualSplitter {
                style = {
                    flexGrow = 0.1f
                }
            };
            
            m_myPropsContainer = new VisualContainer {
                m_listView,
                splitter,
                m_assetInfoView
            };
            m_myPropsContainer.style.flexDirection = FlexDirection.Row;
            
            Add(new Button(LoadMoreButtonClickHandler) {
                text = "Load more",
                style = {
                    marginLeft = 125f,
                    marginRight = 125f
                }
            });
        }

        private void LoadMoreButtonClickHandler() {
            var req = new GetOwnPropsList(m_listView.ItemsCount, 10);
            req.PackageCallbackText = PropLoadedHandler;
            req.Send();
        }
        
        private void PropLoadedHandler(string resp) {
            var json = new JSONData(SA.Common.Data.Json.Deserialize(resp));
            var allAssetsList = json.GetValue<List<object>>("assets");
            allAssetsList.ForEach(assetData => {
                m_listView.AddAsset(new PropTemplate(SA.Common.Data.Json.Serialize(assetData)));
            });

            if (allAssetsList.Count > 0) {
                RecalculatePropsContainerHeight(allAssetsList.Count);
                Insert(1, m_myPropsContainer);
            }
        }

        private void RecalculatePropsContainerHeight(int assetsCount) {
            m_myPropsContainer.style.minHeight = assetsCount * (int) EditorGUIUtility.singleLineHeight;
        }

        private void ItemClickHandler(PropTemplate obj) {
            Debug.Log("ItemClickHandler " + obj.Title);
            m_assetInfoView.SetAsset(obj);
        }
        
        private void EditItemClickHandler(PropTemplate obj) {
            Debug.Log("EditItemClickHandler");
            BundleService.Download(obj);
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
            m_listView.UpdateItem(obj.AssetId, obj.NewReleaseStatus);
        }

        private void RemoveItemClickClickHandler(PropTemplate obj) {
            Debug.Log("RemoveItemClickClickHandler");
            new RemoveAssetCommand(obj.Id).Execute(RemoveAssetHandler);
        }

        private void RemoveAssetHandler(RemoveAssetResult obj) {
            Debug.Log("RemoveAssetHandler success" + obj.Success);
            m_listView.RemoveItem(obj.AssetId);
            m_assetInfoView.ClearTemplate();
        }

        private void CreateDraftClickHandler(PropTemplate obj) {
            Debug.Log("CreateDraftClickHandler");
            new CreatePropDraftFromReleaseCommand(obj.Id).Execute(CreatePropDraftHandler);
        }

        private void CreatePropDraftHandler(AssetRelatedCommandResult<PropTemplate> obj) {
            Debug.Log("CreatePropDraftHandler success" + obj.Success);
            if (obj.Success) {
                m_listView.AddAsset(obj.Asset);
            }
        }
    }
}

#endif
