namespace EaslyController.Layout
{
    using System.Diagnostics;
    using System.Windows.Markup;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <summary>
    /// Layout for bringing the focus to an insertion point.
    /// </summary>
    public interface ILayoutInsertFrame : IFocusInsertFrame, ILayoutStaticFrame
    {
    }

    /// <summary>
    /// Layout for bringing the focus to an insertion point.
    /// </summary>
    [ContentProperty("CollectionName")]
    public class LayoutInsertFrame : FocusInsertFrame, ILayoutInsertFrame
    {
        #region Properties
        /// <summary>
        /// Parent template.
        /// </summary>
        public new ILayoutTemplate ParentTemplate { get { return (ILayoutTemplate)base.ParentTemplate; } }

        /// <summary>
        /// Parent frame, or null for the root frame in a template.
        /// </summary>
        public new ILayoutFrame ParentFrame { get { return (ILayoutFrame)base.ParentFrame; } }

        /// <summary>
        /// Node frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        public new ILayoutNodeFrameVisibility Visibility { get { return (ILayoutNodeFrameVisibility)base.Visibility; } set { base.Visibility = value; } }
        #endregion

        #region Implementation
        /// <summary></summary>
        private protected override void ValidateVisibleCellView(IFrameCellViewTreeContext context, IFrameVisibleCellView cellView)
        {
            Debug.Assert(((ILayoutVisibleCellView)cellView).StateView == ((ILayoutCellViewTreeContext)context).StateView);
            Debug.Assert(((ILayoutVisibleCellView)cellView).Frame == this);
        }

        /// <summary></summary>
        private protected override void ValidateEmptyCellView(IFocusCellViewTreeContext context, IFocusEmptyCellView emptyCellView)
        {
            Debug.Assert(emptyCellView.StateView == context.StateView);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxFocusableCellView object.
        /// </summary>
        private protected override IFrameFocusableCellView CreateFocusableCellView(IFrameNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutInsertFrame));
            return new LayoutFocusableCellView((ILayoutNodeStateView)stateView, this);
        }

        // This class should not need CreateVisibleCellView().

        /// <summary>
        /// Creates a IxxxEmptyCellView object.
        /// </summary>
        private protected override IFocusEmptyCellView CreateEmptyCellView(IFocusNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutInsertFrame));
            return new LayoutEmptyCellView((ILayoutNodeStateView)stateView);
        }
        #endregion
    }
}
