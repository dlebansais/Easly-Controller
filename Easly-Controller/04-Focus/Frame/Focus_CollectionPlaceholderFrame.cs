using EaslyController.Frame;

namespace EaslyController.Focus
{
    /// <summary>
    /// Base frame for a placeholder node in a block list.
    /// </summary>
    public interface IFocusCollectionPlaceholderFrame : IFrameCollectionPlaceholderFrame, IFocusFrame, IFocusBlockFrame
    {
    }

    /// <summary>
    /// Base frame for a placeholder node in a block list.
    /// </summary>
    public abstract class FocusCollectionPlaceholderFrame : FrameCollectionPlaceholderFrame, IFocusCollectionPlaceholderFrame
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
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxCellViewList object.
        /// </summary>
        protected override IFrameCellViewList CreateCellViewList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusCollectionPlaceholderFrame));
            return new FocusCellViewList();
        }

        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        protected override IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusCollectionPlaceholderFrame));
            return new FocusContainerCellView((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView, (IFocusNodeStateView)childStateView);
        }
        #endregion
    }
}
