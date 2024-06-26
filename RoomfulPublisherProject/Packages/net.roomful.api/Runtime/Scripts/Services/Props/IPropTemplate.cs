using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.api.props
{
    /// <summary>
    /// Represents server side prop info.
    /// </summary>
    public interface IPropTemplate : IRoomContentTemplate
    {
        Vector3 Rotation { get; }
        IPropAssetTemplate Asset { get; }
        string Title { get; }
        string Description { get; }
        float Scale { get; }
        Vector3 Position { get; }

        /// <summary>
        /// Is text chat is available for this prop
        /// </summary>
        bool IsAllowTextChat { get; }
        string StyleId { get; }
        string PanelId { get; }
        ///<summary>
        /// Id of Parent prop, string.Empty by default
        ///</summary>
        string ParentId { get; }
        PropInvokeType Type { get; }
        ContentPickerType ContentType { get; }
        bool ParticipatesInNextPrevWalk { get; }

        /// <summary>
        /// Contains applied skins metadata
        /// </summary>
        IPropSkinsMetadata SkinsMetadata { get; }

        /// <summary>
        ///  Defines how prop info (title & description) is displayed.
        ///  The value is set by a user
        /// </summary>
        UserDefinedPropInfoDisplayType UserDefinedInfoDisplayType { get; }
        
        /// <summary>
        /// This value should be used to determine how info is actually displayed.
        /// It will take into account <see cref="UserDefinedInfoDisplayType"/>  and other template values.
        /// </summary>
        PropInfoDisplayType InfoDisplayType { get; }

        int ContentAmount { get; }

        /// <summary>
        /// If not 'null', current prop will fetch content from source prop
        /// </summary>
        IContentSourceLink ContentSourceLink { get; }

        /// <summary>
        /// Prop Content.
        /// </summary>
        IReadOnlyList<IResource> Content { get; }

        /// <summary>
        ///  Types of the prop.
        /// </summary>
        IList<string> PropTypes { get; }

        /// <summary>
        /// Skins applied to a prop. Where key is variant Id and value is skin Id.
        /// </summary>
        IReadOnlyDictionary<string, string> Skins { get; }

        IReadOnlyList<IPropEventsActionsModel> PropEventsActions { get; }

        /// <summary>
        /// Get prop custom param.
        /// </summary>
        /// <param name="paramKey">Custom param key.</param>
        /// <returns>Returns custom param object or `null` if param with provided key does not exists.</returns>
        object GetParam(string paramKey);

        /// <summary>
        /// Method will create new Dictionary that represents prop data.
        /// </summary>
        /// <returns>Dictionary with prop info</returns>
        Dictionary<string, object> ToDictionary();

        /// <summary>
        /// Method will create new Dictionary that represents prop data,
        /// but prop content key will be excluded.
        /// </summary>
        /// <returns>Dictionary with prop info</returns>
        Dictionary<string, object> ToDictionaryExcludeContent();
    }
}
