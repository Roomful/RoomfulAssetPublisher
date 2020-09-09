﻿using UnityEngine;

namespace net.roomful.assets
{
    internal interface IPropComponent
    {
        void PrepareForUpload();
        void RemoveSilhouette();

        void Update();

        Priority UpdatePriority { get; }
        GameObject gameObject { get; }
    }
}