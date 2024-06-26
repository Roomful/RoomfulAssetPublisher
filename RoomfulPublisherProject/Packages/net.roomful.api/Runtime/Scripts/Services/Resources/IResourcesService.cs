using System;
using UnityEngine;

namespace net.roomful.api.resources
{
    /// <summary>
    /// Services provides resources related events and API to interact with resources.
    /// </summary>
    public interface IResourcesService
    {
        event Action<ResourcesUpdateArgs> OnResourcesUpdated;

        /// <summary>
        /// Returns cached resourced with specified id.
        /// </summary>
        /// <param name="resourceId">The id if the requested resource.</param>
        /// <returns>Resource instance if found, or `null` otherwise.</returns>
        IResource GetResource(string resourceId);

        /// <summary>
        /// Load info about the resource.
        /// If cached version of the resource exists it will be returned without a server request.
        /// </summary>
        /// <param name="resourceId">The id if the requested resource.</param>
        /// <param name="callback">The callback with loaded resource instance, or `null` in case of the request fail.</param>
        void LoadResourceInfo(string resourceId, Action<IResource> callback);
        
        /// <summary>
        /// Returns cached resourced with specified id.
        /// If cache is missing, it would Load resource info from the server and cache it.
        /// </summary>
        /// <param name="resourceId">The id if the requested resource.</param>
        /// <param name="callback">The callback with loaded resource instance, or `null` in case of the request fail.</param>
        void RetrieveResourceInfo(string resourceId, Action<IResource> callback);
        void RetrieveResourceInfo(string resourceId, string networkId, Action<IResource> callback);
        void ShowResourcesMetaEditDialog(IResource resource);
        IResource CreateOrUpdateResource(JSONData resourceInfo);
        IResource CreateOrUpdateResource(ResourceDataModel resourceData);
        IResource CreateUntrackedResource(JSONData resourceInfo);
        IResource CreateUntrackedResourceWithThumbnail(JSONData resourceInfo, string thumbnailUrl);
        IResource CreateUntrackedResource(string id);
        void UploadResource(string name, string mimeType, byte[] data, Action<IResource> callback);
        IResource CreateUntrackedResourceFromImageUrl(string url);
        void CreateResourceLink(IResource reference, Action<ResourceLinkCreateResult> callback);

        void UpdateResourceAttributes(IResource resource, ResourceUpdateBuilder updateBuilder);

        /// <summary>
        /// Set user reaction for resource.
        /// </summary>
        /// <param name="resource">Target resource.</param>
        /// <param name="reaction">User reaction.</param>
        void SetReaction(IResource resource, Emoji reaction);

        /// <summary>
        /// Delete User reaction for resource.
        /// </summary>
        /// <param name="resource">Target resource.</param>
        void DeleteReaction(IResource resource);

        /// <summary>
        /// Create Resource with provided data.
        /// NOTE: If creation is failed <see cref="callback"/> will return `null`.
        /// </summary>
        /// <param name="uploadData">The resource creation data.</param>
        /// <param name="callback">Creation callback.</param>
        void CreateImageResource(ImageUploadData uploadData, Action<IResource> callback);

        /// <summary>
        /// Create Resource with provided data.
        /// NOTE: If creation is failed <see cref="callback"/> will return `null`.
        /// </summary>
        /// <param name="uploadData">The resource creation data.</param>
        /// <param name="callback">Creation callback.</param>
        void CreateAudioResource(AudioUploadData uploadData, Action<IResource> callback);

        /// <summary>
        /// Set thumbnail for the resources.
        /// </summary>
        /// <param name="resource">Target resource.</param>
        /// <param name="texture2D">Thumbnail texture.</param>
        /// <param name="callback">Operation callback.</param>
        void SetResourceThumbnail(IResource resource, Texture2D texture2D, Action callback);

        /// <summary>
        /// Open dialog for send a report to the resource.
        /// </summary>
        /// <param name="resource">Target resource.</param>
        void ShowResourceReportDialog(IResource resource);
    }
}
