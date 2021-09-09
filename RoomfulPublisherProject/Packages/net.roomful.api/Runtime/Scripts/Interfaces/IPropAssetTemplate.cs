using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.api
{
    public interface IPropAssetTemplate : IAssetTemplate
    {
        bool IsValid { get; }
        PlacingType Placing { get; }
        PropInvokeType InvokeType { get; }
        List<ContentType> ContentTypes { get; }
        bool CanStack { get; }
        bool HasVariants { get; }

        /// <summary>
        /// Count of thumbnails component marked as logos.
        /// </summary>
        int LogoCount { get; }

        /// <summary>
        /// Count of thumbnails component marked as non logos.
        /// </summary>
        int ThumbnailCount { get; }
        float MaxScale { get; }
        float MinScale { get; }
        Vector3 Size { get; }
        List<string> ContentTypesStrings { get; }
        void SetCanStack(bool value);
    }
}
