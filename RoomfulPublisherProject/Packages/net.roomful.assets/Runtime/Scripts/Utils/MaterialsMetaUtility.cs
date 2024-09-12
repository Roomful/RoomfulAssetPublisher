using System.Collections.Generic;

namespace Utils
{
    public static class MaterialsMetaUtility
    {
        public static readonly Dictionary<string, string> KeywordsAbbreviations = new Dictionary<string, string>() {
            {"_ALPHAPREMULTIPLY_ON", "APRE"},
            {"_ALPHABLEND_ON", "ABON"},
            {"_DEPTH_NO_MSAA", "DNM"},
            {"_ENVIRONMENTREFLECTIONS_OFF", "EROFF"},
            {"_METALLICSPECGLOSSMAP", "MSGM"},
            {"_SPECGLOSSMAP", "SGM"},
            {"_METALLICGLOSSMAP", "MGM"},
            {"_SPECULARHIGHLIGHTS_OFF", "SHOFF"},
            {"_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A", "STACA"},
            {"LIGHTPROBE_SH", "LPSH"},
            {"UNITY_HDR_ON", "UHDR"},
            {"_GLOSSYREFLECTIONS_OFF", "GROFF"},
            {"_ALPHATEST_ON", "ATON"},
            {"_OCCLUSIONMAP", "OCCM"},
            {"_NORMALMAP", "NRMM"},
            {"_RECEIVE_SHADOWS_OFF", "RSOFF"},
            {"_EMISSION", "EM"},
            {"_SPECULAR_SETUP", "SPS"},
            {"_LIGHTMAPPING_DYNAMIC_LIGHTMAPS", "LMDL"},
            {"_UVSEC_UV1", "UUV1"},
            {"_DETAIL_MULX2", "DMULX"},
            {"_GLOSSINESS_FROM_BASE_ALPHA", "GFBA"},
            {"_SPECULAR_COLOR", "SC"},
            {"_FLIPBOOKBLENDING_OFF", "FBOFF"},
            {"_COLOROVERLAY_ON", "COON"},
        };
        
    }
}
