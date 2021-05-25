// Copyright Roomful 2013-2018. All rights reserved.

namespace net.roomful.api
{
    public class UserFacebookLink
    {
        public UserFacebookLink() { }

        public UserFacebookLink(bool linked, string linkId) {
            Linked = linked;
            LinkId = linkId;
        }

        public bool Linked { get; } = false;

        public string LinkId { get; } = string.Empty;
    }
}