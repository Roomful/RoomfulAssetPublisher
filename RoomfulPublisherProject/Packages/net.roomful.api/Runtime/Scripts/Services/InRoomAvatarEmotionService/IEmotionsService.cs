using System;

namespace net.roomful.api.avatars.emotions
{
    public struct UserReactionArgs
    {
        public string UserId;
        public string VideoChatId;
        public AvatarEmotions EmotionType;

        public override string ToString() {
            return $"UserId: {UserId}, VideoChatId: {VideoChatId}, EmotionType: {EmotionType}";
        }
    }

    public enum AvatarEmotions
    {
        LAUGH, APPLAUD, NEGATIVE, POSITIVE, RAISE_HAND
    }

    public interface IEmotionsService
    {
        bool DebugMode { get; }

        event Action<UserReactionArgs> OnUserReacted;
        // Make internal
        void PlayEmotion(AvatarEmotions emotion);
    }
}
