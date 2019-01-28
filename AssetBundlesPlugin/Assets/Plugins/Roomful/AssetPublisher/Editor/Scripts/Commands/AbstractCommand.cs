using System;
namespace RF.AssetWizzard.Commands {
    public abstract class AbstractCommand<T> where T: ICommandResult {

        private Action<T> m_completeCallback;
        public void Execute(Action<T> onComplete) {
            m_completeCallback = onComplete;
            ExecuteImpl();
        }

        protected void FireComplete(T result) {
            m_completeCallback.Invoke(result);
        }

        protected abstract void ExecuteImpl();
    }
}