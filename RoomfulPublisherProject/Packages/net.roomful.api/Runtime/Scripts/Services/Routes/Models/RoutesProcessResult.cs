namespace net.roomful.api
{
    public class RoutesProcessResult
    {
        public RoutePermission MaxPermission { get; }
        public RoutesProcessStatus Status { get; }

        public RoutesProcessResult(RoutesProcessStatus status, RoutePermission maxPermission) {
            Status = status;
            MaxPermission = maxPermission;
        }

    }
}
