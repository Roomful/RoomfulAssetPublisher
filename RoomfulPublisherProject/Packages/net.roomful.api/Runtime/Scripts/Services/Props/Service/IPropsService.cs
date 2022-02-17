using System;
using System.Collections.Generic;
using net.roomful.api.cameras;
using UnityEngine;

namespace net.roomful.api.props
{
    /// <summary>
    /// Services provides prop related events and API to interact with props.
    /// </summary>
    public interface IPropsService
    {
        /// <summary>
        /// Event fired when prop is created.
        /// In most cases prop is still loading and can't be used for all the interactions.
        /// See the <see cref="IProp.IsLoading"/>.
        /// </summary>
        event Action<IProp> OnPropCreated;

        /// <summary>
        /// Event fired when prop is loaded and ready for all interaction. See <see cref="IProp.IsLoading"/>.
        /// </summary>
        event Action<IProp> OnPropLoaded;

        /// <summary>
        /// Event is fired when prop is about to be destroyed
        /// </summary>
        event Action<IProp> OnPropWillDestroy;

        /// <summary>
        /// Event is fired when prop destroyed
        /// </summary>
        event Action<IProp> OnPropDestroyed;

        /// <summary>
        /// Event is fired when prop template is updated.
        ///
        /// *Note:* Prop template instance can be replaced with new objects.
        ///         Do not cache prop template instance.
        /// </summary>
        event Action<IProp> OnPropUpdate;

        /// <summary>
        /// List of the props in the current room.
        /// </summary>
        IReadOnlyList<IProp> CurrentRoomProps { get; }

        /// <summary>
        /// Method will return prop in current Room by it's id.
        /// </summary>
        /// <param name="id">The id of the prop.</param>
        /// <returns>Prop instance or `null` if prop with such id wasn't found.</returns>
        IProp GetCurrentRoomPropById(string id);

        /// <summary>
        /// Returns prop under the Camera hits.
        /// See <see cref="IRoomCameraService.GetRaycastHits"/>
        /// </summary>
        /// <param name="hits"></param>
        /// <returns>Prop instance under the camera hit or `null`.</returns>
        IProp GetPropsUnderHits(CameraRayHits hits);

        /// <summary>
        /// Preview prop.
        /// </summary>
        /// <param name="propId">Prop id.</param>
        void PreviewProp(string propId);

        /// <summary>
        /// Removes prop from the room.
        /// </summary>
        /// <param name="propTemplateId">Prop template id.</param>
        void DestroyProp(string propTemplateId);

        /// <summary>
        /// Removes prop from the room without generating undo action.
        /// </summary>
        /// <param name="propTemplateId">Prop template id.</param>
        void DestroyPropWithoutUndo(string propTemplateId);

        /// <summary>
        /// Use method to disable prop editing for the specified prop.
        /// Please note that you disabling editing for a prop only for the current client.
        /// </summary>
        /// <param name="propTemplateId">Prop template id.</param>
        void DisablePropEdit(string propTemplateId);

        /// <summary>
        /// Update Prop template.
        /// </summary>
        /// <param name="propTemplateId">Prop template id.</param>
        /// <param name="propUpdateBuilder">Update template builder.</param>
        /// <param name="supportUndo">Use `false` if this update actions should be skipped in undo.</param>
        void UpdateProp(string propTemplateId, PropUpdateBuilder propUpdateBuilder, bool supportUndo = true);

        /// <summary>
        /// Update Prop content.
        /// </summary>
        /// <param name="propTemplateId">Prop template id.</param>
        /// <param name="contentUpdateBuilder">Update content builder.</param>
        /// <param name="supportUndo">Use `false` if this update actions should be skipped in undo.</param>
        void UpdatePropContent(string propTemplateId, PropContentUpdateBuilder contentUpdateBuilder, bool supportUndo = true);

