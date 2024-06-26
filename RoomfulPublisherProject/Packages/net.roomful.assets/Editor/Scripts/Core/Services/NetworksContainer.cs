using System.Collections.Generic;

namespace net.roomful.assets.editor
{
    public class NetworksContainer
    {
        readonly List<RoomfulNetwork> m_Models = new List<RoomfulNetwork>();
        readonly List<string> m_Names = new List<string>();
        readonly Dictionary<string, string> m_NetworkNameToId = new Dictionary<string, string>();
        readonly Dictionary<string, RoomfulNetwork> m_NetworkIdToModel = new Dictionary<string, RoomfulNetwork>();

        public IReadOnlyList<string> Names => m_Names;
        public IReadOnlyList<RoomfulNetwork> Models => m_Models;


        public void Clear()
        {
            m_Models.Clear();
            m_Names.Clear();
            m_NetworkNameToId.Clear();
            m_NetworkIdToModel.Clear();
        }
        
        public string GetNetworkId(string networkName)
        {
            return m_NetworkNameToId[networkName];
        }
        
        public RoomfulNetwork GetNetwork(string networkId)
        {
            
            if (string.IsNullOrEmpty(networkId))
            {
                return new RoomfulNetwork("-", "-");
            }
            
            return m_NetworkIdToModel.TryGetValue(networkId, out var network) ? network : null;
        }

        public void Add(RoomfulNetwork network)
        {
            m_Models.Add(network);
            m_Names.Add(network.Name);
            m_NetworkNameToId.Add(network.Name, network.Id);
            m_NetworkIdToModel.Add(network.Id, network);
        }
    }
}
