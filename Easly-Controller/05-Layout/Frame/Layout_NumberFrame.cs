namespace EaslyController.Layout
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Constants;
    using EaslyController.Controller;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <summary>
    /// Frame describing a number string value property in a node.
    /// </summary>
    public interface ILayoutNumberFrame : IFocusNumberFrame, ILayoutValueFrame, ILayoutMeasurableFrame, ILayoutDrawableFrame, ILayoutPrintableFrame, ILayoutFrameWithMargins
    {
    }

    /// <summary>
    /// Frame describing a number string value property in a node.
    /// </summary>
    public class LayoutNumberFrame : FocusNumberFrame, ILayoutNumberFrame
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
        public virtual void Measure(ILayoutMeasureContext measureContext, ILayoutCellView cellView, ILayoutCellViewCollection collectionWithSeparator, ILayoutCellView referenceContainer, Measure separatorLength, out Size size, out Padding padding)
        {
            Node Node = cellView.StateView.State.Node;
            string Text = BaseNodeHelper.NodeTreeHelper.GetString(Node, PropertyName);

            size = measureContext.MeasureNumber(Text);
            measureContext.UpdatePadding(LeftMargin, RightMargin, ref size, out padding);

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
            Node Node = cellView.StateView.State.Node;
            string Text = BaseNodeHelper.NodeTreeHelper.GetString(Node, PropertyName);

            Point OriginWithPadding = origin.Moved(padding.Left, padding.Top);
            drawContext.DrawNumber(Text, OriginWithPadding); // The caret is drawn separately.
        }

        /// <summary>
        /// Prints a cell created with this frame.
        /// </summary>
        /// <param name="printContext">The context used to print the cell.</param>
        /// <param name="cellView">The cell to print.</param>
        /// <param name="origin">The location where to start printing.</param>
        /// <param name="size">The printing size, padding included.</param>
        /// <param name="padding">The padding to use when printing.</param>
        public virtual void Print(ILayoutPrintContext printContext, ILayoutCellView cellView, Point origin, Size size, Padding padding)
        {
            Node Node = cellView.StateView.State.Node;
            string Text = BaseNodeHelper.NodeTreeHelper.GetString(Node, PropertyName);

            Point OriginWithPadding = origin.Moved(padding.Left, padding.Top);
            printContext.PrintNumber(Text, OriginWithPadding);
        }
        #endregion

        #region Implementation
        private protected override void ValidateVisibleCellView(IFrameCellViewTreeContext context, IFrameVisibleCellView cellView)
        {
            Debug.Assert(((ILayoutVisibleCellView)cellView).StateView == ((ILayoutCellViewTreeContext)context).StateView);
            Debug.Assert(((ILayoutVisibleCellView)cellView).Frame == this);
            ILayoutCellViewCollection ParentCellView = ((ILayoutVisibleCellView)cellView).ParentCellView;
        }

        private protected override void ValidateEmptyCellView(IFocusCellViewTreeContext context, IFocusEmptyCellView emptyCellView)
        {
            Debug.Assert(((ILayoutEmptyCellView)emptyCellView).StateView == ((ILayoutCellViewTreeContext)context).StateView);
            ILayoutCellViewCollection ParentCellView = ((ILayoutEmptyCellView)emptyCellView).ParentCellView;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxTextFocusableCellView object.
        /// </summary>
        private protected override IFrameVisibleCellView CreateVisibleCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutNumberFrame));
            return new LayoutStringContentFocusableCellView((ILayoutNodeStateView)stateView, (ILayoutCellViewCollection)parentCellView, this, PropertyName);
        }

        /// <summary>
        /// Creates a IxxxEmptyCellView object.
        /// </summary>
        private protected override IFocusEmptyCellView CreateEmptyCellView(IFocusNodeStateView stateView, IFocusCellViewCollection parentCellView)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutNumberFrame));
            return new LayoutEmptyCellView((ILayoutNodeStateView)stateView, (ILayoutCellViewCollection)parentCellView);
        }
        #endregion
    }
}
