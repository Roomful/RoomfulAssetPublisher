using SA.Foundation.Config;
using SA.Foundation.Patterns;

namespace SA.Productivity.Console
{
    public class UCL_PlatfromsLogSettings : SA_ScriptableSingleton<UCL_PlatfromsLogSettings>
    {
        public const string PLUGIN_FOLDER = SA_Config.StansAssetsProductivityPluginsPath + "Console/";
        public bool iOS_LogsRecord = true;
        public bool iOS_OverrideLogsOutput = true;


        public bool Android_LogsRecord = true;
        public bool Android_OverrideLogsOutput = true;

        protected override string BasePath {
            get { return PLUGIN_FOLDER; }
        }

        public override string PluginName { get; }
        public override string DocumentationURL { get; }
        public override string SettingsUIMenuItem { get; }
    }
}