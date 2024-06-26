using UnityEngine;

namespace net.roomful.api.media
{
    public struct TextureActivatedArgs
    {
        public string Guid;
    }
    
    public struct TextureSizeChangedArgs
    {
        public string Guid;
        public int Width;
        public int Height;
    }
    
    public struct TextureDeactivatedArgs
    {
        public string Guid;
    }
    
    public delegate void TextureActivated(TextureActivatedArgs args);
    public delegate void TextureDeactivated(TextureDeactivatedArgs args);
    public delegate void TextureSizeChanged(TextureSizeChangedArgs args);
    
    public interface IMediaSource// : IBindable
    {
        event TextureActivated OnTextureActivated;
        event TextureDeactivated OnTextureDeactivated;
        event TextureSizeChanged OnTextureSizeChanged;

        string Id { get; }
        bool IsBound { get; }
        bool HasValidData { get; }
        Texture Texture { get; }
        string TypeName { get; }

        void OnActivated();
        void OnDeactivated();
        void Release();
    }
}
