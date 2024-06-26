using System;
using UnityEngine;

namespace net.roomful.api.avatars.emotions
{
    public enum ServiceCommandType
    {
        Undefined = 0,
            
        StopCustomAnimations = -10,
            
        StopCrowdDance = -1,
        StartCrowdDance = 1,
    }
    
    public struct UserReactionArgs
    {
        public string RoomId;
        public string UserId;
        /// <summary>
        /// Not used atm, but transported via server for further purposes
        /// </summary>
        public string VideoChatId;
        public int Emotion;
        public string Bundle;
        
        public ServiceCommandType ServiceCommand;

        public override string ToString() {
            return $"UserId: {UserId}, VideoChatId: {VideoChatId}, EmotionIndex: {Emotion}, Bundle: {Bundle}, ServiceCommand: {ServiceCommand}";
        }
    }

    public static class UserReactionArgsExtensions
    {
        public static bool IsEmotion(this UserReactionArgs @this) {
            return !@this.IsServiceAnimation() && string.IsNullOrEmpty(@this.Bundle);
        }
        
        public static bool IsServiceAnimation(this UserReactionArgs @this) {
            return @this.ServiceCommand != ServiceCommandType.Undefined;
        }
    }

    public struct PanelArgs
    {
        public readonly Texture2D Icon;
        public readonly RectTransform Content;
        
        public PanelArgs(Texture2D icon, RectTransform content) {
            Icon = icon;
            Content = content;
        }
    }
    
    [Serializable]
    public struct EmotionData
    {
        public AvatarEmotions Emotion;
        public Texture2D Icon;
        public float Cooldown;
    }

    public enum AvatarEmotions
    {
        // Conference
        Laugh = 0, 
        Applaud = 1, 
        Negative = 2, 
        Thumbs_Up = 3, 
        HandsUp = 4,
        
        // Presentation
        Presentation_Right = 5,
        Presentation_Left = 6,
        Talk = 7,
        Hello = 8,
        
        Talk2 = 9,
        Talk3 = 10,
        Talk4 = 11,
        Talk5 = 12,
        Talk6 = 13,
    }

    public interface IEmotionConfig
    {
        bool TryGetEmotionData(AvatarEmotions emotion, out EmotionData result);
    }

    public interface IEmotionsService
    {
        event Action<UserReactionArgs> OnUserReacted;
        event Action<UserReactionArgs> OnLocalUserEmotionRequest;
        
        IEmotionConfig Config { get; }

        void DispatchEmotionMessage(string userId, int emotionId, string bundle, int serviceCommand);
        
        void PlayAnimation(int animation, string bundle);

        void PlayServiceAnimation(ServiceCommandType serviceCommand);

        void GetRoomPanelArgs(Action<PanelArgs> onComplete);
    }
}
