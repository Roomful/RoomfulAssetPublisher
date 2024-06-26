using System;
using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.api.assets
{
    public interface IAssetsService
    {
        /// <summary>
        /// Assets loading queue.
        /// </summary>
        IAssetsQueue Queue { get; }

        /// <summary>
        /// Makes gameobject asset area as disabled.
        /// </summary>
        /// <param name="gameObject">target gameobject.</param>
        void MarkAsDisableArea(GameObject gameObject);

        /// <summary>
        /// Makes gameobject as stand area.
        /// </summary>
        /// <param name="gameObject">target gameobject.</param>
        void MarkAsStandArea(GameObject gameObject);

        /// <summary>
        /// Gameobject won't be taken into account when calculating bounds.
        /// </summary>
        /// <param name="gameObject">target gameobject.</param>
        void IgnoreBounds(GameObject gameObject);

        /// <summary>
        /// Creates new instance of the assets search request builder.
        /// See <see cref="IPropAssetsSearchRequestBuilder"/>
        /// </summary>
        /// <param name="offset">Search offset.</param>
        /// <param name="size">Max size of the returned assets models.</param>
        /// <returns>New <see cref="IPropAssetsSearchRequestBuilder"/> instance.</returns>
        IPropAssetsSearchRequestBuilder MakePropAssetsSearchBuilder(int offset, int size);

        /// <summary>
        /// Performs asset search.
        /// </summary>
        /// <param name="builder">Assets search data.</param>
        /// <param name="callback">Search completed callback.</param>
        void Search(IPropAssetsSearchRequestBuilder builder, Action<IReadOnlyCollection<IPropAssetTemplate>> callback);
    }
}
