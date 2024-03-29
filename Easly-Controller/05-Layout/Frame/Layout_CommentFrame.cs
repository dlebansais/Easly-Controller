﻿namespace EaslyController.Layout
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Constants;
    using EaslyController.Controller;
    using EaslyController.Focus;
    using EaslyController.Frame;
    using NotNullReflection;

    /// <summary>
    /// Frame to display comments.
    /// </summary>
    public interface ILayoutCommentFrame : IFocusCommentFrame, ILayoutNodeFrame, ILayoutBlockFrame, ILayoutMeasurableFrame, ILayoutDrawableFrame, ILayoutPrintableFrame
    {
        /// <summary>
        /// Prints a comment created with this frame.
        /// The comment string is provided explicitely.
        /// </summary>
        /// <param name="printContext">The context used to print the cell.</param>
        /// <param name="text">The text to print.</param>
        /// <param name="origin">The location where to start printing.</param>
        void Print(ILayoutPrintContext printContext, string text, Point origin);
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
            padding = Padding.Empty;

            ILayoutCommentCellView CommentCellView = cellView as ILayoutCommentCellView;
            Debug.Assert(CommentCellView != null);
            string Text = CommentHelper.Get(CommentCellView.Documentation);

            CommentDisplayModes DisplayMode = cellView.StateView.ControllerView.CommentDisplayMode;
            Debug.Assert(DisplayMode == CommentDisplayModes.OnFocus || DisplayMode == CommentDisplayModes.All);

            bool IsFocused = cellView.StateView.ControllerView.Focus.CellView == cellView;
            if (IsFocused && Text == null)
                Text = string.Empty;

            bool IsDisplayed = Text != null && ((DisplayMode == CommentDisplayModes.OnFocus && IsFocused) || DisplayMode == CommentDisplayModes.All);

            if (IsDisplayed)
                size = measureContext.MeasureText(Text, TextStyles.Comment, Controller.Measure.Floating);
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
        public virtual void Draw(ILayoutDrawContext drawContext, ILayoutCellView cellView, Point origin, Size size, Padding padding)
        {
            ILayoutCommentCellView CommentCellView = cellView as ILayoutCommentCellView;
            Debug.Assert(CommentCellView != null);
            string Text = CommentHelper.Get(CommentCellView.Documentation);

            CommentDisplayModes DisplayMode = cellView.StateView.ControllerView.CommentDisplayMode;
            Debug.Assert(DisplayMode == CommentDisplayModes.OnFocus || DisplayMode == CommentDisplayModes.All);

            bool IsFocused = cellView.StateView.ControllerView.Focus.CellView == cellView;
            if (IsFocused && Text == null)
                Text = string.Empty;

            if (Text != null)
            {
                if ((DisplayMode == CommentDisplayModes.OnFocus && IsFocused) || DisplayMode == CommentDisplayModes.All)
                {
                    Point OriginWithPadding = origin.Moved(padding.Left, padding.Top);
                    drawContext.DrawTextBackground(Text, OriginWithPadding, TextStyles.Comment);

                    LayoutControllerView ControllerView = cellView.StateView.ControllerView;
                    if (ControllerView.Selection is ILayoutCommentSelection AsCommentSelection && AsCommentSelection.StateView == cellView.StateView)
                    {
                        int Start = AsCommentSelection.Start;
                        int End = AsCommentSelection.End;
                        Debug.Assert(Start <= End);

                        drawContext.DrawSelectionText(Text, OriginWithPadding, TextStyles.Comment, Start, End);
                    }

                    drawContext.DrawText(Text, OriginWithPadding, TextStyles.Comment, isFocused: false); // The caret is drawn separately.
                }
                else if (DisplayMode == CommentDisplayModes.OnFocus && cellView.StateView.ControllerView.ShowUnfocusedComments)
                    drawContext.DrawCommentIcon(new Rect(cellView.CellOrigin, Size.Empty));
            }
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
            ILayoutCommentCellView CommentCellView = cellView as ILayoutCommentCellView;
            Debug.Assert(CommentCellView != null);
            string Text = CommentHelper.Get(CommentCellView.Documentation);

            if (Text != null)
            {
                CommentDisplayModes DisplayMode = cellView.StateView.ControllerView.CommentDisplayMode;
                Debug.Assert(DisplayMode == CommentDisplayModes.OnFocus || DisplayMode == CommentDisplayModes.All);

                if (DisplayMode == CommentDisplayModes.All)
                {
                    Point OriginWithPadding = origin.Moved(padding.Left, padding.Top);
                    printContext.PrintText(Text, OriginWithPadding, TextStyles.Comment);
                }
            }
        }

        /// <summary>
        /// Prints a comment created with this frame.
        /// The comment string is provided explicitely.
        /// </summary>
        /// <param name="printContext">The context used to print the cell.</param>
        /// <param name="text">The text to print.</param>
        /// <param name="origin">The location where to start printing.</param>
        public void Print(ILayoutPrintContext printContext, string text, Point origin)
        {
            printContext.PrintText(text, origin, TextStyles.Comment);
        }
        #endregion

        #region Implementation
        private protected override void ValidateVisibleCellView(IFrameCellViewTreeContext context, IFrameVisibleCellView cellView)
        {
            Debug.Assert(((ILayoutVisibleCellView)cellView).StateView == ((ILayoutCellViewTreeContext)context).StateView);
            Debug.Assert(((ILayoutVisibleCellView)cellView).Frame == this);
            ILayoutCellViewCollection ParentCellView = ((ILayoutVisibleCellView)cellView).ParentCellView;
        }

        private protected override void ValidateEmptyCellView(IFrameCellViewTreeContext context, IFrameEmptyCellView emptyCellView)
        {
            Debug.Assert(((ILayoutEmptyCellView)emptyCellView).StateView == ((ILayoutCellViewTreeContext)context).StateView);
            ILayoutCellViewCollection ParentCellView = ((ILayoutEmptyCellView)emptyCellView).ParentCellView;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxCommentCellView object.
        /// </summary>
        private protected override IFrameCommentCellView CreateCommentCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, Document documentation)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutCommentFrame>());
            return new LayoutCommentCellView((ILayoutNodeStateView)stateView, (ILayoutCellViewCollection)parentCellView, this, documentation);
        }

        /// <summary>
        /// Creates a IxxxEmptyCellView object.
        /// </summary>
        private protected override IFrameEmptyCellView CreateEmptyCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutCommentFrame>());
            return new LayoutEmptyCellView((ILayoutNodeStateView)stateView, (ILayoutCellViewCollection)parentCellView);
        }
        #endregion
    }
}
