using System;
using net.roomful.api.networks;

namespace net.roomful.api.colorization
{
    public interface IColorizationService
    {
        ColorizationScheme CurrentScheme { get; }
        
        void Setup(INetworksService networkService, IUsersService usersService);
        IColorizationUnsubscriber RegisterSubject(IColorizationSubject subject);
    }

    public interface IColorizationUnsubscriber : IDisposable { }

    public struct ColorizationSchemeContext
    {
        public bool UseNetworkColorization;
        public ColorizationSchemeType ColorizationSchemeType;
    }
}
