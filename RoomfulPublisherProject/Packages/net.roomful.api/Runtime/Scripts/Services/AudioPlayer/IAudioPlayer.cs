using System;
using net.roomful.api.props;
using UnityEngine;

namespace net.roomful.api.player
{
    public interface IAudioPlayableEntry : IPlayableEntry { }

    public struct AudioPlayerContext
    {
        public string Name;
        
        public Transform Container;
        public IProp Prop;

        public int ResourceIndex;
        public int InnerPropIndex;
        public int FocusPointIndex;

        public bool IsEmpty => Prop == null;
    }
    
    public struct AudioPlayableEntryChangedArgs
    {
        public IAudioPlayableEntry PreviousEntry;
        public IAudioPlayableEntry CurrentEntry;
    }
    
    public interface IAudioPlayer : IPlayer
    {
        event Action<AudioPlayableEntryChangedArgs> OnPlayableEntryChanged;
        AudioPlayerContext Context { get; }
        IAudioPlayableEntry CurrentPlayableEntry { get; }

        void BindTo(AudioPlayerContext ctx);
        void Play(IAudioPlayableEntry playableEntry);
    }
}
