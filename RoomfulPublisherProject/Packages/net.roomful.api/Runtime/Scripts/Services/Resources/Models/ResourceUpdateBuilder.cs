using System;
using System.Collections.Generic;
using StansAssets.Foundation.Patterns;

namespace net.roomful.api.resources
{
    public class ResourceUpdateBuilder : IDisposable
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Date { get; set; }
        public IReadOnlyDictionary<string, object> UpdatedParams => m_updatedParams;
        public IEnumerable<string> RemovedParams => m_removedParams;


        private Dictionary<string, object> m_updatedParams;
        private readonly List<string> m_removedParams;

        public ResourceUpdateBuilder() {
            m_removedParams = ListPool<string>.Get();
        }

        public void Dispose() {
            ListPool<string>.Release(m_removedParams);
        }


        public void SetParam(string paramKey, object paramValue) {
            if (m_updatedParams == null) {
                m_updatedParams = new Dictionary<string, object>();
            }

            m_updatedParams[paramKey] = paramValue;
        }

        public void RemoveParam(string paramKey) {
            m_removedParams.Add(paramKey);
        }
    }
}
