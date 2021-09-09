// Copyright Roomful 2013-2019. All rights reserved.

namespace net.roomful.api.props
{
    public interface IPropComponent
    {
        void Init(int componentIndex);
        void Refresh();
        void PropScaleChanged();
    }
}
