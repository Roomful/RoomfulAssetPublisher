using UnityEngine;

namespace net.roomful.assets.serialization
{
    public class AvatarPoseData : MonoBehaviour, IRecreatableOnLoad
    {
        public AvatarAnimationData MaleAnimationClip;
        public AvatarAnimationData FemaleAnimationClip;
    }
}
