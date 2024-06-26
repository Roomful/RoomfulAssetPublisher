namespace net.roomful.api
{
    public class NumberEventData : AbstractInputEventData
    {
        public int Number { get; private set; }
        public bool Keypad { get; private set; }

        public void Bind(int number, bool keypad, InputEventPhase phase)
        {
            Number = number;
            Keypad = keypad;
            Init(phase);
        }
    }
    
    public class NumberEventDataPool : EventsPool<NumberEventData>
    {
        public PooledObject Get(int number, bool keypad, InputEventPhase phase) {
            var item = base.Get();
            item.Bind(number, keypad, phase);
            return new PooledObject(item, this);
        }
    }
}
