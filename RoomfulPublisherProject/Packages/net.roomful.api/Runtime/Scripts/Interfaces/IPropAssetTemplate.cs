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
        int LogoCount { get; }
        int ThumbnailCount { get; }
        float MaxScale { get; }
        float MinScale { get; }
        Vector3 Size { get; }
        List<string> ContentTypesStrings { get; }
        void SetCanStack(bool value);
    }
}