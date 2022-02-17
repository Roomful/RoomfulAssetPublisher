namespace net.roomful.api
{
    public readonly struct RouteHandleResult
    {
        public RouteHandleState State { get; }
        public RoutePermission Permission { get; }

        private RouteHandleResult(RouteHandleState state, RoutePermission routePermission) {
            State = state;
            Permission = routePermission;
        }

        public static RouteHandleResult SuccessAndPublic { get; } =
            new RouteHandleResult(RouteHandleState.Success, RoutePermission.Public);

        public static RouteHandleResult SuccessAndPrivate { get; } =
            new RouteHandleResult(RouteHandleState.Success, RoutePermission.Private);

        public static RouteHandleResult Failed { get; } =
            new RouteHandleResult(RouteHandleState.Failed, RoutePermission.Public);
    }
}
