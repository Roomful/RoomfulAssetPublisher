using RF.AssetWizzard.Editor;

namespace RF.AssetWizzard {
    public abstract class BaseCollector: ICollector {
        public abstract void Run(IAsset asset);

        private AssetDatabase m_assetDatabase;

        public ICollector SetAssetDatabase(AssetDatabase assetDatabase) {
            m_assetDatabase = assetDatabase;
            return this;
        }
        
        protected AssetDatabase AssetDatabase {
            get { return m_assetDatabase; }
        }
    }
}