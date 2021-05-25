using System;

public static class VideochatLinkValidator
{
    public static bool ValidateString(string input) {
        var isValidUrl = Uri.TryCreate(input, UriKind.Absolute, out var uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        return isValidUrl || input.Equals(string.Empty);
    }
}