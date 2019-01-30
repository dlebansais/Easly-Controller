namespace EaslyController.Focus
{
    using System;

    /// <summary>
    /// Base frame visibility.
    /// </summary>
    public interface IFocusFrameVisibility
    {
        /// <summary>
        /// True if the visibility depends on the show/hidden state of the view with the focus.
        /// </summary>
        bool IsVolatile { get; }

        /// <summary>
        /// Checks that a frame visibility is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame visibility can describe.</param>
        bool IsValid(Type nodeType);
    }

    /// <summary>
    /// Base frame visibility.
    /// </summary>
    public abstract class FocusFrameVisibility : IFocusFrameVisibility
    {
        #region Properties
        /// <summary>
        /// True if the visibility depends on the show/hidden state of the view with the focus.
        /// </summary>
        public abstract bool IsVolatile { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks that a frame visibility is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame visibility can describe.</param>
        public abstract bool IsValid(Type nodeType);
        #endregion
    }
}
