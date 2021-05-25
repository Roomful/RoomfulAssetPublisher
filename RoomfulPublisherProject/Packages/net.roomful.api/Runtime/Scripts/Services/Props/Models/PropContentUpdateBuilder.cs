using System;
using System.Collections.Generic;
using StansAssets.Foundation.Patterns;

namespace net.roomful.api.props
{
    public class PropContentUpdateBuilder : IDisposable
    {
        private readonly List<IResource> m_addedResources;
        private readonly List<IResource> m_removedResources;
        public IEnumerable<IResource> AddedResources => m_addedResources;
        public IEnumerable<IResource> RemovedResources => m_removedResources;

        public bool HasThumbnailChanges { get; private set; }
        public IResource Thumbnail { get; private set; }

        public PropContentUpdateBuilder() {
            m_addedResources = ListPool<IResource>.Get();
            m_removedResources = ListPool<IResource>.Get();
        }

        public void Dispose() {
            ListPool<IResource>.Release(m_addedResources);
            ListPool<IResource>.Release(m_removedResources);
        }

        public void SetThumbnail(IResource res) {
            HasThumbnailChanges = true;
            Thumbnail = res;
        }

        public void AddResource(IResource res) {
            m_addedResources.Add(res);
        }

        public void RemoveResource(IResource res) {
            m_removedResources.Add(res);
        }
    }
}
