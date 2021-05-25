using System;
using System.Collections.Generic;

// Copyright Roomful 2013-2019. All rights reserved.

namespace net.roomful.api
{
    public class ContentPickerParams
    {
        public Action<List<IResource>> Callback { get; set; }

        public ContentPickerMode PickerMode { get; set; } = ContentPickerMode.SortingPanel;
        public int MaxItemsCount { get; set; }          = -1;
        public bool AddToSortingTable { get; set; }     = true;
        public bool GenerateHistory { get; set; }       = true;
        public string[] Types { get; set; }             = s_defaultTypes;
        public string Title { get; set; }               = string.Empty;
        public string Parent { get; set; }              = string.Empty;//folderId
        public string DirectoryName { get; set; }       = string.Empty;
        public string ButtonUploadMessage { get; set; } = "to Roomful";
        public string PropId { get; set; }              = string.Empty;
        public string RoomToContributeTo { get; set; }  = string.Empty;
        public string UserId { get; set; }              = string.Empty;
        public string From { get; set; }                = string.Empty;
        public bool IsDefault = false;//TODO this one should go after native part refactor

        private static readonly string[] s_defaultTypes = {
            ContentType.Image.ToString(), ContentType.Audio.ToString(), ContentType.Video.ToString(),
            ContentType.Book.ToString(), ContentType.All.ToString()
        };

        public static ContentPickerParams GetImagePickerParams(int itemCount, Action<List<IResource>> callback, string buttonCaption, bool addToSortingTable) {
            var result = new ContentPickerParams();
            result.MaxItemsCount = itemCount;
            result.Types = new[] {ContentType.Image.ToString()};
            result.Callback = callback;
            result.AddToSortingTable = addToSortingTable;
            result.GenerateHistory = addToSortingTable;
            result.ButtonUploadMessage = buttonCaption;
            return result;
        }

        public static ContentPickerParams GetAvatarPickerParams(Action<List<IResource>> callback, string userId) {
            var result = new ContentPickerParams();
            result.MaxItemsCount = 1;
            result.Types = new[] { ContentType.Image.ToString() };
            result.Callback = callback;
            result.AddToSortingTable = false;
            result.GenerateHistory = false;
            result.PickerMode = ContentPickerMode.UserAvatar;
            result.ButtonUploadMessage = "to Profile";
            result.UserId = userId;
            return result;
        }

        public static ContentPickerParams GetCommentAttachmentPickerParams(int itemCount, Action<List<IResource>> callback,
            string buttonCaption) {
            var result = new ContentPickerParams();
            result.PickerMode = ContentPickerMode.Single;
            result.GenerateHistory = false;
            result.MaxItemsCount = itemCount;
            result.Types = new[] { ContentType.Image.ToString() };
            result.Callback = callback;
            result.AddToSortingTable = false;
            result.ButtonUploadMessage = buttonCaption;
            return result;
        }

        public static ContentPickerParams GetPropPickerParams(string propId, PropInvokeType propInvokeType) {
            var result = new ContentPickerParams();
            result.PickerMode = ContentPickerMode.EditProp;
            result.AddToSortingTable = false;
            result.PropId = propId;
            switch (propInvokeType) {
                case PropInvokeType.FileCabinet:
                    result.Types = new[] {ContentType.All.ToString()};
                    result.ButtonUploadMessage = "to Filecabinet";
                    break;
                case PropInvokeType.Bookshelf:
                    result.Types = new[] {ContentType.Book.ToString()};
                    result.ButtonUploadMessage = "to Bookshelf";
                    break;
                default:
                    result.Types = new[] {ContentType.Image.ToString(), ContentType.Video.ToString()};
                    result.ButtonUploadMessage = "to Manage Content";
                    result.From = "manage";
                    break;
            }
            return result;
        }
    }
}