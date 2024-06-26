using StansAssets.Foundation.Editor;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace net.roomful.assets.editor
{
    /// <summary>
    /// Variants Static info.
    /// </summary>
    public static class VariantsPackage
    {
        /// <summary>
        /// The package name
        /// </summary>
        public const string PACKAGE_NAME = "net.roomful.assets";

        /// <summary>
        /// Foundation package root path.
        /// </summary>
        public static readonly string RootPath = PackageManagerUtility.GetPackageRootPath(PACKAGE_NAME);

        /// <summary>
        /// Roomful Asset Publisher package info.
        /// </summary>
        public static readonly PackageInfo Info = PackageManagerUtility.GetPackageInfo(PACKAGE_NAME);

        internal static readonly string VariantsEditorUIPath = $"{RootPath}/Editor/Scripts/Variants/VariantsEditor";
    }
}
