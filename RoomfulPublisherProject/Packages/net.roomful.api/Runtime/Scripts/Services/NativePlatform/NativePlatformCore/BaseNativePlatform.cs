using System;

namespace net.roomful.api.native
{
    public abstract class BaseNativePlatform
    {
        public event Action<ResourcePositionUpdate> OnPropResourcePositionUpdated;

        protected void InvokePropResourcePositionUpdated(ResourcePositionUpdate positionUpdate) {
            OnPropResourcePositionUpdated?.Invoke(positionUpdate);
        }
    }
}
