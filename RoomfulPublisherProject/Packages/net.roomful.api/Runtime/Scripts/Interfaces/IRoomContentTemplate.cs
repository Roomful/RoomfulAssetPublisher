using System;

namespace net.roomful.api {

    public interface IRoomContentTemplate : ITemplate {

        DateTime LastUpdate  { get; set; }
        void SetId(string newId);
    }
}


