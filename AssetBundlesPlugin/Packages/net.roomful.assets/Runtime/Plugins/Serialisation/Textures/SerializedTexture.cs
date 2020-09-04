using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace RF.AssetBundles.Serialization
{
	[System.Serializable]
	public class SerializedTexture {
		public Texture MainTexture;

		public bool 	AllowAlphaSplitting;
		public bool 	AlphaIsTransparency;
		public string 	AlphaSource;
		public float 	AlphaTestReferenceValue;
		public int 		AnisoLevel;
		public bool 	BorderMipmap;
		public int 		CompressionQuality;
		public bool 	ConvertToNormalmap;
		public bool 	CrunchedCompression;
		public bool 	Fadeout;
		public string 	FilterMode;
		public string 	GenerateCubemap;
		public float 	HeightmapScale;
		public bool 	IsReadable;
		public int 		MaxTextureSize;
		public float 	MipMapBias;
		public bool 	MipmapEnabled;
		public int 		MipmapFadeDistanceEnd;
		public int 		MipmapFadeDistanceStart;
		public string 	MipmapFilter;
		public bool 	MipMapsPreserveCoverage;
		public string 	NormalmapFilter;
		public string 	NpotScale;
		public bool 	QualifiesForSpritePacking;
		public Vector4	SpriteBorder;
		public string 	SpriteImportMode;
		public string 	SpritePackingTag;
		public Vector2 	SpritePivot;
		public float 	SpritePixelsPerUnit;
		public SerializedSpriteMetaData[] Spritesheet;
		public bool 	SRGBTexture;
		public string 	TextureCompression;
		public string 	TextureShape;
		public string 	TextureType;
		public string 	WrapMode;
		public string 	WrapModeU;
		public string 	WrapModeV;
		public string 	WrapModeW;


        public Vector2 TextureOffset = Vector2.zero;
        public Vector2 TextureScale = Vector2.one;
        



        public string[] Platforms = new string[]{"iPhone", "Android", "WebGL"};
		public SerializedPlatformTextureSettings[] PlatformsSettings;

		public SerializedDefaultTextureSettings DefaultSettings;

		public void Serialize(Texture tex) {
			#if UNITY_EDITOR
			MainTexture = tex;

			string path = UnityEditor.AssetDatabase.GetAssetPath(tex);
			TextureImporter ti = (TextureImporter)TextureImporter.GetAtPath(path);

			AllowAlphaSplitting = 		ti.allowAlphaSplitting;
			AlphaIsTransparency = 		ti.alphaIsTransparency;
			AlphaSource = 				ti.alphaSource.ToString();
			AlphaTestReferenceValue =	ti.alphaTestReferenceValue;
			AnisoLevel = 				ti.anisoLevel;
			BorderMipmap =				ti.borderMipmap;
			CompressionQuality = 		ti.compressionQuality;
			ConvertToNormalmap = 		ti.convertToNormalmap;
			CrunchedCompression = 		ti.crunchedCompression;
			Fadeout = 					ti.fadeout;
			FilterMode = 				ti.filterMode.ToString();
			GenerateCubemap = 			ti.generateCubemap.ToString();
			HeightmapScale = 			ti.heightmapScale;
			IsReadable = 				ti.isReadable;
			MaxTextureSize = 			ti.maxTextureSize;
			MipMapBias = 				ti.mipMapBias;
			MipmapEnabled = 			ti.mipmapEnabled;
			MipmapFadeDistanceEnd = 	ti.mipmapFadeDistanceEnd;
			MipmapFadeDistanceStart = 	ti.mipmapFadeDistanceStart;
			MipmapFilter = 				ti.mipmapFilter.ToString();
			MipMapsPreserveCoverage = 	ti.mipMapsPreserveCoverage;
			NormalmapFilter = 			ti.normalmapFilter.ToString();
			NpotScale = 				ti.npotScale.ToString();
			QualifiesForSpritePacking = ti.qualifiesForSpritePacking;
			SpriteBorder = 				ti.spriteBorder;
			SpriteImportMode =			ti.spriteImportMode.ToString();
			SpritePackingTag = 			ti.spritePackingTag;
			SpritePivot = 				ti.spritePivot;
			SpritePixelsPerUnit = 		ti.spritePixelsPerUnit;

			Spritesheet = new SerializedSpriteMetaData[ti.spritesheet.Length];
			for (int i = 0; i < Spritesheet.Length; i++) {
				Spritesheet[i].Name =		ti.spritesheet[i].name;
				Spritesheet[i].Rect = 		ti.spritesheet[i].rect;
				Spritesheet[i].Alignment = 	ti.spritesheet[i].alignment;
				Spritesheet[i].Pivot = 		ti.spritesheet[i].pivot;
				Spritesheet[i].Border = 	ti.spritesheet[i].border;
			}

			SRGBTexture = 				ti.sRGBTexture;
			TextureCompression = 		ti.textureCompression.ToString();
			TextureShape = 				ti.textureShape.ToString();
			TextureType = 				ti.textureType.ToString();
			WrapMode = 					ti.wrapMode.ToString();
			WrapModeU = 				ti.wrapModeU.ToString();
			WrapModeV = 				ti.wrapModeV.ToString();
			WrapModeW = 				ti.wrapModeW.ToString();

			List<SerializedPlatformTextureSettings> ps = new List<SerializedPlatformTextureSettings>();

			for (int i = 0; i < Platforms.Length; i++) {
				string  				platformString = Platforms[i];
				int     				platformMaxTextureSize;
				TextureImporterFormat 	platformTextureFmt;
				int     				platformCompressionQuality;
				bool    				platformAllowsAlphaSplit;

				if (ti.GetPlatformTextureSettings(platformString, out platformMaxTextureSize, out platformTextureFmt, out platformCompressionQuality, out platformAllowsAlphaSplit)) {
					SerializedPlatformTextureSettings spts = new SerializedPlatformTextureSettings();

					spts.Platform = platformString;
					spts.MaxTextureSize = platformMaxTextureSize;
					spts.TextureFormat = platformTextureFmt.ToString();
					spts.CompressionQuality = platformCompressionQuality;
					spts.Etc1AlphaSplitEnabled = platformAllowsAlphaSplit;

					ps.Add(spts);
				}
			}

			PlatformsSettings = ps.ToArray();

			DefaultSettings = new SerializedDefaultTextureSettings();
			DefaultSettings.AllowsAlphaSplitting = ti.GetDefaultPlatformTextureSettings().allowsAlphaSplitting;
			DefaultSettings.CompressionQuality = ti.GetDefaultPlatformTextureSettings().compressionQuality;
			DefaultSettings.CrunchedCompression = ti.GetDefaultPlatformTextureSettings().crunchedCompression;
			DefaultSettings.TextureFormat = ti.GetDefaultPlatformTextureSettings().format.ToString();
			DefaultSettings.MaxTextureSize = ti.GetDefaultPlatformTextureSettings().maxTextureSize;
			DefaultSettings.Platform = ti.GetDefaultPlatformTextureSettings().name;
			DefaultSettings.Overridden = ti.GetDefaultPlatformTextureSettings().overridden;
			DefaultSettings.TextureCompression = ti.GetDefaultPlatformTextureSettings().textureCompression.ToString();

			#endif
		}

	}
}