namespace net.roomful.api
{
    public class SwipeEventData : AbstractInputEventData
    {
        // Please take to account that this Enum totally copied from TouchKit.TKSwipeDirection
        [System.Flags]
        public enum SwipeDirection
        {
            Left = 1 << 0,
            Right = 1 << 1,
            Up = 1 << 2,
            Down = 1 << 3,

            UpLeft = 1 << 4,
            DownLeft = 1 << 5,
            UpRight = 1 << 6,
            DownRight = 1 << 7,

            Horizontal = Left | Right,
            Vertical = Up | Down,
            Cardinal = Horizontal | Vertical,

            DiagonalUp = UpLeft | UpRight,
            DiagonalDown = DownLeft | DownRight,
            DiagonalLeft = UpLeft | DownLeft,
            DiagonalRight = UpRight | DownRight,
            Diagonal = DiagonalUp | DiagonalDown,

            RightSide = Right | DiagonalRight,
            LeftSide = Left | DiagonalLeft,
            TopSide = Up | DiagonalUp,
            BottomSide = Down | DiagonalDown,

            All = Horizontal | Vertical | Diagonal
        }
        
        private static readonly SwipeEventDataPool s_pool = new SwipeEventDataPool();
        
        public SwipeDirection Direction { get; private set; }

        public void Init(SwipeEventData.SwipeDirection direction) {
            Direction = direction;
        }
        
        public static SwipeEventDataPool.PooledObject GetPooled(SwipeEventData.SwipeDirection direction) {
            return s_pool.Get(direction);
        }
    }
    
    internal class SwipeEventDataPool : EventsPool<SwipeEventData>
    {
        public PooledObject Get(SwipeEventData.SwipeDirection direction) {
            var item = base.Get();
            item.Init(direction);
            return new PooledObject(item, this);
        }
    }
}