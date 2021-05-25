using System;
using System.Collections.Generic;
using SA.Foundation.Config;

namespace SA.Foundation.Publisher.Exporter
{
    public static class SA_FilesetManager
    {
        static readonly Dictionary<string, SA_PluginFileset> s_Filesets = new Dictionary<string, SA_PluginFileset>();

        static SA_FilesetManager()
        {
            AddFileset(new SA_FoundationFileset());
            AddFileset(new SA_AMM_Fileset());
            AddFileset(new SA_AN_Fileset());
            AddFileset(new SA_GD_Fileset());
            AddFileset(new SA_ISN_Fileset());
            AddFileset(new SA_UM_Fileset());
        }

        static void AddFileset(SA_PluginFileset fileset)
        {
            if (s_Filesets.ContainsKey(fileset.Id)) throw new ArgumentException(
                $"Fileset (id = \"{fileset.Id}\") already registered");

            s_Filesets.Add(fileset.Id, fileset);
        }

        public static SA_PluginFileset GetFileset(string id)
        {
            if (!s_Filesets.ContainsKey(id)) throw new ArgumentException(
                $"Fileset (id = \"{id}\") has not been registered");

            return s_Filesets[id];
        }

        public static bool IsFoundationChanged()
        {
            return SA_Config.FoundationVersion.HasChanges();
        }

        public static void UpdateFoundationVersion()
        {
            SA_Config.FoundationVersion.UpgrageMajorVersionIfNeed();
        }
    }
}
