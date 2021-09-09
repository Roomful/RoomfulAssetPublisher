using System.Collections.Generic;
using net.roomful.api;

namespace net.roomful.api
{
    /// <summary>
    /// Represents users selection result.
    /// </summary>
    public readonly struct ContactsSelectionResult
    {
        /// <summary>
        /// True if selection was canceled.
        /// </summary>
        public bool SelectionCanceled { get; }

        /// <summary>
        /// Selected users list.
        /// </summary>
        public ICollection<IUserFriendTemplate> SelectedUsers { get; }

        public ContactsSelectionResult(bool selectionCanceled, ICollection<IUserFriendTemplate> selectedUsers) {
            SelectedUsers = selectedUsers;
            SelectionCanceled = selectionCanceled;
        }
    }
}
