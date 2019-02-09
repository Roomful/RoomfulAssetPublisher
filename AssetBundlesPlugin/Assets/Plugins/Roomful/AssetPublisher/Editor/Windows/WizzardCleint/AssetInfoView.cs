#if UNITY_2018_3_OR_NEWER

using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace RF.AssetWizzard.Editor {
    
    public class AssetInfoView : VisualElement {

        private ListView m_listView;
        private Action<PropTemplate> m_onCreateDraftClick;
        private Action<PropTemplate> m_onRemoveClick;
        private Action<PropTemplate> m_onSendToReviewClick;
        private Action<PropTemplate> m_onEditClick;
        private PropTemplate m_template;

        public AssetInfoView() {
            style.flexGrow = 0.5f;
            /*style.minWidth = 200;
            style.positionLeft = 200;*/
        }
        
        public void SetAsset(PropTemplate item) {
            Clear();
            m_template = item;
            Add(new Label("CanStack: " + m_template.CanStack));
            Add(new Label("ContentTypes: " + m_template.ContentTypes));
            Add(new Label("Created: " + m_template.Created));
            Add(new Label("DisaplyContent: " + m_template.DisaplyContent));
            Add(new Label("Icon: " + m_template.Icon));
            Add(new Label("Id: " + m_template.Id));
            Add(new Label("InvokeType: " + m_template.InvokeType));
            Add(new Label("IsNew: " + m_template.IsNew));
            Add(new Label("MaxSize: " + m_template.MaxSize));
            Add(new Label("MinSize: " + m_template.MinSize));
            Add(new Label("PedestalInZoomView: " + m_template.PedestalInZoomView));
            Add(new Label("Placing: " + m_template.Placing));
            Add(new Label("Size: " + m_template.Size));
            Add(new Label("Tags: " + m_template.Tags));
            Add(new Label("Title: " + m_template.Title));
            Add(new Label("Updated: " + m_template.Updated));
            Add(new Label("Urls: " + m_template.Urls));
            Add(new Label("ReleaseStatus: " + m_template.ReleaseStatus));
            Add(new Label("DraftAssetId: " + m_template.DraftAssetId));
            Add(new Label("ReleaseAssetId: " + m_template.ReleaseAssetId));
            
            Button button;
            if (m_template.ReleaseStatus == ReleaseStatus.released) {
                if (string.IsNullOrEmpty(m_template.DraftAssetId)) {
                    button = new Button(OnCreateDraftClick) {
                        text = "Create Draft",
                        style = {
                            marginLeft = 125f,
                            marginRight = 125f
                        }
                    };
                    Add(button);
                }
            }
            else {
                if (m_template.ReleaseStatus != ReleaseStatus.pending) {
                    button = new Button(OnSendForReviewClick) {
                        text = "Send For review ",
                        style = {
                            marginLeft = 125f,
                            marginRight = 125f
                        }
                    };
                    Add(button);
                }
                button = new Button(OnEditDraftClick) {
                    text = "Edit Draft",
                    style = {
                        marginLeft = 125f,
                        marginRight = 125f
                    }
                };
                Add(button);
            }
            button = new Button(OnRemoveClick) {
                text = "Remove Asset",
                style = {
                    marginLeft = 125f,
                    marginRight = 125f
                }
            };
            Add(button);
        }

        internal void ClearTemplate() {
            m_template = null;
            Clear();
        }

        private void OnRemoveClick() {
            m_onRemoveClick?.Invoke(m_template);
        }

        private void OnSendForReviewClick() {
            m_onSendToReviewClick?.Invoke(m_template);
        }

        private void OnEditDraftClick() {
            m_onEditClick?.Invoke(m_template);
        }

        private void OnCreateDraftClick() {
            m_onCreateDraftClick?.Invoke(m_template);
        }

        public void SetEditCallback(Action<PropTemplate> callback) {
            m_onEditClick = callback;
        }

        public void SetCreateDraftCallback(Action<PropTemplate> callback) {
            m_onCreateDraftClick = callback;
        }

        public void SetRemoveCallback(Action<PropTemplate> callback) {
            m_onRemoveClick = callback;
        }

        public void SetSendForRewiewCallback(Action<PropTemplate> callback) {
            m_onSendToReviewClick = callback;
        }

        public PropTemplate Item => m_template;
    }
}
#endif