using System;
using UnityEngine;

namespace net.roomful.api.resources
{
    /// <summary>
    /// Services provides resources related events and API to interact with resources.
    /// </summary>
    public interface IResourcesService
    {
        event Action<IResource> OnResourcesUpdated;
        void ShowResourcesMetaEditDialog(IResource resource);

        void UpdateResourceAttributes(IResource resource);

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
