namespace net.roomful.assets
{
    abstract class BaseStyleAsset : Asset<StyleAssetTemplate>
    {
        public void SetTemplate(StyleAssetTemplate tpl) {
            m_Template = tpl;
        }
    }
}
