namespace net.roomful.api.props
{
    public interface ILongClickEvent : IClickEvent
    {
        PropClickContext Context { get; }
    }
    
    public class LongClickEvent : BaseEvent<PropClickContext>, ILongClickEvent
    {
        
    }
    
    public interface IPropLongClickHandler
    {
        void OnPropLongClicked(ILongClickEvent e);
    }
}
