using System.Collections.Generic;
using net.roomful.api.props;

namespace net.roomful.api.movies
{
    public interface IBootstrapper
    {
        IEnumerable<IMoviesSubject> Subjects { get; }
        void Bootstrap(IPropComponent component);
        void Dispose(IPropComponent component);
    }
}