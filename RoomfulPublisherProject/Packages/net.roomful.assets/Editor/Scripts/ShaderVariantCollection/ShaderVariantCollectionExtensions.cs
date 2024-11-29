using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace net.roomful.assets.editor.svc
{
    static class ShaderVariantCollectionExtensions
    {
        public struct CompareResult
        {
            public List<ShaderVariantCollection.ShaderVariant> NewShaderVariants;
        }
        
        public static void GetShaderVariants(this ShaderVariantCollection @this, ref List<ShaderVariantCollection.ShaderVariant> shaderVariants)
        {
            var so = new SerializedObject(@this);
            var shaders = so.FindProperty("m_Shaders");
            for (var i = 0; i < shaders.arraySize; ++i)
            {
                var entryProp = shaders.GetArrayElementAtIndex(i);
                Shader shader = (Shader)entryProp.FindPropertyRelative("first").objectReferenceValue;
                var variantsProp = entryProp.FindPropertyRelative("second.variants");
                
                for (var j = 0; j < variantsProp.arraySize; ++j)
                {
                    var prop = variantsProp.GetArrayElementAtIndex(j);
                    var keywords = prop.FindPropertyRelative("keywords").stringValue;
                    var passType = (UnityEngine.Rendering.PassType)prop.FindPropertyRelative("passType").intValue;
                    var shaderVariant = new ShaderVariantCollection.ShaderVariant(shader, passType, !string.IsNullOrEmpty(keywords) ? keywords.Split(" ") : Array.Empty<string>());
                    shaderVariants.Add(shaderVariant);
                }
            }
        }

        public static void UnionWith(this ShaderVariantCollection @this, ShaderVariantCollection other, ShaderCollection shaderCollection)
        {
            var shaderVariants = new List<ShaderVariantCollection.ShaderVariant>();
            other.GetShaderVariants(ref shaderVariants);

            for (var i = 0; i < shaderVariants.Count; ++i)
            {
                var shaderVariant = shaderVariants[i];
                if (shaderVariant.ShouldBeAdded(shaderCollection, @this))
                {
                    @this.Add(shaderVariant);
                }
            }
        }

        public static CompareResult CompareTo(this ShaderVariantCollection @this, ShaderVariantCollection other, ShaderCollection shaderCollection)
        {
            var shaderVariants = new List<ShaderVariantCollection.ShaderVariant>();
            other.GetShaderVariants(ref shaderVariants);

            List<ShaderVariantCollection.ShaderVariant> newShaderVariants = new();
            for (var i = 0; i < shaderVariants.Count; ++i)
            {
                var shaderVariant = shaderVariants[i];
                if (shaderVariant.ShouldBeAdded(shaderCollection, @this))
                {
                    newShaderVariants.Add(shaderVariant);
                    Debug.Log($"New Shader: {shaderVariant.shader.name}, keywords: {string.Join(", ", shaderVariant.keywords)} added to ShaderVariantCollection with name: {@this.name}.");
                }
            }

            return new CompareResult
            {
                NewShaderVariants = newShaderVariants
            };
        }

        static bool ShouldBeAdded(this ShaderVariantCollection.ShaderVariant @this, ShaderCollection shaderCollection, ShaderVariantCollection destination)
        {
            var shaderName = @this.shader.name;
            if (shaderName.StartsWith("Hidden/") 
                || shaderName.StartsWith("UI/") 
                || shaderName.StartsWith("TextMeshPro/")
                || shaderName.StartsWith("Roomful/")) return false;
            return !shaderCollection.Shaders.Contains(@this.shader) && !destination.Contains(@this);
        }
    }
}
