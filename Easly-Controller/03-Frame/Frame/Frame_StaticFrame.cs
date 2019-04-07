namespace EaslyController.Frame
{
    using System.Diagnostics;

    /// <summary>
    /// Frame for decoration purpose only.
    /// </summary>
    public interface IFrameStaticFrame : IFrameFrame, IFrameNodeFrame
    {
    }

    /// <summary>
    /// Frame for decoration purpose only.
    /// </summary>
    public abstract class FrameStaticFrame : FrameFrame, IFrameStaticFrame
    {
        #region Properties
        private protected abstract bool IsFrameFocusable { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="parentCellView">The parent cell view.</param>
        public virtual IFrameCellView BuildNodeCells(IFrameCellViewTreeContext context, IFrameCellViewCollection parentCellView)
        {
            IFrameVisibleCellView CellView;

            if (IsFrameFocusable)
                CellView = CreateFocusableCellView(context.StateView, parentCellView);
            else
                CellView = CreateVisibleCellView(context.StateView, parentCellView);

            ValidateVisibleCellView(context, CellView);

            return CellView;
        }
        #endregion

        #region Implementation
        private protected virtual void ValidateVisibleCellView(IFrameCellViewTreeContext context, IFrameVisibleCellView cellView)
        {
            Debug.Assert(cellView.StateView == context.StateView);
            Debug.Assert(cellView.Frame == this);
            IFrameCellViewCollection ParentCellView = cellView.ParentCellView;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxFocusableCellView object.
        /// </summary>
        private protected virtual IFrameFocusableCellView CreateFocusableCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameStaticFrame));
            return new FrameFocusableCellView(stateView, parentCellView, this);
        }

        /// <summary>
        /// Creates a IxxxVisibleCellView object.
        /// </summary>
        private protected virtual IFrameVisibleCellView CreateVisibleCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameStaticFrame));
            return new FrameVisibleCellView(stateView, parentCellView, this);
        }
        #endregion
    }
}
