namespace EaslyController.Layout
{
    using System;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using EaslyController.Constants;
    using EaslyController.Controller;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <summary>
    /// View of a IxxxController.
    /// </summary>
    public partial class LayoutControllerView : FocusControllerView, ILayoutInternalControllerView
    {
        /// <summary>
        /// Invalidates the entire view.
        /// </summary>
        public virtual void Invalidate()
        {
            IsInvalidated = true;
        }

        /// <summary>
        /// Invalidates the specified region.
        /// </summary>
        public virtual void Invalidate(Rect region)
        {
            IsInvalidated = true;
        }

        /// <summary>
        /// Measure and arrange cells in the view.
        /// </summary>
        public virtual void MeasureAndArrange()
        {
            IsInvalidated = false;

            MeasureCells();
            ArrangeCells();
        }

        /// <summary>
        /// Updates all floating sizes and positions.
        /// </summary>
        public void UpdateLayout()
        {
            if (IsInvalidated)
                MeasureAndArrange();

            ILayoutNodeState RootState = Controller.RootState;
            ILayoutNodeStateView RootStateView = (ILayoutNodeStateView)StateViewTable[RootState];

            RootStateView.UpdateActualCellsSize();
            Debug.Assert(RegionHelper.IsValid(RootStateView.ActualCellSize));
        }

        /// <summary>
        /// Draws all visible cells in the view using <see cref="DrawContext"/>.
        /// <param name="stateView">The view to draw.</param>
        /// </summary>
        public virtual void Draw(ILayoutNodeStateView stateView)
        {
            UpdateLayout();

            if (IsCaretShown)
                DrawContext.HideCaret();

            Debug.Assert(RegionHelper.IsValid(stateView.ActualCellSize));
            stateView.DrawCells();

            if (ShowLineNumber)
                DisplayLineNumber(DrawCellViewLineNumber);

            if (IsCaretShown)
            {
                if (IsCaretOnText(out ILayoutStringContentFocus TextFocus))
                    DrawTextCaret(TextFocus);
                else if (IsCaretOnComment(out ILayoutCommentFocus CommentFocus))
                    DrawCommentCaret(CommentFocus);
            }
        }

        private protected virtual void DrawCellViewLineNumber(string lineText, Point lineOrigin)
        {
            DrawContext.DrawText(lineText, lineOrigin, TextStyles.LineNumber, isFocused: false);
        }

        /// <summary>
        /// Prints all visible cells in a view using <see cref="PrintContext"/>.
        /// </summary>
        /// <param name="stateView">The view to print.</param>
        /// <param name="origin">The origin from where to start printing.</param>
        public virtual void Print(ILayoutNodeStateView stateView, Point origin)
        {
            UpdateLayout();

            Debug.Assert(RegionHelper.IsValid(stateView.ActualCellSize));
            stateView.PrintCells(origin);

            if (ShowLineNumber)
                DisplayLineNumber(PrintCellViewLineNumber);
        }

        private protected virtual void PrintCellViewLineNumber(string lineText, Point lineOrigin)
        {
            PrintContext.PrintText(lineText, lineOrigin, TextStyles.LineNumber);
        }

        /// <summary>
        /// Prints the selection.
        /// </summary>
        public virtual void PrintSelection()
        {
            Selection.Print();
        }

        /// <summary>
        /// Shows or hides the caret.
        /// </summary>
        /// <param name="show">Shows the caret if true. Otherwise, hides it.</param>
        /// <param name="draw">Draws the caret according to <paramref name="show"/> if true. Otherwise, just save the setting.</param>
        public virtual void ShowCaret(bool show, bool draw)
        {
            if (IsCaretShown == show)
                return;

            IsCaretShown = show;

            if (draw)
                if (IsCaretShown)
                {
                    if (IsCaretOnText(out ILayoutStringContentFocus TextFocus))
                        DrawTextCaret(TextFocus);
                    else if (IsCaretOnComment(out ILayoutCommentFocus CommentFocus))
                        DrawCommentCaret(CommentFocus);
                    else
                        Focus.CellView.Draw();
                }
                else
                    DrawContext.HideCaret();
        }

        private protected bool IsCaretOnText(out ILayoutStringContentFocus textCellFocus)
        {
            textCellFocus = null;

            if (Focus is ILayoutStringContentFocus AsTextFocus)
            {
                bool IsHandled = false;

                switch (AsTextFocus.CellView.Frame)
                {
                    case ILayoutCharacterFrame AsCharacterFrame:
                        IsHandled = true; // The focus was displayed directly with the character.
                        break;

                    case ILayoutNumberFrame AsNumberFrame:
                    case ILayoutTextValueFrame AsTextValueFrame:
                        textCellFocus = AsTextFocus;
                        IsHandled = true;
                        break;
                }

                Debug.Assert(IsHandled);
            }

            return textCellFocus != null;
        }

