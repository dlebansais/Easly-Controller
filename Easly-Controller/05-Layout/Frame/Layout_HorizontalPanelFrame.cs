namespace EaslyController.Layout
{
    using System;
    using System.Diagnostics;
    using System.Windows.Markup;
    using EaslyController.Constants;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <summary>
    /// Frame for displaying more frames horizontally.
    /// </summary>
    public interface ILayoutHorizontalPanelFrame : IFocusHorizontalPanelFrame, ILayoutPanelFrame, ILayoutFrameWithHorizontalSeparator
    {
    }

    /// <summary>
    /// Frame for displaying more frames horizontally.
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
        public new LayoutFrameList Items { get { return (LayoutFrameList)base.Items; } }

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

        /// <summary>
        /// Indicates that block geometry must be drawn around a block.
        /// (Set in Xaml)
        /// </summary>
        public bool HasBlockGeometry { get; set; }

        /// <summary>
        /// Horizontal separator.
        /// (Set in Xaml)
        /// </summary>
        public HorizontalSeparators Separator { get; set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks that a frame is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame can describe.</param>
        /// <param name="nodeTemplateTable">Table of templates with all frames.</param>
        /// <param name="commentFrameCount">Number of comment frames found so far.</param>
        public override bool IsValid(Type nodeType, FrameTemplateReadOnlyDictionary nodeTemplateTable, ref int commentFrameCount)
        {
            bool IsValid = true;

            IsValid &= base.IsValid(nodeType, nodeTemplateTable, ref commentFrameCount);
            IsValid &= !HasBlockGeometry || (ParentTemplate is ILayoutBlockTemplate);
            IsValid &= !HasBlockGeometry || (ParentFrame.ParentTemplate == null);

            Debug.Assert(IsValid);
            return IsValid;
        }
        #endregion

        #region Implementation
        private protected override void ValidateEmbeddingCellView(IFrameCellViewTreeContext context, IFrameCellViewCollection embeddingCellView)
        {
            Debug.Assert(((ILayoutCellViewCollection)embeddingCellView).StateView == ((ILayoutCellViewTreeContext)context).StateView);
            ILayoutCellViewCollection ParentCellView = ((ILayoutCellViewCollection)embeddingCellView).ParentCellView;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxFrameList object.
        /// </summary>
        private protected override FrameFrameList CreateItems()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutHorizontalPanelFrame));
            return new LayoutFrameList();
        }

        /// <summary>
        /// Creates a IxxxCellViewList object.
        /// </summary>
        private protected override FrameCellViewList CreateCellViewList()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutHorizontalPanelFrame));
            return new LayoutCellViewList();
        }

        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        private protected override IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView, IFramePlaceholderFrame frame)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutHorizontalPanelFrame));
            return new LayoutContainerCellView((ILayoutNodeStateView)stateView, (ILayoutCellViewCollection)parentCellView, (ILayoutNodeStateView)childStateView, (ILayoutPlaceholderFrame)frame);
        }

        /// <summary>
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        private protected override IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, FrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutHorizontalPanelFrame));
            return new LayoutLine((ILayoutNodeStateView)stateView, (ILayoutCellViewCollection)parentCellView, (LayoutCellViewList)list, this);
        }
        #endregion
    }
}
