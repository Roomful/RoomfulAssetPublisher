using System;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace net.roomful.api
{
    public interface IControlTemplate
    {
        string Title { get; }
        GameObject Create(Transform parent);
        void HideControl();
        void DisableInteraction();
    }

    public interface IPageTemplate
    {
        void Create(Action<GameObject> onPageCreated);
    }

    public class PageTemplate : IPageTemplate
    {
        private readonly Action<Action<GameObject>> m_createPageDelegate;
        
        public PageTemplate(Action<Action<GameObject>>  createPageDelegate) {
            m_createPageDelegate = createPageDelegate;
        }
        public void Create(Action<GameObject>onPageCreated) {
            m_createPageDelegate.Invoke(onPageCreated);
        }
    }

    public abstract class DefaultControlTemplate : IControlTemplate
    {
        private readonly Func<GameObject> m_createPrefabDelegate;
        private GameObject m_control;

        public virtual string Title { get; }

        protected GameObject Control => m_control;
        protected GameObject Prefab => m_createPrefabDelegate.Invoke();

        protected DefaultControlTemplate(string title, Func<GameObject> createPrefabDelegate) {
            m_createPrefabDelegate = createPrefabDelegate;
            Title = title;
        }

        public abstract GameObject Create(Transform parent);
        public void HideControl() {
            m_control.SetActive(false);
        }

        public abstract void DisableInteraction();

        protected virtual void OnControlInstantiated(GameObject control) {
            m_control = control;
            TrySetTitle(Title);
        }

        protected virtual bool TrySetTitle(string title) {
            var textComponent = m_control.GetComponentInChildren<TMP_Text>();
            if (textComponent != null) {
                textComponent.text = title;
                return true;
            }

            return false;
        }
    }

    public abstract class DefaultControlTemplate<T> : DefaultControlTemplate
    {
        private readonly Action<T> m_valueChangedDelegate;
        private readonly Func<T> m_initValueDelegate;

        protected Action<T> ValueChangedDelegate => m_valueChangedDelegate;
        protected Func<T> InitValueDelegate => m_initValueDelegate;

        protected DefaultControlTemplate(string title,  Action<T> valueChangedDelegate, Func<T> initValueDelegate,
            Func<GameObject> createPrefabDelegate) : base(title, createPrefabDelegate) {

            m_valueChangedDelegate = valueChangedDelegate;
            m_initValueDelegate = initValueDelegate;
        }

        public override GameObject Create(Transform parent) {
            if (Prefab == null) {
                Debug.LogError($"Prefab is null for {Title}!");
                return null;
            }

            var control = Object.Instantiate(Prefab, parent);
            OnControlInstantiated(control);
            SetInitialValue(InitValueDelegate.Invoke());

            return control;
        }

        protected abstract void SetInitialValue(T initValue);
    }
}