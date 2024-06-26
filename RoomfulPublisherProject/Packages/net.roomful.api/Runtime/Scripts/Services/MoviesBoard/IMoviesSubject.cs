using net.roomful.api.props;
using net.roomful.api.player.video;

namespace net.roomful.api.movies
{
    public enum State
    {
        Offline,
        Viewer,
        Presenter
    }
    
    public struct MoviesBundleUpdatedArgs { }

    public struct CurrentVideoChangedArgs
    {
        public Video Video;
    }
    
    public delegate void MoviesBundleUpdated(MoviesBundleUpdatedArgs args);
    public delegate void CurrentVideoChanged(CurrentVideoChangedArgs args);

    public interface IMoviesContext
    {
        event MoviesBundleUpdated OnMoviesBundleUpdated;
        event CurrentVideoChanged OnCurrentVideoChanged;
        
        IProp Source { get; }
        IVideoPlayer Player { get; }
        MoviesBundle Bundle { get; }
        Video CurrentVideo { get; }
        void Release();
    }
    
    public interface IMoviesSubject
    {
        IProp Owner { get; }
        IMoviesContext Context { get; }
        void CompleteBootstrap();
        void SetState(State state);
        void Dispose();
    }
}
