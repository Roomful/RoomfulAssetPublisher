// Copyright Roomful 2013-2019. All rights reserved.

namespace net.roomful.api
{
    public abstract class AbstractInputEventData
    {
        public InputEventPhase Phase { get; private set; }

        internal void Init(InputEventPhase phase) {
            Phase = phase;
        }
    }
}
