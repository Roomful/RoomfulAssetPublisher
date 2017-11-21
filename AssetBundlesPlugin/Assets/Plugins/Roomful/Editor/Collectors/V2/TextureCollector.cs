using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using RF.AssetBundles.Serialization;


namespace RF.AssetWizzard.Editor
{
	public class TextureCollector {
		public void Run(IAsset asset, SerializedTexture st) {
			#if UNITY_EDITOR
			string texName = st.MainTexture.name;

			AssetDatabase.SaveAsset<Texture> (asset, st.MainTexture);

			string path = UnityEditor.AssetDatabase.GetAssetPath(AssetDatabase.LoadAsset<Texture>(asset, texName));
			TextureImporter ti = (TextureImporter)TextureImporter.GetAtPath(path);

			ti.allowAlphaSplitting = 		st.AllowAlphaSplitting;
			ti.alphaIsTransparency = 		st.AlphaIsTransparency;
			ti.alphaTestReferenceValue = 	st.AlphaTestReferenceValue;
			ti.anisoLevel = 				st.AnisoLevel;
			ti.borderMipmap = 				st.BorderMipmap;
			ti.compressionQuality = 		st.CompressionQuality;
			ti.convertToNormalmap = 		st.ConvertToNormalmap;
			ti.crunchedCompression = 		st.CrunchedCompression;
			ti.fadeout = 					st.Fadeout;
			ti.heightmapScale = 			st.HeightmapScale;
			ti.isReadable = 				st.IsReadable;
			ti.maxTextureSize = 			st.MaxTextureSize;
			ti.mipMapBias = 				st.MipMapBias;
			ti.mipmapEnabled = 				st.MipmapEnabled;
			ti.mipmapFadeDistanceEnd = 		st.MipmapFadeDistanceEnd;
			ti.mipmapFadeDistanceStart = 	st.MipmapFadeDistanceStart;
			ti.mipMapsPreserveCoverage = 	st.MipMapsPreserveCoverage;
			//ti.qualifiesForSpritePacking = 	st.QualifiesForSpritePacking;

			ti.spriteBorder = 				st.SpriteBorder;
			ti.spritePackingTag = 			st.SpritePackingTag;
			ti.spritePivot = 				st.SpritePivot;
			ti.spritePixelsPerUnit = 		st.SpritePixelsPerUnit;
			ti.sRGBTexture = 				st.SRGBTexture;

			ti.spritesheet = new SpriteMetaData[st.Spritesheet.Length];

			for (int i = 0; i < st.Spritesheet.Length; i++) {
				ti.spritesheet[i].name =		st.Spritesheet[i].Name;
				ti.spritesheet[i].rect = 		st.Spritesheet[i].Rect;
				ti.spritesheet[i].alignment = 	st.Spritesheet[i].Alignment;
				ti.spritesheet[i].pivot = 		st.Spritesheet[i].Pivot;
				ti.spritesheet[i].border = 		st.Spritesheet[i].Border;
			}

			ti.alphaSource = 		(TextureImporterAlphaSource)System.Enum.Parse(typeof(TextureImporterAlphaSource), st.AlphaSource);
			ti.filterMode = 		(FilterMode)System.Enum.Parse(typeof(FilterMode), st.FilterMode);
			ti.generateCubemap = 	(TextureImporterGenerateCubemap)System.Enum.Parse(typeof(TextureImporterGenerateCubemap), st.GenerateCubemap);
			ti.mipmapFilter = 		(TextureImporterMipFilter)System.Enum.Parse(typeof(TextureImporterMipFilter), st.MipmapFilter);
			ti.normalmapFilter = 	(TextureImporterNormalFilter)System.Enum.Parse(typeof(TextureImporterNormalFilter), st.NormalmapFilter);
			ti.npotScale = 			(TextureImporterNPOTScale)System.Enum.Parse(typeof(TextureImporterNPOTScale), st.NpotScale);
			ti.spriteImportMode = 	(SpriteImportMode)System.Enum.Parse(typeof(SpriteImportMode), st.SpriteImportMode);
			ti.textureCompression = (TextureImporterCompression)System.Enum.Parse(typeof(TextureImporterCompression), st.TextureCompression);
			ti.textureShape = 		(TextureImporterShape)System.Enum.Parse(typeof(TextureImporterShape), st.TextureShape);
			ti.textureType = 		(TextureImporterType)System.Enum.Parse(typeof(TextureImporterType), st.TextureType);
			ti.wrapMode = 			(TextureWrapMode)System.Enum.Parse(typeof(TextureWrapMode), st.WrapMode);
			ti.wrapModeU = 			(TextureWrapMode)System.Enum.Parse(typeof(TextureWrapMode), st.WrapModeU);
			ti.wrapModeV = 			(TextureWrapMode)System.Enum.Parse(typeof(TextureWrapMode), st.WrapModeV);
			ti.wrapModeW = 			(TextureWrapMode)System.Enum.Parse(typeof(TextureWrapMode), st.WrapModeW);

			for ( int i = 0; i < st.PlatformsSettings.Length; i++ ) {
				TextureImporterPlatformSettings settings = new TextureImporterPlatformSettings();

				settings.name = 				st.PlatformsSettings [i].Platform;
                settings.maxTextureSize = 		st.PlatformsSettings [i].MaxTextureSize;
                settings.compressionQuality = 	st.PlatformsSettings [i].CompressionQuality;
      			settings.allowsAlphaSplitting = st.PlatformsSettings [i].Etc1AlphaSplitEnabled;
                settings.format = 				(TextureImporterFormat)System.Enum.Parse(typeof(TextureImporterFormat), st.PlatformsSettings [i].TextureFormat);

                ti.SetPlatformTextureSettings (settings);
			}

			ti.SaveAndReimport();
			#endif
		}
	}
}
