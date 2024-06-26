using System.Collections.Generic;
using net.roomful.api.props;

namespace net.roomful.api.presentation.board
{
    public interface IBootstrapper
    {
        IEnumerable<IPresentationSubject> Subjects { get; }
        void Bootstrap(IPropComponent component);
        void Dispose(IPropComponent component);
    }
}