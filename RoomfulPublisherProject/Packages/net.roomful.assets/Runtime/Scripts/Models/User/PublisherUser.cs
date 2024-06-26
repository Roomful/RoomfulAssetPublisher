using System.Collections.Generic;

namespace net.roomful.assets
{
    public class PublisherUser
    {
        public string Id { get; }
        public string FullName { get; }

        public string CompanyName { get; }
        public string CompanyTitle { get; }
        
        public PublisherUser(Dictionary<string, object> userData)
        {
            Id = userData["id"].ToString();
            FullName = $"{userData["firstName"]} {userData["lastName"]}";
            CompanyName = userData["companyName"].ToString();
            CompanyTitle = userData["companyTitle"].ToString();
        }
    }
}
