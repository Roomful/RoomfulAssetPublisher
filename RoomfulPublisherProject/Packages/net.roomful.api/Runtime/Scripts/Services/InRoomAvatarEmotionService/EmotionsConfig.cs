using System.Collections.Generic;

namespace net.roomful.api.avatars.emotions
{
    public static class AvatarEmotionsExtension
    {
        private static readonly Dictionary<AvatarEmotions, string> s_cachedAnimationNames = new Dictionary<AvatarEmotions, string>();

        public static string ToParameterName(this AvatarEmotions emotion) {
            if (!s_cachedAnimationNames.TryGetValue(emotion, out var animationName)) {
                animationName = emotion.ToString();
                s_cachedAnimationNames[emotion] = animationName;
            }

            return animationName;
        }
    }
}