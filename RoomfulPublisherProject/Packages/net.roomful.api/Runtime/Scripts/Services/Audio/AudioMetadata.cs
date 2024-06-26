// Copyright Roomful 2013-2020. All rights reserved.
using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.api.audio
{
    public class AudioMetadata
    {
        public string Title { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public string Album { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public float Duration { get; set; }

        public AudioMetadata() { }

        public AudioMetadata(string title, string artist, string album, string genre, float duration)
        {
            Title = title;
            Artist = artist;
            Album = album;
            Genre = genre;
            Duration = duration;
        }

        public AudioMetadata(JSONData data) {
            ParseTemplate(data);
        }

        public Dictionary<string, object> ToDictionary() {
            return new Dictionary<string, object> {
                { "title", Title },
                { "artist", Artist },
                { "album", Album },
                { "genre", Genre },
                { "durationFloat", Duration },
            };
        }

        private void ParseTemplate(JSONData audio) {
            if (audio.HasValue("title")) {
                Title = audio.GetValue<string>("title");
            }

            if (audio.HasValue("artist")) {
                Artist = audio.GetValue<string>("artist");
            }

            if (audio.HasValue("album")) {
                Album = audio.GetValue<string>("album");
            }

            if (audio.HasValue("genre")) {
                Genre = audio.GetValue<string>("genre");
            }

            if (audio.HasValue("durationFloat") &&  !Mathf.Approximately(0f, audio.GetValue<float>("durationFloat"))) {
                Duration = audio.GetValue<float>("durationFloat");
            }
            else {
                if (audio.HasValue("duration")) {
                    Duration = audio.GetValue<int>("duration");
                }
            }
        }
    }
}
