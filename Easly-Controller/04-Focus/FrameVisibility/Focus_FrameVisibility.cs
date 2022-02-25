namespace EaslyController.Focus
{
    using NotNullReflection;

    /// <summary>
    /// Base frame visibility.
    /// </summary>
    public interface IFocusFrameVisibility
    {
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
        #region Client Interface
        /// <summary>
        /// Checks that a frame visibility is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame visibility can describe.</param>
        public abstract bool IsValid(Type nodeType);
        #endregion
    }
}
