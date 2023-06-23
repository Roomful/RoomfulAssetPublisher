using UnityEngine;

namespace net.roomful.api
{
    public struct AudioSourceParams
    {
        public bool Valid;
        
        public bool Spatialize;
        public bool Loop;
        public float SpatialBlend;
        public float Volume;
        public Transform Parent;
        public Vector3 LocalPosition;
    }
    
    public static class AudioSourceParamsExtensions
    {
        public static void ApplyParams(this AudioSource @this, AudioSourceParams param) {
            @this.spatialize = param.Spatialize;
            @this.loop = param.Loop;
            @this.spatialBlend = param.SpatialBlend;
            @this.volume = param.Volume;
            @this.transform.SetParent(param.Parent);
            @this.transform.localPosition = param.LocalPosition;
        }

        public static AudioSourceParams GetParams(this AudioSource @this) {
            return new AudioSourceParams {
                Valid = true,
                
                Spatialize = @this.spatialize,
                Loop = @this.loop,
                SpatialBlend = @this.spatialBlend,
                Volume = @this.volume,
                Parent = @this.transform.parent,
                LocalPosition =  @this.transform.localPosition
            };
        }
    }
}