using System.Collections.Generic;

// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api
{
	public interface IStorylineFrameTimeline
	{
		///<summary>
		/// Length of the timeline in seconds.
		///</summary>
		float Length { get; set; }

		///<summary>
		/// Timeline events. Sorted by time ascending.
		///</summary>
		List<IStorylineFrameTimelineEvent> Events { get; }

		void SetAudioNarration(IResource resource, float preferredDuration);
		bool Parse(JSONData data);
		Dictionary<string, object> ToDictionary();
		bool HasAvatar();
		void CollectResourcesForDownload(List<IResource> downloadRes);
		void InsertEvent(IStorylineFrameTimelineEvent e);
		void InitAvatar();
        bool HasPropAnimationControl();
	}
}
