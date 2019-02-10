#if UNITY_2018_3_OR_NEWER

using System;
using UnityEngine.Experimental.UIElements;

namespace RF.AssetWizzard.Editor {
    
    public class AssetInfoView : VisualElement {

        private ListView m_listView;
        private Action<PropTemplate> m_onCreateDraftClick;
        private Action<PropTemplate> m_onRemoveClick;
        private Action<PropTemplate> m_onSendToReviewClick;
        private Action<PropTemplate> m_onEditClick;
        private PropTemplate m_template;

        private const float AVATAR_MAX_WIDTH = 64f;
        private const float AVATAR_MAX_HEIGHT = 64f;
        
        public AssetInfoView() {
            style.flexGrow = 0.5f;
        }
        
        public void SetAsset(PropTemplate item) {
            Clear();
            m_template = item;

            var resourceAvatarLabel = new Label();
            Add(resourceAvatarLabel);
            
            if(!string.IsNullOrEmpty(m_template.Icon.Id)) {
                TextureLoader.GetTextureWWW(m_template.Icon, tex => {
                    var width = (float)tex.width;
                    var height = (float)tex.height;
                    TextureResizer.ResizeTextureMaintainAspectRatio(AVATAR_MAX_WIDTH, AVATAR_MAX_HEIGHT, ref width, ref height);
                    
                    resourceAvatarLabel.style.width = width;
                    resourceAvatarLabel.style.height = height;
                    resourceAvatarLabel.style.backgroundImage = tex;
                });
            }
            
            Add(new Label("Title: " + m_template.Title));
            Add(new Label("Id: " + m_template.Id));
            Add(new Label("Created: " + m_template.Created));
            Add(new Label("Updated: " + m_template.Updated));
            
            Add(new Label("IsNew: " + m_template.IsNew));           
            Add(new Label("DisplayContent: " + m_template.DisplayContent));
            Add(new Label("Size: " + m_template.Size));
            Add(new Label("MinSize: " + m_template.MinSize));
            Add(new Label("MaxSize: " + m_template.MaxSize));
            Add(new Label("CanStack: " + m_template.CanStack));
            Add(new Label("InvokeType: " + m_template.InvokeType));
            Add(new Label("PedestalInZoomView: " + m_template.PedestalInZoomView));
            Add(new Label("Placing: " + m_template.Placing));
            Add(new Label("ReleaseStatus: " + m_template.ReleaseStatus));
            Add(new Label("DraftAssetId: " + m_template.DraftAssetId));
            Add(new Label("ReleaseAssetId: " + m_template.ReleaseAssetId));
            
            var types = string.Empty;
            var contentTypesLabel = new Label();
            if (m_template.ContentTypes.Count > 0) {
                m_template.ContentTypes.ForEach(type => {types = string.Concat(type.ToString(), ",");});
                types = types.Substring(0, types.Length - 1);
            }
            contentTypesLabel.text = "Types: " + types;
            Add(contentTypesLabel);
            
            var tags = string.Empty;
            var tagsLabel = new Label();
            if (m_template.Tags.Count > 0) {
                m_template.Tags.ForEach(tag => {tags = string.Concat(tag, ",");});
                tags = tags.Substring(0, tags.Length - 1);
            }
            tagsLabel.text = "Tags: " + tags;
            Add(tagsLabel);
            
            var urls = string.Empty;
            var urlsLabel = new Label();
            if (m_template.Urls.Count > 0) {
                m_template.Urls.ForEach(url => {urls = string.Concat(url.Platform, ",");});
                urls = urls.Substring(0, urls.Length - 1);
            }
            urlsLabel.text = "Urls: " + urls;
            Add(urlsLabel);
            
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