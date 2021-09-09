﻿using net.roomful.api.props;

namespace net.roomful.api.zoom
{
    /// <summary>
    /// Defines Prop View context inside the zoom view.
    /// </summary>
    public struct ZoomViewContext
    {
        public const int UNDEFINED_INDEX = -1;
        public static readonly ZoomViewContext Undefined = new ZoomViewContext();

        /// <summary>
        /// Prop Resourced Index Zoom view will focus on.
        /// </summary>
        public int ResourceIndex;

        /// <summary>
        /// Prop Resourced Index Zoom view will focus on.
        /// </summary>
        public int InnerPropIndex;

        /// <summary>
        /// Prop Resourced Index Zoom view will focus on.
        /// </summary>
        public int FocusPointIndex;

        /// <summary>
        /// Prop to View.
        /// </summary>
        public IProp Prop;

        /// <summary>
        /// Active prop resource, based on <see cref="ResourceIndex"/>
        /// </summary>
        public IResource Resource {
            get {
                if (Prop == null)
                    return null;

                if (Prop.Template.Content.Count == 0)
                    return null;

                var resIndex = ResourceIndex;
                if (resIndex == UNDEFINED_INDEX) {
                    if (Prop.Asset.InvokeType != PropInvokeType.Default) {
                        return null;
                    }
                    //Logo is the special snowflake. We should never use logo as zoom view resource context
                    if (Prop.Asset.LogoCount > 0) {
                        return null;
                    }
                    resIndex = 0;
                }

                return Prop.Template.Content[resIndex];
            }
        }

        public bool HasVideoToPlay => Resource != null && Resource.Type == ContentType.Video;

        public IProp InnerProp {
            get {
                if (Prop == null)
                    return null;

                if (InnerPropIndex == UNDEFINED_INDEX)
                    return null;

                if (Prop.Children.Count <= InnerPropIndex)
                    return null;

                return Prop.Children[InnerPropIndex];
            }
        }

        public bool IsInsideProp {
            get {
                if (ResourceIndex != UNDEFINED_INDEX
                    || InnerPropIndex != UNDEFINED_INDEX
                    || FocusPointIndex != UNDEFINED_INDEX) {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Create Default view context for a prop.
        /// </summary>
        /// <param name="prop">The target prop.</param>
        public ZoomViewContext(IProp prop) {
            Prop = prop;
            ResourceIndex = UNDEFINED_INDEX;
            InnerPropIndex = UNDEFINED_INDEX;
            FocusPointIndex = UNDEFINED_INDEX;
        }
    }
}

