using System.Collections.Generic;
using net.roomful.api.assets;
using UnityEngine;

namespace net.roomful.api.props
{
    /// <summary>
    /// Represents a Prop object created in room
    /// </summary>
    public interface IProp
    {
        /// <summary>
        /// Prop Asset
        /// </summary>
        IPropAssetTemplate Asset { get; }

        /// <summary>
        /// Prop server side representation.
        /// </summary>
        IPropTemplate Template { get; }

        /// <summary>
        /// Prop Collider.
        /// </summary>
        BoxCollider Collider { get; }

        /// <summary>
        /// Method will generate prop title based on the prop Asset, Properties and Content.
        /// </summary>
        string GetTitle();

        /// <summary>
        /// Method will generate prop description based on the prop Asset, Properties and Content.
        /// </summary>
        string GetDescription();

        /// <summary>
        /// Read-only prop transform
        /// </summary>
        IPropTransform Transform { get; }

        /// <summary>
        /// Collection of child props.
        /// </summary>
        IReadOnlyList<IProp> Children { get; }

        /// <summary>
        /// Focus pointer.
        /// Pointer should be used as the look target when you want to look at the prop.
        /// </summary>
        Vector3 FocusPointer { get; }

        /// <summary>
        /// Set delegate to monitor prop events.
        /// </summary>
        /// <param name="propDelegate">delegate instance.</param>
        void AddDelegate(IPropDelegate propDelegate);

        /// <summary>
        /// Remove delegate to monitor prop events.
        /// </summary>
        /// <param name="propDelegate">delegate instance.</param>
        void RemoveDelegate(IPropDelegate propDelegate);

        IReadOnlyList<Animator> AnimatorControllers { get; }

        bool HasAnimators { get; }
        bool HasFocusPointers { get; }
        int ResourcesCount { get; }

        void RunAnimationWithTriggerName(string triggerName);
        void RunAnimator();

        /// <summary>
        /// Gets a value that indicates whether this prop is loading.
        /// </summary>
        bool IsLoading { get; }

        T GetPropComponent<T>() where T : IPropComponent;

        T[] GetPropComponents<T>() where T : IPropComponent;

        IAssetLoader GetActiveLoader();

        bool IsGameObjectDestroyed();

        void SetActive(bool active);

        /// <summary>
        /// Defines if prop has active collision
        /// TODO this is very heavy method, and should be moved to prop editor implementation.
        /// </summary>
        bool HasActiveCollision { get; }

        /// <summary>
        /// The function is quite heavy.
        /// It will use object and oll of it's children renderer bounds,
        /// and will resize prop colliders size based on calculated renderer bounds.
        /// </summary>
        void AutosizeCollider();
    }
}
