using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

// Copyright Roomful 2013-2018. All rights reserved.

namespace net.roomful.api
{
    public class JSONData
    {
        private string m_rawData = string.Empty;

        public JSONData(string data) {
            try {
                Data = Json.Deserialize(data) as Dictionary<string, object>;
                m_rawData = data;
                IsValid = true;
            }
            catch (Exception ex) {
                Debug.LogError(ex.Message);
                Debug.LogError("Can't parse JSONData out of: " + data);
            }
        }

        public JSONData(object data) {
            try {
                Data = (Dictionary<string, object>) data;
                IsValid = true;
            }
            catch (Exception ex) {
                Debug.LogError(ex.Message);
                Debug.LogError("Can't parse JSONData out of: " + data);
            }
        }

        public bool HasValue(params string[] keys) {
            var dict = Data;
            for (var i = 0; i < keys.Length - 1; i++) {
                dict = (Dictionary<string, object>) dict[keys[i]];
            }

            var valueKey = keys[keys.Length - 1];

            if (dict.ContainsKey(valueKey)) {
                return dict[valueKey] != null;
            }

            return false;
        }

        public T GetValue<T>(params string[] keys) {
            T value;
            var dict = Data;
            for (var i = 0; i < keys.Length - 1; i++) {
                dict = (Dictionary<string, object>) dict[keys[i]];
            }

            var valueKey = keys[keys.Length - 1];
            var data = dict[valueKey];

            if (typeof(T) == typeof(DateTime)) {
                var dateString = Convert.ToString(data);
                var parsed = TryParseRfc3339(dateString, out var date);
                if (!parsed) {
                    Debug.LogWarning("Date Parsing failed: " + dateString);
                }

                value = (T) Convert.ChangeType(date, typeof(T));
            }
            else {
                value = (T) Convert.ChangeType(data, typeof(T));
            }

            return value;
        }

        private static string[] s_rfc3339Formats = new string[0];

        private static string[] DateTimePatterns {
            get {
                if (s_rfc3339Formats.Length > 0) {
                    return s_rfc3339Formats;
                }

                s_rfc3339Formats = new string[11];

                // Rfc3339DateTimePatterns
                s_rfc3339Formats[0] = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffK";
                s_rfc3339Formats[1] = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffffffK";
                s_rfc3339Formats[2] = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffK";
                s_rfc3339Formats[3] = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffffK";
                s_rfc3339Formats[4] = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffK";
                s_rfc3339Formats[5] = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffK";
                s_rfc3339Formats[6] = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fK";
                s_rfc3339Formats[7] = "yyyy'-'MM'-'dd'T'HH':'mm':'ssK";

                // Fall back patterns
                s_rfc3339Formats[8] = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffK"; // RoundtripDateTimePattern
                s_rfc3339Formats[9] = DateTimeFormatInfo.InvariantInfo.UniversalSortableDateTimePattern;
                s_rfc3339Formats[10] = DateTimeFormatInfo.InvariantInfo.SortableDateTimePattern;

                return s_rfc3339Formats;
            }
        }

        private static bool TryParseRfc3339(string s, out DateTime result) {
            var wasConverted = false;
            result = DateTime.Now;

            if (!string.IsNullOrEmpty(s)) {
                if (DateTime.TryParseExact(s, DateTimePatterns, DateTimeFormatInfo.InvariantInfo,
                    DateTimeStyles.AdjustToUniversal, out var parseResult)) {
                    result = DateTime.SpecifyKind(parseResult, DateTimeKind.Utc);
                    result = result.ToLocalTime();
                    wasConverted = true;
                }
            }

            return wasConverted;
        }

        public string RawData {
            get {
                if (string.IsNullOrEmpty(m_rawData)) {
                    m_rawData = Json.Serialize(Data);
                }

                return m_rawData;
            }
        }

        public Dictionary<string, object> Data { get; } = null;

        public bool IsValid { get; } = false;
    }
}