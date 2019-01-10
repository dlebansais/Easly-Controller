using EaslyController.Frame;

namespace EaslyController.Focus
{
    /// <summary>
    /// Base frame for displaying more frames.
    /// </summary>
    public interface IFocusPanelFrame : IFramePanelFrame, IFocusFrame, IFocusNodeFrame, IFocusBlockFrame
    {
        /// <summary>
        /// List of frames within this frame.
        /// </summary>
        new IFocusFrameList Items { get; }
    }

    /// <summary>
    /// Base frame for displaying more frames.
    /// </summary>
    public abstract class FocusPanelFrame : FramePanelFrame, IFocusPanelFrame
    {
        #region Properties
        /// <summary>
        /// Parent template.
        /// </summary>
        public new IFocusTemplate ParentTemplate { get { return (IFocusTemplate)base.ParentTemplate; } }

        /// <summary>
        /// Parent frame, or null for the root frame in a template.
        /// </summary>
        public new IFocusFrame ParentFrame { get { return (IFocusFrame)base.ParentFrame; } }

        /// <summary>
        /// List of frames within this frame.
        /// </summary>
        public new IFocusFrameList Items { get { return (IFocusFrameList)base.Items; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxFrameList object.
        /// </summary>
        protected override IFrameFrameList CreateItems()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusPanelFrame));
            return new FocusFrameList();
        }

        /// <summary>
        /// Creates a IxxxCellViewList object.
        /// </summary>
        protected override IFrameCellViewList CreateCellViewList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusPanelFrame));
            return new FocusCellViewList();
        }

        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        protected override IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusPanelFrame));
            return new FocusContainerCellView((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView, (IFocusNodeStateView)childStateView);
        }
        #endregion
    }
}
