namespace RF.AssetWizzard.Editor {

    public static class TextureResizer {
        
        public static void ResizeTextureMaintainAspectRatio(float maxWidth, float maxHeight, ref float width, ref float height) {
            float ratio;
            if(width > height) {
                ratio = height / width;
                width = maxWidth;
                height = width * ratio;
            }
            else {
                ratio = width / height;
                height = maxHeight;
                width = height * ratio;
            }
        }
    }
}
