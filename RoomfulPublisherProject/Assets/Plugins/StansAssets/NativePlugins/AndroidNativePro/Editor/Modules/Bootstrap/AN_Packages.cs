using System.Collections.Generic;
using StansAssets.Foundation.Editor;
using UnityEditor;
using UnityEditor.PackageManager;

namespace SA.Android
{
    public static class AN_Packages
    {
        public static readonly string FirebaseAnalyticsPackage = "com.google.firebase.analytics";
        public static readonly string FirebaseMessagingPackage = "com.google.firebase.messaging";

        const string k_FirebaseSdkVersion = "6.15.2";

        public static bool IsMessagingSdkInstalled {
            get {
#if AN_FIREBASE_MESSAGING
                return true;
#else
                return false;
#endif
            }
        }

        public static bool IsAnalyticsSdkInstalled {
            get {
#if AN_FIREBASE_ANALYTICS
                return true;
#else
                return false;
#endif
            }
        }
    }
}