        private protected void DrawTextCaret(ILayoutStringContentFocus textCellFocus)
        {
            ILayoutStringContentFocusableCellView CellView = textCellFocus.CellView;

            Node Node = CellView.StateView.State.Node;
            string PropertyName = CellView.PropertyName;
            string Text = NodeTreeHelper.GetString(Node, PropertyName);

            Point CellOrigin = CellView.CellOrigin;
            Padding CellPadding = CellView.CellPadding;

            Point OriginWithPadding = CellOrigin.Moved(CellPadding.Left, CellPadding.Top);
            DrawContext.ShowCaret(OriginWithPadding, Text, FocusedTextStyle, ActualCaretMode, CaretPosition);
        }

        private protected bool IsCaretOnComment(out ILayoutCommentFocus commentFocus)
        {
            commentFocus = Focus as ILayoutCommentFocus;
            return commentFocus != null;
        }

        private protected void DrawCommentCaret(ILayoutCommentFocus commentFocus)
        {
            ILayoutCommentCellView CellView = commentFocus.CellView;
            string Text = CommentHelper.Get(CellView.Documentation);
            if (Text == null)
                Text = string.Empty;

            Point CellOrigin = CellView.CellOrigin;
            Padding CellPadding = CellView.CellPadding;

            Point OriginWithPadding = CellOrigin.Moved(CellPadding.Left, CellPadding.Top);
            DrawContext.ShowCaret(OriginWithPadding, Text, TextStyles.Comment, ActualCaretMode, CaretPosition);
        }

        /// <summary>
        /// Moves the focus up or down.
        /// The starting point is the center of the area covered by the current focus.
        /// </summary>
        /// <param name="distance">The distance to cross.</param>
        /// <param name="resetAnchor">If true, resets the selected text anchor.</param>
        /// <param name="isMoved">True if the focus has changed.</param>
        public virtual void MoveFocusVertically(double distance, bool resetAnchor, out bool isMoved)
        {
            ulong OldFocusHash = FocusHash;

            int OldFocusIndex = FocusChain.IndexOf(Focus);
            int NewFocusIndex = -1;
            double BestVerticalDistance = 0;
            double BestHorizontalDistance = 0;

            FindClosestFocusVertical(distance, OldFocusIndex, ref NewFocusIndex, ref BestVerticalDistance, ref BestHorizontalDistance);

            // Always choose an extremum cell view if all are on the wrong side of the target.
            if (NewFocusIndex < 0)
                NewFocusIndex = (distance < 0) ? 0 : FocusChain.Count - 1;

            if (NewFocusIndex != OldFocusIndex)
            {
                Debug.Assert(NewFocusIndex >= MinFocusMove + OldFocusIndex && NewFocusIndex <= MaxFocusMove + OldFocusIndex);

                ChangeFocus(NewFocusIndex - OldFocusIndex, OldFocusIndex, NewFocusIndex, resetAnchor, out bool IsRefreshed);
                isMoved = true;
            }
            else
                isMoved = false;

            Debug.Assert(isMoved || OldFocusHash == FocusHash);
        }

        private void FindClosestFocusVertical(double distance, int oldFocusIndex, ref int newFocusIndex, ref double bestVerticalDistance, ref double bestHorizontalDistance)
        {
            Point Origin = GetMoveFocusDestinationLocation(distance);

            for (int i = 0; i < FocusChain.Count; i++)
            {
                int CellIndex = distance < 0 ? i : FocusChain.Count - i - 1;
                ILayoutFocus TestFocus = (ILayoutFocus)FocusChain[CellIndex];

                ILayoutFocusableCellView CellView = TestFocus.CellView;

                // If we check against the current focus, it might be the closest cell and then we don't move!
                if (CellIndex == oldFocusIndex)
                    continue;

                // Don't consider cells that are in the wrong direction;
                if ((distance < 0 && CellView.CellOrigin.Y.Draw >= Origin.Y.Draw) || (distance > 0 && CellView.CellOrigin.Y.Draw + CellView.CellSize.Height.Draw <= Origin.Y.Draw))
                    continue;

                if (CellView.CellRect.IsPointInRect(Origin.X.Draw, Origin.Y.Draw))
                {
                    newFocusIndex = CellIndex;
                    break;
                }
                else
                {
                    Point Center = CellView.CellRect.Center;

                    double VerticalDistance = Math.Abs((Center.Y - Origin.Y).Draw);
                    double HorizontalDistance = Math.Abs((Center.X - Origin.X).Draw);

                    if (newFocusIndex < 0 || bestVerticalDistance > VerticalDistance || (RegionHelper.IsZero(bestVerticalDistance - VerticalDistance) && bestHorizontalDistance > HorizontalDistance))
                    {
                        bestVerticalDistance = VerticalDistance;
                        bestHorizontalDistance = HorizontalDistance;
                        newFocusIndex = CellIndex;
                    }
                }
            }
        }

