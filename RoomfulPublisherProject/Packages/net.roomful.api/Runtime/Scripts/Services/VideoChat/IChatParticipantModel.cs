using System.Collections.Generic;

namespace net.roomful.api
{
    /// <summary>
    /// Video chat participant.
    /// </summary>
    public interface IChatParticipantModel
    {
        IUserTemplateSimple User { get; }
        IEnumerable<IIdentityModel> IdentityModels { get; }
        IConferenceUserPermissions Permissions { get; }
        int SequentialNumber { get; }

        Dictionary<string, object> ToDictionary();
    }
}
