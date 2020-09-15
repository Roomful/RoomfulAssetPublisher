using UnityEngine;

namespace net.roomful.assets
{
    internal interface IPropComponent
    {
        void PrepareForUpload();
        void RemoveSilhouette();

        void Update();

        PropComponentUpdatePriority UpdatePriority { get; }
        GameObject gameObject { get; }
    }
}