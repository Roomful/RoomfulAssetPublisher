using System;
using net.roomful.api.props;

namespace net.roomful.api.zoom
{
    public class CustomPropAction : ICustomPropAction
    {
        private readonly Action<IProp> m_onExecute;

        public CustomPropAction(Action<IProp> onExecute) {
            m_onExecute = onExecute;
        }

        public void Execute(IProp prop) {
            m_onExecute.Invoke(prop);
        }
    }
}
