using System;
using System.Collections.Generic;

namespace net.roomful.api {

    public interface IPropEventsActionsHandler {
        
        bool IsValid();
        List<IPropEventsActionsHandler> FilterQueue(List<IPropEventsActionsHandler> eventsQueue);
        void Run(Action callback);
    }
}