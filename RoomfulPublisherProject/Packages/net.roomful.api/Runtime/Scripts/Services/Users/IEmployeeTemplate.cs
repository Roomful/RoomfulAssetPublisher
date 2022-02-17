
// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api {

    public interface IEmployeeTemplate {
        
        string Position { get; set; }
        string Company { get; set; }
        void UpdateTemplate(string position, string company);
    }
}
