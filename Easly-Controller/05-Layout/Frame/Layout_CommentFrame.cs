namespace EaslyController.Layout
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Constants;
    using EaslyController.Controller;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <summary>
    /// Frame to display comments.
    /// </summary>
    public interface ILayoutCommentFrame : IFocusCommentFrame, ILayoutNodeFrame, ILayoutBlockFrame, ILayoutMeasurableFrame, ILayoutDrawableFrame
    {
    }

    /// <summary>
    /// Frame to display comments.
    /// </summary>
    public class LayoutCommentFrame : FocusCommentFrame, ILayoutCommentFrame
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
        /// Margin at the left side of the cell.
        /// </summary>
        public Margins LeftMargin { get { return Margins.None; } }

        /// <summary>
        /// Margin at the right side of the cell.
        /// </summary>
        public Margins RightMargin { get { return Margins.None; } }
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
            padding = Padding.Empty;

            INode Node = cellView.StateView.State.Node;
            string Text = Node.Documentation.Comment;
            if (!string.IsNullOrEmpty(Text))
                size = drawContext.MeasureText(Text, TextStyles.Comment, double.NaN);
            else
                size = Size.Empty;

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
        /// <param name="isFocused">True if this cell has the focus.</param>
        public virtual void Draw(ILayoutDrawContext drawContext, ILayoutCellView cellView, Point origin, Size size, Padding padding, bool isFocused)
        {
            INode Node = cellView.StateView.State.Node;
            string Text = Node.Documentation.Comment;

            if (!string.IsNullOrEmpty(Text))
            {
                Point OriginWithPadding = origin.Moved(padding.Left, padding.Top);
                drawContext.DrawText(Text, OriginWithPadding, TextStyles.Comment, false);
            }

            // The caret is drawn separately.
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
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxCommentCellView object.
        /// </summary>
        private protected override IFrameCommentCellView CreateCommentCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutCommentFrame));
            return new LayoutCommentCellView((ILayoutNodeStateView)stateView, (ILayoutCellViewCollection)parentCellView, this);
        }
        #endregion
    }
}
