// Copyright Roomful 2013-2020. All rights reserved.

using UnityEngine;

namespace net.roomful.api
{
    public class UrlLink
    {
        public enum Type
        {
            URL,
            PDF
        }

        private string m_title;
        private string m_url;

        public UrlLink(string url = "", string title = "") {
            m_title = title;
            m_url = url;
            UrlType = Type.URL;
        }

        public void SetTitle(string title) {
            m_title = title;
        }

        public void SetUrl(string url) {
            m_url = url;
        }

        public void SetType(Type type) {
            UrlType = type;
        }

        public string Title {
            get => m_title;
            set => SetTitle(value);
        }

        public string Url {
            get => m_url;
            set => SetUrl(value);
        }

        public bool IsEmpty => string.IsNullOrEmpty(Url);

        public Type UrlType { get; private set; }

        public void Open() {
            if (string.IsNullOrEmpty(Title)) {
                Title = Url;
            }

            if (Roomful.Platform == RoomfulPlatform.Editor) {
                Application.OpenURL(Url);
            }
            else {
                Roomful.Native.ShowWebView(this);
            }
        }
    }
}
