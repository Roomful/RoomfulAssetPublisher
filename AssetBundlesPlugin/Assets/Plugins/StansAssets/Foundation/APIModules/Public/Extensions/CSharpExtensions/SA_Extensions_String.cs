using System;
using System.Collections.Generic;
using UnityEngine;

using SA.Foundation;

public static class SA_Extensions_String  {

    public static string GetLast(this string source, int count) {
        if (count >= source.Length)
            return source;
        return source.Substring(source.Length - count);
    }

    public static string RemoveLast(this string source, int count) {
        if (count >= source.Length)
            return string.Empty;
        return source.Remove(source.Length - count); 
    }

    public static string GetFirst(this string source, int count) {


        if (count >= source.Length)
            return source;
        return source.Substring(0, count);
    }

    public static void CopyToClipboard(this string source) {
        TextEditor te = new TextEditor();
        te.text = source;
        te.SelectAll();
        te.Copy();
    }

    public static System.Uri CovertToURI(this string source) {
        return new System.Uri(source);
    }


    public static string UppercaseFirst(this string source) {
        // Check for empty string.
        if (string.IsNullOrEmpty(source)) {
            return source;
        }
        // Return char and concat substring.
        return char.ToUpper(source[0]) + source.Substring(1);
    }


    public static string SafeStringFormat(this string source, params object[] args) {

        if (string.IsNullOrEmpty(source)) {
            return source;
        }

        string formated = source;
        try {
            formated =  string.Format(source, args);
        } catch(Exception ex) {
            Debug.LogWarning(ex.Message);
        }

        return formated;
    }

    

    public static byte[] ToBytes(this string source) {
        return System.Text.Encoding.UTF8.GetBytes(source);
    }


    public static byte[] BytesFromBase64String(this string source) {
        return Convert.FromBase64String(source);
    }

}
