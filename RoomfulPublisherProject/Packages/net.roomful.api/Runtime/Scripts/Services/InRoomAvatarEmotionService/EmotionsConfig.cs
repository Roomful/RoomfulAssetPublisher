namespace net.roomful.api.avatars.emotions
{
    public static class EmotionsConfig
    {
        private static string[] EmotionsNameInAnimator = {"Laugh", "Applaud", "Negative", "Thumbs_Up", "HandsUp"};

        public static string ConvertToAnimatorAnimation(AvatarEmotions emotion) {
            return EmotionsNameInAnimator[(int) emotion];
        }
    }
}