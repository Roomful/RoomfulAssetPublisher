using UnityEngine;

namespace net.roomful.api.props
{
    /// <summary>
    /// Prop skin metadata
    /// </summary>
    public interface IPropSkinsMetadata
    {
        Color GetUserDefinedVariantColor(string variantId);
        bool HasCustomColorForVariant(string variantId);
    }
}
