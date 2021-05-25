using System;

namespace net.roomful.api.zoom
{
    public class NextPreviousBehaviourOverrideContext
    {
        public readonly string Name;

        public readonly Action<string> NextClickAction;
        public readonly Action<string> PreviousClickAction;
        public readonly Func<string, bool> NextButtonVisibilityDelegate;
        public readonly Func<string, bool> PreviousButtonVisibilityDelegate;

        public NextPreviousBehaviourOverrideContext(string name, Action<string> nextAction, Action<string> previousAction,
            Func<string, bool> nextButtonVisibilityDelegate, Func<string, bool> previousButtonVisibilityDelegate) {

            Name = name;
            NextClickAction = nextAction;
            PreviousClickAction = previousAction;
            NextButtonVisibilityDelegate = nextButtonVisibilityDelegate;
            PreviousButtonVisibilityDelegate = previousButtonVisibilityDelegate;
        }
    }
}