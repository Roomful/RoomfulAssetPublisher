using System.Collections.Generic;
using StansAssets.Foundation;
using UnityEngine;

namespace net.roomful.assets
{
    abstract class GetAssetsList : BaseWebPackage
    {
        public const string PackUrl = "/api/v0/asset/list";
        readonly string m_Title = string.Empty;
        string m_Id = string.Empty;
        readonly List<string> m_Tags = new List<string>();
        readonly int m_Offset;
        readonly int m_Size;
        string m_Network;

        protected GetAssetsList(int offset, int size, List<string> tags, string url) : base(url)
        {
            m_Offset = offset;
            m_Size = size;

            m_Tags.AddRange(tags);
        }

        protected GetAssetsList(int offset, int size, string title, string url) : base(url)
        {
            m_Offset = offset;
            m_Size = size;
            m_Title = title;
        }

        public void SetNetwork(string networkId)
        {
            m_Network = networkId;
        }

        public void SetId(string id)
        {
            m_Id = id;
        }

        public override Dictionary<string, object> GenerateData()
        {
            var OriginalJSON = new Dictionary<string, object>();

            if (m_Tags.Count > 0)
            {
                OriginalJSON.Add("tags", m_Tags);
            }

            if (!m_Title.Equals(string.Empty))
            {
                OriginalJSON.Add("title", m_Title);
            }

            if (!m_Id.Equals(string.Empty))
            {
                OriginalJSON.Add("id", m_Id);
            }

            if (!string.IsNullOrEmpty(m_Network))
            {
                OriginalJSON.Add("forNetwork", m_Network);
            }

            OriginalJSON.Add("offset", m_Offset);
            OriginalJSON.Add("size", m_Size);

            return OriginalJSON;
        }
    }
}