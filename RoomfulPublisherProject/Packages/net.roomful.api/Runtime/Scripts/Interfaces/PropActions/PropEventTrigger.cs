// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api {

 	/// <summary>
 	/// Necessary for action events behavior in the prop settings
 	/// </summary>
    public enum PropEventTrigger {
		OnClick,
		OnUpdate,
        OnClickNext,
        OnClickPrev,
		OnStartPlayVideo,
		OnPauseVideo,
		OnFinishPlayVideo,
		OnPlayComments,
		OnClickRequestThoughts,
		OnClickWriteComments,
	    OnClickReactions,
		OnChangeChannel,
		OnPropExit
    }
}
