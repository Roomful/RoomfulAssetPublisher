﻿// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api.room
{
    /// <summary>
    /// Possible Room UI Modes
    /// </summary>
    public enum RoomUIMode
    {
        Undefined = -1,
        /// <summary>
        /// Default room mode,
        /// where we see a burger menu (depending on UI Controller, which differs for network and application type)
        /// </summary>
        SimpleView = 0,
        RoomBuilder = 2,
        DecoratorEditor = 3,
        SingleObjectEditor = 4,
        /// <summary>
        /// Add photos panel UI mode
        /// </summary>
        SortingList = 6,

        /// <summary>
        /// For creating room thumbnail (snapshot)
        /// </summary>
        Settings = 8,
        QuestionnaireEditor = 10,
        StoryLineEditor = 13,
        StoryLinePlayer = 16,
        StorylineSidePanel = 15,
        InvitationPanel = 17,
        StoryLineWizard = 18,
        RoomInfo = 20,
        HelpMenu = 21,
        MultiObjectEditor = 25,

        // Mode for any custom state of UI
        EmptyUIMode
    }
}
