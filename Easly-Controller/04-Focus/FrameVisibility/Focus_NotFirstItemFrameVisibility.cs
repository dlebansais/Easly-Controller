namespace EaslyController.Focus
{
    using NotNullReflection;

    /// <summary>
    /// Frame visibility that depends if the current state is not the first in the parent.
    /// </summary>
    public interface IFocusNotFirstItemFrameVisibility : IFocusFrameVisibility, IFocusNodeFrameVisibility
    {
    }

    /// <summary>
    /// Frame visibility that depends if the current state is not the first in the parent.
    /// </summary>
    public class FocusNotFirstItemFrameVisibility : FocusFrameVisibility, IFocusNotFirstItemFrameVisibility
    {
        #region Properties
        /// <summary>
        /// True if the visibility depends on the show/hidden state of the view with the focus.
        /// </summary>
        public virtual bool IsVolatile { get { return false; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks that a frame visibility is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame visibility can describe.</param>
        public override bool IsValid(Type nodeType)
        {
            return true;
        }

        /// <summary>
        /// Is the associated frame visible.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="frame">The frame with the associated visibility.</param>
        public virtual bool IsVisible(IFocusCellViewTreeContext context, IFocusNodeFrameWithVisibility frame)
        {
            bool IsVisible = true;

            IsVisible &= !((IFocusInternalControllerView)context.ControllerView).IsFirstItem(context.StateView);

            return IsVisible;
        }
        #endregion
    }
}
