namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Constants;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <summary>
    /// Frame for a block list displayed horizontally.
    /// </summary>
    public interface ILayoutHorizontalBlockListFrame : IFocusHorizontalBlockListFrame, ILayoutBlockListFrame, ILayoutFrameWithHorizontalSeparator
    {
    }

    /// <summary>
    /// Frame for a block list displayed horizontally.
    /// </summary>
    public class LayoutHorizontalBlockListFrame : FocusHorizontalBlockListFrame, ILayoutHorizontalBlockListFrame
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

        /// <summary>
        /// List of optional selectors.
        /// (Set in Xaml)
        /// </summary>
        public new LayoutFrameSelectorList Selectors { get { return (LayoutFrameSelectorList)base.Selectors; } }

        /// <summary>
        /// Horizontal separator.
        /// (Set in Xaml)
        /// </summary>
        public HorizontalSeparators Separator { get; set; }
        #endregion

        #region Implementation
        private protected override void ValidateEmbeddingCellView(IFrameCellViewTreeContext context, IFrameCellViewCollection embeddingCellView)
        {
            Debug.Assert(((ILayoutCellViewCollection)embeddingCellView).StateView == ((ILayoutCellViewTreeContext)context).StateView);
            ILayoutCellViewCollection ParentCellView = ((ILayoutCellViewCollection)embeddingCellView).ParentCellView;
        }

        private protected override void ValidateBlockCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, FrameBlockStateView blockStateView, IFrameBlockCellView blockCellView)
        {
            Debug.Assert(((ILayoutBlockCellView)blockCellView).StateView == (ILayoutNodeStateView)stateView);
            Debug.Assert(((ILayoutBlockCellView)blockCellView).ParentCellView == (ILayoutCellViewCollection)parentCellView);
            Debug.Assert(((ILayoutBlockCellView)blockCellView).BlockStateView == (LayoutBlockStateView)blockStateView);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxCellViewList object.
        /// </summary>
        private protected override FrameCellViewList CreateCellViewList()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutHorizontalBlockListFrame));
            return new LayoutCellViewList();
        }

        /// <summary>
        /// Creates a IxxxBlockCellView object.
        /// </summary>
        private protected override IFrameBlockCellView CreateBlockCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, FrameBlockStateView blockStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutHorizontalBlockListFrame));
            return new LayoutBlockCellView((ILayoutNodeStateView)stateView, (ILayoutCellViewCollection)parentCellView, (LayoutBlockStateView)blockStateView);
        }

        /// <summary>
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        private protected override IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, FrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutHorizontalBlockListFrame));
            return new LayoutLine((ILayoutNodeStateView)stateView, (ILayoutCellViewCollection)parentCellView, (LayoutCellViewList)list, this);
        }

        /// <summary>
        /// Creates a IxxxFrameSelectorList object.
        /// </summary>
        private protected override FocusFrameSelectorList CreateEmptySelectorList()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutHorizontalBlockListFrame));
            return new LayoutFrameSelectorList();
        }
        #endregion
    }
}
