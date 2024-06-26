using System;

namespace net.roomful.api.settings
{
    public interface IBuildSettingsViewDelegate<T>
    {
        void BuildView(T service, Action onComplete);
    }
}
