using net.roomful.api.props;
using UnityEngine;

namespace net.roomful.assets.serialization
{
    public class JukeBox : VerusMusicPlayer
    {
        [Header("Action Buttons")]
        [SerializeField] GameObject m_MyButton;
        [SerializeField] GameObject m_NewButton;
        [SerializeField] GameObject m_GiftButton;
        [SerializeField] GameObject m_BuyButton;

        public ActionButton MyButton { get; private set; }
        public ActionButton NewButton { get; private set; }
        public ActionButton GiftButton { get; private set; }
        public ActionButton BuyButton { get; private set; }

        public override void Init(IProp prop, int componentIndex)
        {
            base.Init(prop, componentIndex);
            MyButton = m_MyButton.GetComponentInChildren<ActionButton>();
            NewButton = m_NewButton.GetComponentInChildren<ActionButton>();
            GiftButton = m_GiftButton.GetComponentInChildren<ActionButton>();
            BuyButton = m_BuyButton.GetComponentInChildren<ActionButton>();
        }
    }
}
