
// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api {

    public class EmployeeTemplate : IEmployeeTemplate {
        
        public string Position { get; set; }
        public string Company { get; set; }
        
        public void UpdateTemplate(string position, string company) {
            Position = position;
            Company = company;
        }
    }
}
