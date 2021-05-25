namespace net.roomful.api.avatars.emotions
{
    public enum AvatarEmotions
    {
        LAUGH, APPLAUD, NEGATIVE, POSITIVE, RAISE_HAND
    }
    
    public interface IInRoomAvatarsEmotionService
    {
        void PlayEmotion(AvatarEmotions emotion);

        string ConvertToAnimatorAnimation(AvatarEmotions emotion);
    }
}
