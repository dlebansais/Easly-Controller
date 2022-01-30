namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Frame visibility that depends on the IsComplex template property.
    /// </summary>
    public interface ILayoutComplexFrameVisibility : IFocusComplexFrameVisibility, ILayoutFrameVisibility, ILayoutNodeFrameVisibility
    {
    }

    /// <summary>
    /// Frame visibility that depends on the IsComplex template property.
    /// </summary>
    public class LayoutComplexFrameVisibility : FocusComplexFrameVisibility, ILayoutComplexFrameVisibility
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="LayoutComplexFrameVisibility"/> object.
        /// </summary>
        public static new LayoutComplexFrameVisibility Empty { get; } = new LayoutComplexFrameVisibility();
        #endregion
    }
}
