// Copyright Roomful 2013-2019. All rights reserved.

using System.Collections.Generic;
using System.Linq;

namespace net.roomful.api
{
    /// <summary>
    /// Describes Content picker params.
    /// Those params are used to perform an upload in the native platform.
    /// </summary>
    public class ContentPickerParams
    {
        /// <summary>
        /// Defines UI Mode.
        /// </summary>
        public ContentPickerMode PickerMode { get; private set; }

        /// <summary>
        /// Max items count available for upload.
        /// </summary>
        public int MaxItemsCount { get; private set; }

        /// <summary>
        /// Browser window title.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Types that are allowed to be uploaded.
        /// </summary>
        public string[] Types { get; private set; }

        /// <summary>
        /// Picker popup back button description.
        /// </summary>
        public string BackMessage { get; private set; }

        /// <summary>
        /// Route will be set when coming back from the popup.
        /// </summary>
        public string BackRoute { get; private set; }

        /// <summary>
        /// Browser url that can be used for upload resources.
        /// This url is used by a platforms that will relay on opening web browser
        /// To perform an upload rather then relaying on own upload implementation.
        /// </summary>
        public UrlLink UploadUrl { get; private set; }

        /// <summary>
        /// Room Id for contributions.
        /// Only available for <see cref="ContentPickerMode.SortingPanel"/>
        /// and <see cref="ContentPickerMode.EditProp"/> modes.
        /// </summary>
        public string RoomId { get; private set; } = string.Empty;

        /// <summary>
        /// Prop Id for contributions.
        /// Only available for <see cref="ContentPickerMode.EditProp"/> mode.
        /// </summary>
        public string PropId { get; set; } = string.Empty;

        /// <summary>
        /// Target User Id for contributions. Only available for <see cref="ContentPickerMode.UserAvatar"/> mode.
        /// </summary>
        public string UserId { get; private set; } = string.Empty;
        private static string BaseAPIUrl => $"{Roomful.WebAPIUrl}{Roomful.Session.Id}";

        /// <summary>
        /// Create picker params for uploading into the sorting table.
        /// </summary>
        /// <param name="backRoute">Route that will be set for an application when modal window is closed.</param>
        /// <param name="mobileApiUrl">
        /// This url is used by a platforms that will relay on opening web browser
        /// To perform an upload rather then relaying on own upload implementation.
        /// </param>
        /// <param name="targetRoomId">
        /// If target room is set,
        /// resources will be uploaded to the room contribute.
        /// </param>
        public static ContentPickerParams MakeSortingTablePickerParams(string backRoute, string mobileApiUrl, string targetRoomId) {
            var pickerParams = new ContentPickerParams();
            pickerParams.PickerMode = ContentPickerMode.SortingPanel;
            pickerParams.MaxItemsCount = 50;
            pickerParams.Types = new[] { ContentType.All.ToString() };
            pickerParams.BackMessage = "to Room";
            pickerParams.BackRoute = backRoute;
            pickerParams.Title = "Upload Content";

            if (!string.IsNullOrEmpty(targetRoomId)) {
                pickerParams.RoomId = targetRoomId;
            }

            pickerParams.UploadUrl = new UrlLink($"{BaseAPIUrl}/{mobileApiUrl}", pickerParams.Title);
            return pickerParams;
        }

        public static ContentPickerParams MakeEnvironmentUploadParams(string backRoute, string mobileApiUrl) {
            var pickerParams = new ContentPickerParams();
            pickerParams.PickerMode = ContentPickerMode.SortingPanel;
            pickerParams.MaxItemsCount = 1;

            pickerParams.Types = new[] { ContentType.Image.ToString(), ContentType.Video.ToString() };
            pickerParams.BackMessage = "to Settings";
            pickerParams.BackRoute = backRoute;
            pickerParams.Title = "Upload Environment";

            pickerParams.UploadUrl = new UrlLink($"{BaseAPIUrl}/{mobileApiUrl}", pickerParams.Title);
            return pickerParams;
        }

        public static ContentPickerParams MakePropContentParams(string backRoute, string mobileApiUrl, string propId, string roomId, List<ContentType> contentTypes) {
            var pickerParams = new ContentPickerParams();
            pickerParams.PickerMode = ContentPickerMode.EditProp;
            pickerParams.MaxItemsCount = 50;
            pickerParams.RoomId = roomId;
            pickerParams.PropId = propId;

            pickerParams.Types = contentTypes.Select(type => type.ToString()).ToArray();
            pickerParams.BackMessage = "to Prop";
            pickerParams.BackRoute = backRoute;
            pickerParams.Title = "Upload Content";

            pickerParams.UploadUrl = new UrlLink($"{BaseAPIUrl}/{mobileApiUrl}", pickerParams.Title);
            return pickerParams;
        }

        /// <summary>
        /// Create picker params for uploading user avatar.
        /// </summary>
        /// <param name="backRoute">Route that will be set for an application when modal window is closed.</param>
        /// <param name="mobileApiUrl">
        /// This url is used by a platforms that will relay on opening web browser
        /// To perform an upload rather then relaying on own upload implementation.
        /// </param>
        /// <param name="userId">
        /// Target user id.
        /// </param>
        public static ContentPickerParams MakeAvatarPickerParams(string backRoute, string userId, string mobileApiUrl) {
            var pickerParams = new ContentPickerParams();
            pickerParams.PickerMode = ContentPickerMode.UserAvatar;
            pickerParams.MaxItemsCount = 1;
            pickerParams.Types = new[] { ContentType.Image.ToString() };
            pickerParams.BackMessage = "to Profile";
            pickerParams.BackRoute = backRoute;
            pickerParams.UserId = userId;
            pickerParams.Title = "Upload Avatar";

            pickerParams.UploadUrl = new UrlLink($"{BaseAPIUrl}/{mobileApiUrl}", pickerParams.Title);

            return pickerParams;
        }
    }
}
