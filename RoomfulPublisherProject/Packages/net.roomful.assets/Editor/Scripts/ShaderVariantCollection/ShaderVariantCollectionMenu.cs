using UnityEditor;

namespace net.roomful.assets.editor.svc
{
    static class ShaderVariantCollectionMenu
    {
        [MenuItem("Roomful/ShaderVariantCollection/Compare BaseShaderVariantCollection with Current [STEP 1]")]
        static void CompareShaderVariantCollection()
        {
            if (ShaderVariantCollectionUtil.TryLoadCollection(out var baseShaderVariantCollection))
            {
                var currentCollectionPath = ShaderVariantCollectionUtil.SaveCurrentCollection($"Assets/{ShaderVariantCollectionUtil.NewShaderVariantCollectionName}.{ShaderVariantCollectionUtil.ShaderVariantCollectionExtension}");
                if (ShaderVariantCollectionUtil.TryLoadCollection(currentCollectionPath, out var newCollection)
                && ShaderVariantCollectionUtil.TryLoadShaderCollection(out var excludeCollection))
                {
                    var result = baseShaderVariantCollection.CompareTo(newCollection, excludeCollection);
                    var stringResult = result.NewShaderVariants.Count > 0 ? $"has {result.NewShaderVariants.Count}" : "doesn't have";
                    EditorUtility.DisplayDialog("ShaderVariantCollection Compare Complete", $"Current ShaderVariantCollection {stringResult} new ShaderVariants compared to {ShaderVariantCollectionUtil.BaseShaderVariantCollectionName}", "Okay");
                    UnityEditor.AssetDatabase.DeleteAsset(currentCollectionPath);
                }
            }
        }
        
        [MenuItem("Roomful/ShaderVariantCollection/Update BaseShaderVariantCollection with Current  [STEP 2]")]
        static void UpdateShaderVariantCollection()
        {
            if (ShaderVariantCollectionUtil.TryLoadCollection(out var baseShaderVariantCollection))
            {
                var currentCollectionPath = ShaderVariantCollectionUtil.SaveCurrentCollection($"Assets/{ShaderVariantCollectionUtil.NewShaderVariantCollectionName}.{ShaderVariantCollectionUtil.ShaderVariantCollectionExtension}");
                if (ShaderVariantCollectionUtil.TryLoadCollection(currentCollectionPath, out var newCollection)
                    && ShaderVariantCollectionUtil.TryLoadShaderCollection(out var excludeCollection))
                {
                    string beforeString = $"Shaders: {baseShaderVariantCollection.shaderCount}, Variants: {baseShaderVariantCollection.variantCount}";
                    baseShaderVariantCollection.UnionWith(newCollection, excludeCollection);
                    string afterString = $"Shaders: {baseShaderVariantCollection.shaderCount}, Variants: {baseShaderVariantCollection.variantCount}";
                    
                    EditorUtility.DisplayDialog("ShaderVariantCollection Update complete", $"Before {beforeString}.\nAfter {afterString}", "Okay");
                    
                    UnityEditor.AssetDatabase.DeleteAsset(currentCollectionPath);
                    EditorApplication.ExecuteMenuItem("File/Save Project");
                }
            }
        }
        
        [MenuItem("Roomful/ShaderVariantCollection/Clear Current ShaderVariantCollection")]
        static void ClearCurrentShaderVariantCollection()
        {
            ShaderVariantCollectionUtil.ClearCurrentCollection();
        }
    }
}
