namespace EaslyController.Layout
{
    using System.Diagnostics;
    using System.Windows.Markup;
    using EaslyController.Constants;
    using EaslyController.Controller;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <summary>
    /// Frame for decoration purpose only.
    /// </summary>
    public interface ILayoutKeywordFrame : IFocusKeywordFrame, ILayoutFrame, ILayoutBlockFrame, ILayoutNodeFrameWithVisibility, ILayoutMeasurableFrame, ILayoutDrawableFrame
    {
    }

    /// <summary>
    /// Frame for decoration purpose only.
    /// </summary>
    [ContentProperty("Text")]
    public class LayoutKeywordFrame : FocusKeywordFrame, ILayoutKeywordFrame
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
        /// Block frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        public new ILayoutBlockFrameVisibility BlockVisibility { get { return (ILayoutBlockFrameVisibility)base.BlockVisibility; } set { base.BlockVisibility = value; } }

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

        /// <summary></summary>
        protected virtual TextStyles TextStyle
        {
            get
            {
                if (ParentFrame is ILayoutDiscreteFrame)
                    return TextStyles.Discrete;
                else
                    return TextStyles.Keyword;
            }
        }
        #endregion

        #region Client Interface
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
            size = drawContext.MeasureText(Text, TextStyle);
            drawContext.UpdatePadding(LeftMargin, RightMargin, ref size, out padding);

            Debug.Assert(MeasureHelper.IsValid(size));
        }

        /// <summary>
        /// Draws a cell created with this frame.
        /// </summary>
        /// <param name="drawContext">The context used to draw the cell.</param>
        /// <param name="cellView">The cell to draw.</param>
        /// <param name="origin">The location where to start drawing.</param>
        /// <param name="size">The drawing size, padding included.</param>
        /// <param name="padding">The padding to use when drawing.</param>
        /// <param name="isFocused">True if this cell has the focus.</param>
        public virtual void Draw(ILayoutDrawContext drawContext, ILayoutCellView cellView, Point origin, Size size, Padding padding, bool isFocused)
        {
            Point OriginWithPadding = new Point(origin.X + padding.Left, origin.Y + padding.Top);
            drawContext.DrawText(Text, OriginWithPadding, TextStyle, isFocused);
        }
        #endregion

        #region Implementation
        /// <summary></summary>
        private protected override void ValidateVisibleCellView(IFrameCellViewTreeContext context, IFrameVisibleCellView cellView)
        {
            Debug.Assert(((ILayoutVisibleCellView)cellView).StateView == ((ILayoutCellViewTreeContext)context).StateView);
            Debug.Assert(((ILayoutVisibleCellView)cellView).Frame == this);
            ILayoutCellViewCollection ParentCellView = ((ILayoutVisibleCellView)cellView).ParentCellView;
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
        /// Creates a IxxxFocusableCellView object.
        /// </summary>
        private protected override IFrameFocusableCellView CreateFocusableCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutKeywordFrame));
            return new LayoutFocusableCellView((ILayoutNodeStateView)stateView, (ILayoutCellViewCollection)parentCellView, this);
        }

        /// <summary>
        /// Creates a IxxxVisibleCellView object.
        /// </summary>
        private protected override IFrameVisibleCellView CreateVisibleCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutKeywordFrame));
            return new LayoutVisibleCellView((ILayoutNodeStateView)stateView, (ILayoutCellViewCollection)parentCellView, this);
        }

        /// <summary>
        /// Creates a IxxxEmptyCellView object.
        /// </summary>
        private protected override IFocusEmptyCellView CreateEmptyCellView(IFocusNodeStateView stateView, IFocusCellViewCollection parentCellView)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutKeywordFrame));
            return new LayoutEmptyCellView((ILayoutNodeStateView)stateView, (ILayoutCellViewCollection)parentCellView);
        }
        #endregion
    }
}
