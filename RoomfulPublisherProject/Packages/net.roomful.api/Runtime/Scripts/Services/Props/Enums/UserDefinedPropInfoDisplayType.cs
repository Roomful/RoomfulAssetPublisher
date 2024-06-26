// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api {

    /// <summary>
    /// Defines how prop info (title & description) is displayed.
    /// </summary>
    public enum UserDefinedPropInfoDisplayType {

        /// <summary>
        /// Prop info is not displayed.
        /// For wall -> info panel
        /// For floor -> none
        /// For booth -> cloud
        /// </summary>
        Default = 0,

        /// <summary>
        /// Prop info is displayed on gold panel when entering a zoom view.
        /// </summary>
        DisplayIn2d = 1,

        /// <summary>
        /// Prop info displayed in a cloud near the prop.
        /// </summary>
        DisplayIn3d = 2,
        
        /// <summary>
        /// Nothing will be showed.
        /// </summary>
        None = 3
    }
    
    
    public enum PropInfoDisplayType {
        /// <summary>
        /// Prop info is displayed on gold panel when entering a zoom view.
        /// </summary>
        DisplayIn2d = 1,

        /// <summary>
        /// Prop info displayed in a cloud near the prop.
        /// </summary>
        DisplayIn3d = 2,
        
        /// <summary>
        /// Nothing will be showed.
        /// </summary>
        None = 3
    }
    
}
