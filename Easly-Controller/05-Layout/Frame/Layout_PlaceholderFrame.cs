namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Constants;
    using EaslyController.Controller;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <summary>
    /// Frame for describing an child node.
    /// </summary>
    public interface ILayoutPlaceholderFrame : IFocusPlaceholderFrame, ILayoutNamedFrame, ILayoutNodeFrameWithVisibility, ILayoutFrameWithSelector, ILayoutSelectorPropertyFrame, ILayoutMeasurableFrame, ILayoutFrameWithMargins
    {
    }

    /// <summary>
    /// Frame for describing an child node.
    /// </summary>
    public class LayoutPlaceholderFrame : FocusPlaceholderFrame, ILayoutPlaceholderFrame
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
        public new ILayoutFrameSelectorList Selectors { get { return (ILayoutFrameSelectorList)base.Selectors; } }

        /// <summary>
        /// Margin at the left side of the cell.
        /// (Set in Xaml)
        /// </summary>
        public Margins LeftMargin { get; set; }

        /// <summary>
        /// Margin at the right side of the cell.
        /// (Set in Xaml)
        /// </summary>
        public Margins RightMargin { get; set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Measures a cell created with this frame.
        /// </summary>
        /// <param name="measureContext">The context used to measure the cell.</param>
        /// <param name="cellView">The cell to measure.</param>
        /// <param name="collectionWithSeparator">A collection that can draw separators around the cell.</param>
        /// <param name="referenceContainer">The cell view in <paramref name="collectionWithSeparator"/> that contains this cell.</param>
        /// <param name="separatorLength">The length of the separator in <paramref name="collectionWithSeparator"/>.</param>
        /// <param name="size">The cell size upon return, padding included.</param>
        /// <param name="padding">The cell padding.</param>
        public virtual void Measure(ILayoutMeasureContext measureContext, ILayoutCellView cellView, ILayoutCellViewCollection collectionWithSeparator, ILayoutCellView referenceContainer, SeparatorLength separatorLength, out Size size, out Padding padding)
        {
            ILayoutContainerCellView ContainerCellView = cellView as ILayoutContainerCellView;
            Debug.Assert(ContainerCellView != null);

            ILayoutNodeStateView ChildStateView = ContainerCellView.ChildStateView;
            Debug.Assert(ChildStateView != null);
            ChildStateView.MeasureCells(collectionWithSeparator, referenceContainer, separatorLength);

            Debug.Assert(RegionHelper.IsValid(ChildStateView.CellSize));

            size = ChildStateView.CellSize;
            measureContext.UpdatePadding(LeftMargin, RightMargin, ref size, out padding);

            Debug.Assert(RegionHelper.IsValid(size));
        }
        #endregion

        #region Implementation
        /// <summary></summary>
        private protected override void ValidateContainerCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView, IFrameContainerCellView containerCellView)
        {
            Debug.Assert(((ILayoutContainerCellView)containerCellView).StateView == (ILayoutNodeStateView)stateView);
            Debug.Assert(((ILayoutContainerCellView)containerCellView).ParentCellView == (ILayoutCellViewCollection)parentCellView);
            Debug.Assert(((ILayoutContainerCellView)containerCellView).ChildStateView == (ILayoutNodeStateView)childStateView);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        private protected override IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutPlaceholderFrame));
            return new LayoutContainerCellView((ILayoutNodeStateView)stateView, (ILayoutCellViewCollection)parentCellView, (ILayoutNodeStateView)childStateView, this);
        }

        /// <summary>
        /// Creates a IxxxFrameSelectorList object.
        /// </summary>
        private protected override IFocusFrameSelectorList CreateEmptySelectorList()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutPlaceholderFrame));
            return new LayoutFrameSelectorList();
        }
        #endregion
    }
}