        private protected Point GetMoveFocusDestinationLocation(double distance)
        {
            Point Origin;

            if (Focus is ILayoutTextFocus AsTextFocus)
            {
                Debug.Assert(CaretPosition >= 0 && CaretPosition <= MaxCaretPosition);

                ILayoutTextFocusableCellView CellView = AsTextFocus.CellView;
                double Position;

                if (CaretMode == CaretModes.Override && CaretPosition < MaxCaretPosition)
                {
                    Position = (CellView.CellSize.Width.Draw * ((CaretPosition * 2) + 1)) / (MaxCaretPosition * 2);
                    Origin = CellView.CellOrigin.Moved(new Measure() { Draw = Position }, new Measure() { Draw = CellView.CellSize.Height.Draw / 2 });
                }
                else
                {
                    Position = (CellView.CellSize.Width.Draw * CaretPosition) / MaxCaretPosition;
                    Origin = CellView.CellOrigin.Moved(new Measure() { Draw = Position }, new Measure() { Draw = CellView.CellSize.Height.Draw / 2 });
                }
            }
            else
            {
                ILayoutFocusableCellView CellView = Focus.CellView;
                Origin = CellView.CellOrigin.Moved(new Measure() { Draw = CellView.CellSize.Width.Draw / 2 }, new Measure() { Draw = CellView.CellSize.Height.Draw / 2 });
            }

            Debug.Assert(RegionHelper.IsValid(Origin));

            Origin = Origin.Moved(Measure.Zero, new Measure() { Draw = distance });

            return Origin;
        }

        /// <summary>
        /// Moves the focus to the beginning or end of a line.
        /// The starting point is the center of the area covered by the current focus.
        /// </summary>
        /// <param name="direction">-1 for the beginning of the line, +1 for the end.</param>
        /// <param name="resetAnchor">If true, resets the selected text anchor.</param>
        /// <param name="isMoved">True if the focus has changed.</param>
        public virtual void MoveFocusHorizontally(int direction, bool resetAnchor, out bool isMoved)
        {
            Debug.Assert(direction != 0);

            ulong OldFocusHash = FocusHash;

            int OldFocusIndex = FocusChain.IndexOf(Focus);
            int NewFocusIndex = -1;
            double BestSquaredDistance = 0;

            FindClosestFocusHorizontal(direction, ref NewFocusIndex, ref BestSquaredDistance);

            if (NewFocusIndex >= 0 && NewFocusIndex != OldFocusIndex)
            {
                Debug.Assert(NewFocusIndex >= MinFocusMove + OldFocusIndex && NewFocusIndex <= MaxFocusMove + OldFocusIndex);

                ChangeFocus(direction, OldFocusIndex, NewFocusIndex, resetAnchor, out bool IsRefreshed);
                isMoved = true;

                resetAnchor = true;
            }
            else
                isMoved = false;

            // Also move the caret.
            if (Focus is ILayoutTextFocus AsTextFocus)
            {
                string Text = GetFocusedText(AsTextFocus);
                bool IsCaretMoved;

                if (direction < 0)
                    SetTextCaretPosition(Text, 0, resetAnchor, out IsCaretMoved);
                else
                    SetTextCaretPosition(Text, Text.Length, resetAnchor, out IsCaretMoved);

                isMoved |= IsCaretMoved;
            }

            Debug.Assert(isMoved || OldFocusHash == FocusHash);
        }

        private void FindClosestFocusHorizontal(int direction, ref int newFocusIndex, ref double bestSquaredDistance)
        {
            Point FocusCellOrigin = Focus.CellView.CellOrigin;
            Size FocusCellSize = Focus.CellView.CellSize;
            Point FocusCellCenter = Focus.CellView.CellRect.Center;

            for (int i = 0; i < FocusChain.Count; i++)
            {
                int CellIndex = direction < 0 ? i : FocusChain.Count - i - 1;
                ILayoutFocus TestFocus = (ILayoutFocus)FocusChain[CellIndex];

                ILayoutFocusableCellView TestCellView = TestFocus.CellView;
                Point TestCellOrigin = TestCellView.CellOrigin;
                Size TestCellSize = TestCellView.CellSize;
                Point TestCellCenter = TestCellView.CellRect.Center;

                // Don't consider cells that are not on the same line;
                if ((TestCellOrigin.Y.Draw + TestCellSize.Height.Draw <= FocusCellOrigin.Y.Draw) || (TestCellOrigin.Y.Draw >= FocusCellOrigin.Y.Draw + FocusCellSize.Height.Draw))
                    continue;

                // Don't consider cells that are in the wrong direction;
                if ((direction < 0 && TestCellOrigin.X.Draw >= FocusCellOrigin.X.Draw + FocusCellSize.Width.Draw) || (direction >= 0 && TestCellOrigin.X.Draw + TestCellSize.Width.Draw <= FocusCellOrigin.X.Draw))
                    continue;

                double SquaredDistance = Point.SquaredDistance(FocusCellCenter, TestCellCenter);

                if (newFocusIndex < 0 || bestSquaredDistance < SquaredDistance)
                {
                    bestSquaredDistance = SquaredDistance;
                    newFocusIndex = CellIndex;
                }
            }
        }

