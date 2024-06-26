using net.roomful.api.props;
using UnityEngine;

namespace net.roomful.assets.serialization
{
    public class MusicMintStation : VerusMusicPlayer
    {
        [Header("Action Buttons")]
        [SerializeField] GameObject m_CreateButton;
        [SerializeField] GameObject m_ManageContentButton;
        [SerializeField] GameObject m_ListForSaleButton;

        public ActionButton CreateButton { get; private set; }
        public ActionButton ListForSaleButton { get; private set; }
        public ActionButton ManageContentButton { get; private set; }

        public override void Init(IProp prop, int componentIndex)
        {
            base.Init(prop, componentIndex);
            CreateButton = m_CreateButton.GetComponentInChildren<ActionButton>();
            ListForSaleButton = m_ManageContentButton.GetComponentInChildren<ActionButton>();
            ManageContentButton = m_ListForSaleButton.GetComponentInChildren<ActionButton>();
        }
    }
}
