using net.roomful.api.props;
using UnityEngine;

namespace net.roomful.assets.serialization
{
    public class MoviesBoard : PropComponent, IRecreatableOnLoad
    {
        [Header("UI")]
        public Canvas UiCanvas;
        public RectTransform CompositionSelectorContainer;
        public RectTransform PlaybackEntryTitleContainer;
        public RectTransform VideoPlayerControlsContainer;
        public RectTransform VideoStreamingCanvasContainer;
        public RectTransform ParticipantsViewContainer;
        
        [Header("Action Buttons")]
        public ActionButton PlayButton;
        public ActionButton PauseButton;
        public ActionButton StopButton;
        public ActionButton GoToTargetButton;

        protected virtual void OnDestroy() { }
        
    }
}