        /// <summary>
        /// Sets <see cref="ShowUnfocusedComments"/>.
        /// </summary>
        /// <param name="show">True to show, false to hide.</param>
        public virtual void SetShowUnfocusedComments(bool show)
        {
            if (ShowUnfocusedComments != show)
            {
                ShowUnfocusedComments = show;
                Refresh(Controller.RootState);
            }
        }

        /// <summary>
        /// Sets <see cref="ShowBlockGeometry"/>.
        /// </summary>
        /// <param name="show">True to show, false to hide.</param>
        public void SetShowBlockGeometry(bool show)
        {
            if (ShowBlockGeometry != show)
            {
                ShowBlockGeometry = show;
                Refresh(Controller.RootState);
            }
        }

        /// <summary>
        /// Sets <see cref="ShowLineNumber"/>.
        /// </summary>
        /// <param name="show">True to show, false to hide.</param>
        public virtual void SetShowLineNumber(bool show)
        {
            if (ShowLineNumber != show)
            {
                ShowLineNumber = show;
                Refresh(Controller.RootState);
            }
        }

        /// <summary>
        /// Gets the visible cell view corresponding to a location.
        /// </summary>
        /// <param name="x">X-coordinate of the location where to look.</param>
        /// <param name="y">Y-coordinate of the location where to look.</param>
        /// <param name="cellView">The cell view upon return. Null if not found.</param>
        /// <returns>True if found; otherwise, false.</returns>
        public virtual bool CellViewFromPoint(double x, double y, out ILayoutVisibleCellView cellView)
        {
            EnumerateVisibleCellViews((IFrameVisibleCellView item) => ((ILayoutVisibleCellView)item).CellRect.IsPointInRect(x, y), out IFrameVisibleCellView foundCellView, reversed: false);
            cellView = (ILayoutVisibleCellView)foundCellView;

            return cellView != null;
        }

        /// <summary>
        /// Sets the focus to the visible cell view corresponding to a location.
        /// </summary>
        /// <param name="x">X-coordinate of the location where to set the focus.</param>
        /// <param name="y">Y-coordinate of the location where to set the focus.</param>
        /// <param name="resetAnchor">If true, resets the selected text anchor.</param>
        /// <param name="isMoved">True if the focus was moved.</param>
        public virtual void SetFocusToPoint(double x, double y, bool resetAnchor, out bool isMoved)
        {
            isMoved = false;
            ulong OldFocusHash = FocusHash;
            int OldIndex = FocusChain.IndexOf(Focus);
            int NewIndex = -1;

            // Takes page margins and padding into account.
            DrawContext.ToRelativeLocation(ref x, ref y);

            for (int i = 0; i < FocusChain.Count; i++)
            {
                ILayoutFocus TestFocus = (ILayoutFocus)FocusChain[i];
                if (TestFocus.CellView.CellRect.IsPointInRect(x, y))
                {
                    NewIndex = i;
                    break;
                }
            }

            if (NewIndex >= 0)
            {
                if (NewIndex != OldIndex)
                    ChangeFocus(NewIndex - OldIndex, OldIndex, NewIndex, resetAnchor, out bool IsRefreshed);

                if (Focus is ILayoutTextFocus AsTextFocus)
                {
                    Point CellOrigin = Focus.CellView.CellOrigin;
                    Size CellSize = Focus.CellView.CellSize;

                    double XRelativeToCell = x - CellOrigin.X.Draw;
                    double YRelativeToCell = y - CellOrigin.Y.Draw;
                    Debug.Assert(XRelativeToCell >= 0 && XRelativeToCell < CellSize.Width.Draw);
                    Debug.Assert(YRelativeToCell >= 0 && YRelativeToCell < CellSize.Height.Draw);

                    string Text = GetFocusedText(AsTextFocus);
                    int NewCaretPosition = DrawContext.GetCaretPositionInText(XRelativeToCell, Text, FocusedTextStyle, CaretMode, Measure.Floating);
                    Debug.Assert(NewCaretPosition >= 0 && NewCaretPosition <= MaxCaretPosition);

                    SetTextCaretPosition(Text, NewCaretPosition, resetAnchor, out isMoved);
                }

                isMoved = true;
            }

            Debug.Assert(isMoved || OldFocusHash == FocusHash);
        }
    }
}
