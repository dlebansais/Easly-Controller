using EaslyController.Frame;

namespace EaslyController.Focus
{
    /// <summary>
    /// Context used when building the cell view tree.
    /// </summary>
    public interface IFocusCellViewTreeContext : IFrameCellViewTreeContext
    {
        /// <summary>
        /// The view in which cells are created.
        /// </summary>
        new IFocusControllerView ControllerView { get; }

        /// <summary>
        /// The state view for which to create cells.
        /// </summary>
        new IFocusNodeStateView StateView { get; }

        /// <summary>
        /// The block state view for which to create cells. Can be null.
        /// </summary>
        new IFocusBlockStateView BlockStateView { get; }

        /// <summary>
        /// True if cells are shown ccording to the frame visibility.
        /// </summary>
        bool IsFrameVisible { get; }

        /// <summary>
        /// True if the user requested to see elements that are otherwise not shown.
        /// </summary>
        bool IsUserVisible { get; }

        /// <summary>
        /// True if visibility allow the cell tree to be visible.
        /// </summary>
        bool IsVisible { get; }

        /// <summary>
        /// Update the current visibility of frames.
        /// </summary>
        /// <param name="frame">The frame with the visibility to check.</param>
        /// <param name="oldNodeFrameVisibility">The previous visibility upon return.</param>
        void UpdateNodeFrameVisibility(IFocusNodeFrame frame, out bool oldNodeFrameVisibility);

        /// <summary>
        /// Restores the frame visibility that was changed with <see cref="UpdateNodeFrameVisibility"/>.
        /// </summary>
        /// <param name="oldNodeFrameVisibility">The previous visibility</param>
        void RestoreFrameVisibility(bool oldNodeFrameVisibility);
    }

    /// <summary>
    /// Context used when building the cell view tree.
    /// </summary>
    public class FocusCellViewTreeContext : FrameCellViewTreeContext, IFocusCellViewTreeContext
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of a <see cref="FocusCellViewTreeContext"/> object.
        /// </summary>
        /// <param name="controllerView">The view in which cells are created.</param>
        /// <param name="stateView">The state view for which to create cells.</param>
        public FocusCellViewTreeContext(IFrameControllerView controllerView, IFrameNodeStateView stateView)
            : base(controllerView, stateView)
        {
            IsFrameVisible = true;
            IsUserVisible = false;
        }
        #endregion

        #region Client Interface
        /// <summary>
        /// Update the current visibility of frames.
        /// </summary>
        /// <param name="frame">The frame with the visibility to check.</param>
        /// <param name="oldNodeFrameVisibility">The previous visibility upon return.</param>
        public void UpdateNodeFrameVisibility(IFocusNodeFrame frame, out bool oldNodeFrameVisibility)
        {
            oldNodeFrameVisibility = IsFrameVisible;

            if (frame.Visibility != null)
            {
                bool IsVisible = frame.Visibility.IsVisible(this, frame);
                IsFrameVisible &= IsVisible;
            }
        }

        /// <summary>
        /// Restores the frame visibility that was changed with <see cref="UpdateNodeFrameVisibility"/>.
        /// </summary>
        /// <param name="oldNodeFrameVisibility">The previous visibility</param>
        public void RestoreFrameVisibility(bool oldNodeFrameVisibility)
        {
            IsFrameVisible = oldNodeFrameVisibility;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The view in which cells are created.
        /// </summary>
        public new IFocusControllerView ControllerView { get { return (IFocusControllerView)base.ControllerView; } }

        /// <summary>
        /// The state view for which to create cells.
        /// </summary>
        public new IFocusNodeStateView StateView { get { return (IFocusNodeStateView)base.StateView; } }

        /// <summary>
        /// The block state view for which to create cells. Can be null.
        /// </summary>
        public new IFocusBlockStateView BlockStateView { get { return (IFocusBlockStateView)base.BlockStateView; } }

        /// <summary>
        /// True if cells are shown ccording to the frame visibility.
        /// </summary>
        public bool IsFrameVisible { get; private set; }

        /// <summary>
        /// True if the user requested to see elements that are otherwise not shown.
        /// </summary>
        public bool IsUserVisible { get; private set; }

        /// <summary>
        /// True if visibility allow the cell tree to be visible.
        /// </summary>
        public bool IsVisible
        {
            get { return IsFrameVisible || IsUserVisible; }
        }
        #endregion
    }
}
