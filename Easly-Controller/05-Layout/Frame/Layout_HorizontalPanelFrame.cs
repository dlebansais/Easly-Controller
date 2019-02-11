namespace EaslyController.Layout
{
    using System.Diagnostics;
    using System.Windows.Markup;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <summary>
    /// Layout for displaying more frames horizontally.
    /// </summary>
    public interface ILayoutHorizontalPanelFrame : IFocusHorizontalPanelFrame, ILayoutPanelFrame
    {
    }

    /// <summary>
    /// Layout for displaying more frames horizontally.
    /// </summary>
    [ContentProperty("Items")]
    public class LayoutHorizontalPanelFrame : FocusHorizontalPanelFrame, ILayoutHorizontalPanelFrame
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
        /// List of frames within this frame.
        /// </summary>
        public new ILayoutFrameList Items { get { return (ILayoutFrameList)base.Items; } }

        /// <summary>
        /// Node frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        public new ILayoutNodeFrameVisibility Visibility { get { return (ILayoutNodeFrameVisibility)base.Visibility; } set { base.Visibility = value; } }

        /// <summary>
        /// Block frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        public new ILayoutBlockFrameVisibility BlockVisibility { get { return (ILayoutBlockFrameVisibility)base.BlockVisibility; } set { base.BlockVisibility = value; } }
        #endregion

        #region Implementation
        /// <summary></summary>
        private protected override void ValidateEmbeddingCellView(IFrameCellViewTreeContext context, IFrameCellViewCollection embeddingCellView)
        {
            Debug.Assert(((ILayoutCellViewCollection)embeddingCellView).StateView == ((ILayoutCellViewTreeContext)context).StateView);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxFrameList object.
        /// </summary>
        private protected override IFrameFrameList CreateItems()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutHorizontalPanelFrame));
            return new LayoutFrameList();
        }

        /// <summary>
        /// Creates a IxxxCellViewList object.
        /// </summary>
        private protected override IFrameCellViewList CreateCellViewList()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutHorizontalPanelFrame));
            return new LayoutCellViewList();
        }

        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        private protected override IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutHorizontalPanelFrame));
            return new LayoutContainerCellView((ILayoutNodeStateView)stateView, (ILayoutCellViewCollection)parentCellView, (ILayoutNodeStateView)childStateView);
        }

        /// <summary>
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        private protected override IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutHorizontalPanelFrame));
            return new LayoutLine((ILayoutNodeStateView)stateView, (ILayoutCellViewList)list);
        }
        #endregion
    }
}
