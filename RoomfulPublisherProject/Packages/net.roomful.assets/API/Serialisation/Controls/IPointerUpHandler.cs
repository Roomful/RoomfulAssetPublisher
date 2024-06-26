namespace net.roomful.assets.serialization
{
    public struct PointerUpArgs
    {
        public InputButton Button;
    }
    
    public interface IPointerUpHandler
    {
        void HandlePointerUp(PointerUpArgs args);
    }
}