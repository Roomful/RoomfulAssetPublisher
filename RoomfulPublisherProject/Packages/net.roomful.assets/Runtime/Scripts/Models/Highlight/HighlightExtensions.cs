using HighlightPlus;

namespace net.roomful.assets.serialization
{
    public static class HighlightExtensions
    {
        public static void Apply(this HighlightEffect @this, Highlight highlight)
        {
            if (highlight != null && highlight.Profile != null)
            {
                @this.Apply(highlight.Profile);
            }
        }

        public static void Apply(this HighlightEffect @this, HighlightProfile profile)
        {
            @this.profile = profile;
            @this.profile.Load(@this);
        }
    }
}
