using System;
using System.Collections.Generic;
using StansAssets.Foundation.Patterns;

namespace net.roomful.api.props
{
    public class PropContentUpdateBuilder : IDisposable
    {
        private readonly List<IResource> m_addedResources;
        private readonly List<IResource> m_addedResourcesFromRoomContent;

        private readonly List<IResource> m_removedResources;
        private readonly List<IResource> m_removedResourcesToRoomContent;
        public IEnumerable<IResource> AddedResources => m_addedResources;
        public IEnumerable<IResource> AddedResourcesFromRoomContent => m_addedResourcesFromRoomContent;
        public IEnumerable<IResource> RemovedResources => m_removedResources;
        public IEnumerable<IResource> RemovedResourcesToRoomContent => m_removedResourcesToRoomContent;

        public bool HasThumbnailChanges { get; private set; }
        public IResource Thumbnail { get; private set; }

        public PropContentUpdateBuilder() {
            m_addedResources = ListPool<IResource>.Get();
            m_removedResources = ListPool<IResource>.Get();
            m_removedResourcesToRoomContent = ListPool<IResource>.Get();
            m_addedResourcesFromRoomContent = ListPool<IResource>.Get();
        }

        public void Dispose() {
            ListPool<IResource>.Release(m_addedResources);
            ListPool<IResource>.Release(m_removedResources);
            ListPool<IResource>.Release(m_removedResourcesToRoomContent);
            ListPool<IResource>.Release(m_addedResourcesFromRoomContent);
        }

        public void SetThumbnail(IResource res) {
            HasThumbnailChanges = true;
            Thumbnail = res;
        }

        public void AddResource(IResource res) {
            m_addedResources.Add(res);
        }

        public void AddResourceFromRoomContent(IResource res) {
            m_addedResourcesFromRoomContent.Add(res);
        }

        public void RemoveResource(IResource res) {
            m_removedResources.Add(res);
        }

        public void RemoveResourceToRoomContent(IResource res) {
            m_removedResourcesToRoomContent.Add(res);
        }
    }
}
