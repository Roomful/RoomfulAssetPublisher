namespace net.roomful.assets.serialization
{
    public enum InputButton
    {
        Left = 0,
        Right = 1,
        Middle = 2
    }
    
    public struct PointerDownArgs
    {
        public InputButton Button;
    }
    
    public interface IPointerDownHandler
    {
        void HandlePointerDown(PointerDownArgs args);
    }
}