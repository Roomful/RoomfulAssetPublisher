using SA.Foundation.Config;
using SA.Foundation.Patterns;

namespace SA.Productivity.Console
{
    public class UCL_PlatfromsLogSettings : SA_ScriptableSingleton<UCL_PlatfromsLogSettings>
    {
        public const string PLUGIN_FOLDER = SA_Config.STANS_ASSETS_PRODUCTIVITY_PLUGINS_PATH + "Console/";
        public bool iOS_LogsRecord = true;
        public bool iOS_OverrideLogsOutput = true;


        public bool Android_LogsRecord = true;
        public bool Android_OverrideLogsOutput = true;

        protected override string BasePath {
            get { return PLUGIN_FOLDER; }
        }
    }
}