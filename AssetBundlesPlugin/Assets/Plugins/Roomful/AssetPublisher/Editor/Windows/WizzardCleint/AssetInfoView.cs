
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
        PropTemplate m_template;

        public AssetInfoView() : base() {
            style.flexGrow = 0.5f;
            style.minWidth = 200;
            style.positionLeft = 200;
        }

        public void SetAsset(PropTemplate item) {
            Clear();
            m_template = item;
            Add(new Label("CanStack " + m_template.CanStack.ToString()));
            Add(new Label("ContentTypes " + m_template.ContentTypes.ToString()));
            Add(new Label("Created " + m_template.Created.ToString()));
            Add(new Label("DisaplyContent " + m_template.DisaplyContent.ToString()));
            Add(new Label("Icon " + m_template.Icon.ToString()));
            Add(new Label("Id" + m_template.Id));
            Add(new Label("InvokeType " + m_template.InvokeType.ToString()));
            Add(new Label("IsNew " + m_template.IsNew.ToString()));
            Add(new Label("MaxSize " + m_template.MaxSize.ToString()));
            Add(new Label("MinSize " + m_template.MinSize.ToString()));
            Add(new Label("PedestalInZoomView " + m_template.PedestalInZoomView.ToString()));
            Add(new Label("Placing " + m_template.Placing.ToString()));
            Add(new Label("Size " + m_template.Size.ToString()));
            Add(new Label("Tags " + m_template.Tags.ToString()));
            Add(new Label("Title " + m_template.Title));
            Add(new Label("Updated " + m_template.Updated.ToString()));
            Add(new Label("Urls" + m_template.Urls.ToString()));
            Add(new Label("ReleaseStatus " + m_template.ReleaseStatus.ToString()));
            Add(new Label("DraftAssetId " + m_template.DraftAssetId.ToString()));
            Add(new Label("ReleaseAssetId" + m_template.ReleaseAssetId.ToString()));
            Button button;
            if (m_template.ReleaseStatus == ReleaseStatus.released) {
                if (string.IsNullOrEmpty(m_template.DraftAssetId)) {
                    button = new Button(OnCreateDraftClick);
                    button.text = "Create Draft";
                    Add(button);
                }
            }
            else {
                if (m_template.ReleaseStatus != ReleaseStatus.pending) {
                    button = new Button(OnSendForReviewClick);
                    button.text = "Send For review ";
                    Add(button);
                }
                button = new Button(OnEditDraftClick);
                button.text = "Edit Draft";
                Add(button);
            }
            button = new Button(OnRemoveClick);
            button.text = "Remove Asset";
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

        public PropTemplate Item {
            get {
                return m_template;
            }
        }
    }
}

#endif