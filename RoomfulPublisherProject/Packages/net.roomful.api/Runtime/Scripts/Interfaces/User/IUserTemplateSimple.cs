using System.Collections.Generic;

namespace net.roomful.api
{
    public interface IUserTemplateSimple : ITemplate
    {
        Dictionary<string, object> ToDictionary(bool includeBase64 = true);
        string FirstName { get; }
        string LastName { get; }
        string FullName { get; }
        string AvatarURL { get; }
        string CompanyName { get; }
        string CompanyTitle { get; }
        Avatar3DInfo Avatar3DInfo { get; }
        int Counter { get; }
        IUserRoomPosition UserRoomPosition { get; }
        string GetShortenName();
        void SetFirstName(string name);
        void SetLastName(string lastName);
        void SetCompany(string companyName, string companyTitle);
        void UseAsRoomVisitor();
    }
}