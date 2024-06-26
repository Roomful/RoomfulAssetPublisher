using net.roomful.api.props;
using UnityEngine;

namespace net.roomful.api.presentation.board
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
    
    public struct SourceBindArgs { }
    public delegate void SourceBind(SourceBindArgs args);
    
    public struct SourceUnbindArgs { }
    public delegate void SourceUnbind(SourceUnbindArgs args);

    public interface IBindable
    {
        event SourceBind OnSourceBind;
        event SourceUnbind OnSourceUnbind;
    }

    public interface IMediaSource : IBindable
    {
        event TextureActivated OnTextureActivated;
        event TextureDeactivated OnTextureDeactivated;
        event TextureSizeChanged OnTextureSizeChanged;

        string Id { get; }
        bool IsBound { get; }
        bool HasValidData { get; }
        Texture Texture { get; }
        string TypeName { get; }

        void Bind();
        void Unbind();
        void Release();
    }

    public interface IMediaTarget
    {
        IMediaSource ActiveSource { get; }
        void SetSource(IMediaSource source);
        void ReleaseSource(IMediaSource source);
    }
    
    public interface IPresentationSubject
    {
        IProp Owner { get; }
        IMediaTarget ShareScreen { get; }
        
        void CompleteBootstrap();
        void Dispose();
    }
}