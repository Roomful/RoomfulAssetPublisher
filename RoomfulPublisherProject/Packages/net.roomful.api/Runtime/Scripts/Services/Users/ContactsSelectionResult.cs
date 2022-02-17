using System.Collections.Generic;

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
        public ICollection<IUserTemplateSimple> SelectedUsers { get; }

        public ContactsSelectionResult(bool selectionCanceled, ICollection<IUserTemplateSimple> selectedUsers) {
            SelectedUsers = selectedUsers;
            SelectionCanceled = selectionCanceled;
        }
    }
}
