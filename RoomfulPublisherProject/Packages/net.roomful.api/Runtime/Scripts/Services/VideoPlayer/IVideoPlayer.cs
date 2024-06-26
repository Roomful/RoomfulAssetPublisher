using System;
using System.Collections.ObjectModel;
using net.roomful.api.props;
using UnityEngine;

namespace net.roomful.api.player.video
{
    public enum RenderingMode
    {
        Canvas,
        Texture
    }
    
    public struct VideoPlayerContext
    {
        public string Name;
        public RenderingMode Mode;
        
        public Transform Container;
        public IProp Prop;

        public int ResourceIndex;
        public int InnerPropIndex;
        public int FocusPointIndex;

        public bool IsEmpty => Prop == null;
        
        /// <summary>
        /// When set to 'true', player won't be affected by a prop update.
        /// This is useful if provided playable entries do not come from a prop content.
        /// </summary>
        public bool SkipRebindOnUpdate;
    }

    public struct VideoPlayableEntryChangedArgs
    {
        public IVideoPlayableEntry PreviousEntry;
        public IVideoPlayableEntry CurrentEntry;
    }
    
    public interface IVideoPlayer: IPlayer
    {
        event Action OnSourceTextureUpdated;
        event Action<VideoPlayableEntryChangedArgs> OnPlayableEntryChanged;
        
        VideoPlayerContext Context { get; }
        IVideoPlayableEntry CurrentPlayableEntry { get; }
        ReadOnlyCollection<IVideoPlayableEntry> PlaybackQueue { get; }

        void BindTo(VideoPlayerContext ctx);
        void Play(IVideoPlayableEntry playableEntry);
        void Play(ReadOnlyCollection<IVideoPlayableEntry> playbackQueue, int startIndex = 0);

        Texture GetSourceTexture();
    }
}
