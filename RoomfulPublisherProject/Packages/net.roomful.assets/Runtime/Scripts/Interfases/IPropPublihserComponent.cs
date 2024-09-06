using UnityEngine;

namespace net.roomful.assets
{
    interface IPropPublihserComponent
    {
        void PrepareForUpload();

        void Update();

        PropComponentUpdatePriority UpdatePriority { get; }
        GameObject gameObject { get; }
    }
}