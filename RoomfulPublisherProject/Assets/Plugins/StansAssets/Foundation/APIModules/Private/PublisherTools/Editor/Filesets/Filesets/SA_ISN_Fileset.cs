﻿using SA.iOS;
using System.Collections.Generic;
using SA.Foundation.Config;

namespace SA.Foundation.Publisher.Exporter
{
    public class SA_ISN_Fileset : SA_PluginFileset
    {
        public const string ID = "IOS Native Plugin";
        public override string Id => ID;

        public override List<string> GetDirsIncludedPaths()
        {
            var paths = base.GetDirsIncludedPaths();
            paths.Add(ISN_Settings.IOSNativeFolder);
            paths.Add(SA_Config.StansAssetsXcodePackagePath);
            return paths;
        }

        public override List<string> GetExcludedPaths()
        {
            var excludes = base.GetExcludedPaths();
            excludes.Add(".*ISD_EditorMenu.cs");
            excludes.Add(".*XCodeEditorMenu.cs");

            return excludes;
        }

        protected override PluginVersionHandler GetPluginVersion()
        {
            return ISN_Settings.PluginVersion;
        }
    }
}
