using net.roomful.assets.Editor;

namespace net.roomful.assets
{
    internal abstract class BaseCollector : ICollector
    {
        public abstract void Run(IAsset asset);

        private AssetDatabase m_assetDatabase;

        public ICollector SetAssetDatabase(AssetDatabase assetDatabase) {
            m_assetDatabase = assetDatabase;
            return this;
        }

        protected AssetDatabase AssetDatabase => m_assetDatabase;
    }
}