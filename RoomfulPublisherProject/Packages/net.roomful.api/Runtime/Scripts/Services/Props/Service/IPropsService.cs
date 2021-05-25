using System;
using System.Collections.Generic;

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
        /// See the <see cref="IProp.IsReadyToUse"/>.
        /// </summary>
        event Action<IProp> OnPropCreated;

        /// <summary>
        /// Event fired when prop is loaded and ready for all interaction. See <see cref="IProp.IsReadyToUse"/>.
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
        void UpdatePropPropContent(string propTemplateId, PropContentUpdateBuilder contentUpdateBuilder, bool supportUndo = true);

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
        void ServerCreateProp(string propTemplateId, bool supportUndo = true);

        /// <summary>
        /// Loads prop asset by asset id.
        /// Note: if asset with such id does not exists on a server,
        /// <see cref="callback"/> will contain `null` object.
        /// </summary>
        /// <param name="assetId">Prop asset id.</param>
        /// <param name="callback">Callback fired when asset is loaded.</param>
        void GetPropAsset(string assetId, Action<IPropAssetTemplate> callback);

        /// <summary>
        /// Allows to override application prop click behaviour
        /// </summary>
        IPropsInput Input { get; }
    }
}
