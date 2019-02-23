namespace EaslyController.Layout
{
    using System;
    using System.Diagnostics;
    using System.Windows.Markup;
    using EaslyController.Constants;
    using EaslyController.Controller;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <summary>
    /// Frame describing an enum value that can be displayed with different frames depending on its value.
    /// </summary>
    public interface ILayoutDiscreteFrame : IFocusDiscreteFrame, ILayoutValueFrame, ILayoutMeasurableFrame, ILayoutDrawableFrame, ILayoutFrameWithMargins
    {
        /// <summary>
        /// List of frames that can be displayed.
        /// (Set in Xaml)
        /// </summary>
        new ILayoutKeywordFrameList Items { get; }
    }

    /// <summary>
    /// Frame describing an enum value that can be displayed with different frames depending on its value.
    /// </summary>
    [ContentProperty("Items")]
    public class LayoutDiscreteFrame : FocusDiscreteFrame, ILayoutDiscreteFrame
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
        /// List of frames that can be displayed.
        /// (Set in Xaml)
        /// </summary>
        public new ILayoutKeywordFrameList Items { get { return (ILayoutKeywordFrameList)base.Items; } }

        /// <summary>
        /// Node frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        public new ILayoutNodeFrameVisibility Visibility { get { return (ILayoutNodeFrameVisibility)base.Visibility; } set { base.Visibility = value; } }

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
        /// Checks that a frame is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame can describe.</param>
        /// <param name="nodeTemplateTable">Table of templates with all frames.</param>
        public override bool IsValid(Type nodeType, IFrameTemplateReadOnlyDictionary nodeTemplateTable)
        {
            bool IsValid = true;

            IsValid &= base.IsValid(nodeType, nodeTemplateTable);

            foreach (ILayoutKeywordFrame Item in Items)
                IsValid &= (Item.LeftMargin == Margins.None) && (Item.RightMargin == Margins.None);

            Debug.Assert(IsValid);
            return IsValid;
        }

        /// <summary>
        /// Measures a cell created with this frame.
        /// </summary>
        /// <param name="drawContext">The context used to measure the cell.</param>
        /// <param name="cellView">The cell to measure.</param>
        /// <param name="collectionWithSeparator">A collection that can draw separators around the cell.</param>
        /// <param name="referenceContainer">The cell view in <paramref name="collectionWithSeparator"/> that contains this cell.</param>
        /// <param name="separatorLength">The length of the separator in <paramref name="collectionWithSeparator"/>.</param>
        /// <param name="size">The cell size upon return, padding included.</param>
        /// <param name="padding">The cell padding.</param>
        public virtual void Measure(ILayoutDrawContext drawContext, ILayoutCellView cellView, ILayoutCellViewCollection collectionWithSeparator, ILayoutCellView referenceContainer, double separatorLength, out Size size, out Padding padding)
        {
            ILayoutDiscreteContentFocusableCellView DiscreteContentFocusableCellView = cellView as ILayoutDiscreteContentFocusableCellView;
            Debug.Assert(DiscreteContentFocusableCellView != null);

            ILayoutKeywordFrame KeywordFrame = DiscreteContentFocusableCellView.KeywordFrame;
            Debug.Assert(KeywordFrame != null);

            KeywordFrame.Measure(drawContext, DiscreteContentFocusableCellView, collectionWithSeparator, referenceContainer, separatorLength, out size, out padding);

            Debug.Assert(padding.IsEmpty);
            drawContext.UpdatePadding(LeftMargin, RightMargin, ref size, out padding);

            Debug.Assert(RegionHelper.IsValid(size));
        }

        /// <summary>
        /// Draws a cell created with this frame.
        /// </summary>
        /// <param name="drawContext">The context used to draw the cell.</param>
        /// <param name="cellView">The cell to draw.</param>
        /// <param name="origin">The location where to start drawing.</param>
        /// <param name="size">The drawing size, padding included.</param>
        /// <param name="padding">The padding to use when drawing.</param>
        public virtual void Draw(ILayoutDrawContext drawContext, ILayoutCellView cellView, Point origin, Size size, Padding padding)
        {
            ILayoutDiscreteContentFocusableCellView DiscreteContentFocusableCellView = cellView as ILayoutDiscreteContentFocusableCellView;
            Debug.Assert(DiscreteContentFocusableCellView != null);

            ILayoutKeywordFrame KeywordFrame = DiscreteContentFocusableCellView.KeywordFrame;
            Debug.Assert(KeywordFrame != null);

            KeywordFrame.Draw(drawContext, DiscreteContentFocusableCellView, origin, size, padding);
        }
        #endregion

        #region Implementation
        /// <summary></summary>
        private protected override void ValidateDiscreteContentFocusableCellView(IFrameCellViewTreeContext context, IFrameKeywordFrame keywordFrame, IFrameDiscreteContentFocusableCellView cellView)
        {
            Debug.Assert(((ILayoutDiscreteContentFocusableCellView)cellView).StateView == ((ILayoutCellViewTreeContext)context).StateView);
            Debug.Assert(((ILayoutDiscreteContentFocusableCellView)cellView).Frame == this);
            Debug.Assert(((ILayoutDiscreteContentFocusableCellView)cellView).KeywordFrame == (ILayoutKeywordFrame)keywordFrame);
            ILayoutCellViewCollection ParentCellView = ((ILayoutDiscreteContentFocusableCellView)cellView).ParentCellView;
        }

        /// <summary></summary>
        private protected override void ValidateEmptyCellView(IFocusCellViewTreeContext context, IFocusEmptyCellView emptyCellView)
        {
            Debug.Assert(((ILayoutEmptyCellView)emptyCellView).StateView == ((ILayoutCellViewTreeContext)context).StateView);
            ILayoutCellViewCollection ParentCellView = ((ILayoutEmptyCellView)emptyCellView).ParentCellView;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxKeywordFrameList object.
        /// </summary>
        private protected override IFrameKeywordFrameList CreateKeywordFrameList()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutDiscreteFrame));
            return new LayoutKeywordFrameList();
        }

        /// <summary>
        /// Creates a IxxxDiscreteContentFocusableCellView object.
        /// </summary>
        private protected override IFrameDiscreteContentFocusableCellView CreateDiscreteContentFocusableCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameKeywordFrame keywordFrame)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutDiscreteFrame));
            return new LayoutDiscreteContentFocusableCellView((ILayoutNodeStateView)stateView, (ILayoutCellViewCollection)parentCellView, this, PropertyName, (ILayoutKeywordFrame)keywordFrame);
        }

        /// <summary>
        /// Creates a IxxxEmptyCellView object.
        /// </summary>
        private protected override IFocusEmptyCellView CreateEmptyCellView(IFocusNodeStateView stateView, IFocusCellViewCollection parentCellView)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutDiscreteFrame));
            return new LayoutEmptyCellView((ILayoutNodeStateView)stateView, (ILayoutCellViewCollection)parentCellView);
        }
        #endregion
    }
}
