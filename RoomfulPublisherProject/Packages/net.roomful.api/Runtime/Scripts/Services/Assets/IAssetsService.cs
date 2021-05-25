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

        IPropAssetsSearchRequestBuilder MakePropAssetsSearchBuilder(int offset, int size);

        void Search(IPropAssetsSearchRequestBuilder builder, Action<IReadOnlyCollection<IPropAssetTemplate>> callback);
    }
}
