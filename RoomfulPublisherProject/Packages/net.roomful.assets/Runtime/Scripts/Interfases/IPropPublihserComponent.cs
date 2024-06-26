using UnityEngine;

namespace net.roomful.assets
{
    interface IPropPublihserComponent
    {
        void PrepareForUpload();
        void RemoveSilhouette();

        void Update();

        PropComponentUpdatePriority UpdatePriority { get; }
        GameObject gameObject { get; }
    }
}