        /// <summary>
        /// Save Current prop transformation to the server.
        /// Method will save current prop physical transform.
        /// </summary>
        /// <param name="propTemplateId">Prop template id.</param>
        void SavePropTransformation(string propTemplateId);

        /// <summary>
        /// Create new prop. Please note that prop only exists locally until you use <see cref="ServerCreateProp"/>
        /// </summary>
        /// <param name="assetTemplate">Prop asset template.</param>
        /// <param name="initialTemplateBuilder">Use to set initial prop template properties.</param>
        /// <param name="transformation">Use to set prop initial transform values</param>
        /// <param name="includePropLoader">Use 'false' if you do not want default preloader to be added on a prop</param>
        /// <returns>Returns prop instance that only exists locally.</returns>
        IProp InstantiateProp(IPropAssetTemplate assetTemplate,
            PropUpdateBuilder initialTemplateBuilder,
            PropTransformationBuilder transformation,
            bool includePropLoader = true);

        /// <summary>
        /// Saves local prop on a server side.
        /// </summary>
        /// <param name="propTemplateId">Prop template id</param>
        /// <param name="supportUndo">Use `false` if this update actions should be skipped in undo.</param>
        /// <param name="moveResourcesToRoomOnUndo">Defines if resource should be moved to room on undo.</param>
        void ServerCreateProp(string propTemplateId, bool supportUndo = true, bool moveResourcesToRoomOnUndo = false);

        /// <summary>
        /// Loads prop asset by asset id.
        /// Note: if asset with such id does not exists on a server,
        /// <see cref="callback"/> will contain `null` object.
        /// </summary>
        /// <param name="assetId">Prop asset id.</param>
        /// <param name="callback">Callback fired when asset is loaded.</param>
        void GetPropAsset(string assetId, Action<IPropAssetTemplate> callback);

        /// <summary>
        /// Remove skins from the prop.
        /// </summary>
        /// <param name="propTemplateId">Prop template id.</param>
        /// <param name="skinId">Skin Id.</param>
        /// <param name="localOnly">Set to `true` if you only want this skin to removed locally.</param>
        void RemoveSkin(string propTemplateId, string skinId, bool localOnly);

        /// <summary>
        /// Apply skins from the prop.
        /// </summary>
        /// <param name="propTemplateId">Prop template id.</param>
        /// <param name="skinId">Skin Id.</param>
        /// <param name="localOnly">Set to `true` if you only want this skin to be applied locally.</param>
        void ApplySkin(string propTemplateId, string skinId, bool localOnly);

        /// <summary>
        /// Apply color to the prop variant.
        /// </summary>
        /// <param name="propTemplateId">Prop template id.</param>
        /// <param name="variantId">Variant Id.</param>
        /// <param name="color">Color to apply.</param>
        /// <param name="localOnly">Set to `true` if you only want this skin to be applied locally.</param>
        void ApplyPropVariantColor(string propTemplateId, string variantId, Color color, bool localOnly);

        /// <summary>
        /// Allows to override application prop click behaviour
        /// </summary>
        IPropsInput Input { get; }

        /// <summary>
        /// Add content to a prop.
        /// The default way to add content to a prop, if you need something custom,
        /// see the <see cref="PropContentUpdateBuilder"/>.
        /// </summary>
        /// <param name="propId">Id of the target prop.</param>
        /// <param name="res">The resource to add.</param>
        /// <param name="source">The source where resource was added from.</param>
        void AddContent(string propId, IResource res, ResourceAddSource source = ResourceAddSource.Unknown);

        /// <summary>
        /// Request to Update prop thumbnails resolution.
        /// If room has specific settings to work on the lower resolution, resolution will be updated
        /// to the max supported size.
        /// </summary>
        /// <param name="propTemplateId">Target prop id.</param>
        /// <param name="resource">Target resource.</param>
        /// <param name="size">Desired resolution.</param>
        void UpdateThumbnailsResolution(string propTemplateId, IResource resource, ThumbnailSize size);
    }
}
