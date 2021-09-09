using System;
using System.Text;

namespace SA.Productivity.GoogleSheets
{
    public static class GD_LocalizationUtil
    {
        private static readonly StringBuilder s_stringBuilder = new StringBuilder();


        public static string SafeStringFormat(string source, params object[] args) {
            if (string.IsNullOrEmpty(source)) return source;

            var formated = source;
            try {
                formated = string.Format(source, args);
            } catch (Exception ex) {
                UnityEngine.Debug.LogWarning(ex.Message);
            }
            return formated;
        }

        public static string Concat(params object[] objects) {
            s_stringBuilder.Length = 0;
            foreach (var t in objects) {
                s_stringBuilder.Append(t);
            }

            return s_stringBuilder.ToString();
        }
    }
}
