namespace EaslyController.Focus
{
    using System;

    /// <summary>
    /// Frame visibility that depends on the IsTextMatch template property.
    /// </summary>
    public interface IFocusTextMatchFrameVisibility : IFocusFrameVisibility, IFocusNodeFrameVisibility
    {
        /// <summary>
        /// Name of the string property to check.
        /// (Set in Xaml)
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Text that must match to show the frame.
        /// (Set in Xaml)
        /// </summary>
        string TextPattern { get; }
    }

    /// <summary>
    /// Frame visibility that depends on the IsTextMatch template property.
    /// </summary>
    public class FocusTextMatchFrameVisibility : FocusFrameVisibility, IFocusTextMatchFrameVisibility
    {
        #region Properties
        /// <summary>
        /// True if the visibility depends on the show/hidden state of the view with the focus.
        /// </summary>
        public virtual bool IsVolatile { get { return false; } }

        /// <summary>
        /// Name of the string property to check.
        /// (Set in Xaml)
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Text that must match to show the frame.
        /// (Set in Xaml)
        /// </summary>
        public string TextPattern { get; set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks that a frame visibility is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame visibility can describe.</param>
        public override bool IsValid(Type nodeType)
        {
            bool IsValid = true;

            IsValid &= !string.IsNullOrEmpty(TextPattern);

            return IsValid;
        }

        /// <summary>
        /// Is the associated frame visible.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="frame">The frame with the associated visibility.</param>
        public virtual bool IsVisible(IFocusCellViewTreeContext context, IFocusNodeFrameWithVisibility frame)
        {
            bool IsVisible = true;

            IsVisible &= !((IFocusInternalControllerView)context.ControllerView).StringMatchTextPattern(context.StateView, PropertyName, TextPattern);

            return IsVisible;
        }
        #endregion
    }
}
