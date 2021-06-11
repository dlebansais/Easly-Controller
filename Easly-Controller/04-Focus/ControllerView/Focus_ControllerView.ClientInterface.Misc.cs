namespace EaslyController.Focus
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows;
    using BaseNode;
    using BaseNodeHelper;
    using EaslyController.Constants;
    using EaslyController.Controller;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;

    /// <summary>
    /// View of a IxxxController.
    /// </summary>
    public partial class FocusControllerView : FrameControllerView, IFocusControllerView, IFocusInternalControllerView
    {
        /// <summary>
        /// Sets the node with the focus to have all its frames visible.
        /// If another node had this flag set, it is reset, regardless of the value of <paramref name="isUserVisible"/>.
        /// </summary>
        public virtual void SetUserVisible(bool isUserVisible)
        {
            if (isUserVisible)
                foreach (KeyValuePair<IFocusNodeState, IFocusNodeStateView> Entry in StateViewTable)
                    Entry.Value.SetIsUserVisible(false);

            IFocusNodeStateView StateView = Focus.CellView.StateView;
            Debug.Assert(StateView != null);

            while (true)
            {
                StateView.SetIsUserVisible(isUserVisible);

                foreach (KeyValuePair<string, IFocusInner> Entry in StateView.State.InnerTable)
                {
                    if (Entry.Value is IFocusPlaceholderInner AsPlaceholderInner)
                    {
                        IFocusNodeStateView ChildStateView = StateViewTable[AsPlaceholderInner.ChildState];
                        if (((IFocusNodeTemplate)ChildStateView.Template).IsSimple)
                            ChildStateView.SetIsUserVisible(isUserVisible);
                    }
                }

                if (!((IFocusNodeTemplate)StateView.Template).IsSimple || StateView.State.ParentState == null)
                    break;

                StateView = StateViewTable[StateView.State.ParentState];
            }

            Refresh(StateView.State);
        }

        /// <summary>
        /// Force the comment attached to the node with the focus to show, if empty, and move the focus to this comment.
        /// </summary>
        public virtual void ForceShowComment(out bool isMoved)
        {
            IFocusNodeState State = Focus.CellView.StateView.State;
            IDocument Documentation;
            if (State is IFocusOptionalNodeState AsOptionalNodeState)
            {
                Debug.Assert(AsOptionalNodeState.ParentInner.IsAssigned);
                Documentation = AsOptionalNodeState.Node.Documentation;
            }
            else
                Documentation = State.Node.Documentation;

            isMoved = false;
            ulong OldFocusHash = FocusHash;

            if (!(Focus is IFocusCommentFocus))
            {
                string Text = CommentHelper.Get(Documentation);
                if (Text == null)
                {
                    IFocusNodeStateView StateView = Focus.CellView.StateView;
                    ForcedCommentStateView = StateView;

                    Refresh(Controller.RootState);
                    Debug.Assert(ForcedCommentStateView == null);

                    for (int i = 0; i < FocusChain.Count; i++)
                        if (FocusChain[i] is IFocusCommentFocus AsCommentFocus && AsCommentFocus.CellView.StateView == StateView)
                        {
                            int OldFocusIndex = FocusChain.IndexOf(Focus);
                            Debug.Assert(OldFocusIndex >= 0); // The old focus must have been preserved.

                            int NewFocusIndex = i;

                            ChangeFocus(NewFocusIndex - OldFocusIndex, OldFocusIndex, NewFocusIndex, true, out bool IsRefreshed);
                            Debug.Assert(!IsRefreshed); // Refresh must not be done twice.

                            isMoved = true;
                            break;
                        }

                    Debug.Assert(isMoved);
                }
            }

            if (isMoved)
                ResetSelection();

            Debug.Assert(isMoved || OldFocusHash == FocusHash);
        }

        /// <summary>
        /// Changes the value of a text. The caret position is also moved for this view and other views where the caret is at the same focus and position.
        /// </summary>
        /// <param name="newText">The new text.</param>
        /// <param name="newCaretPosition">The new caret position.</param>
        /// <param name="changeCaretBeforeText">True if the caret should be changed before the text, to preserve the caret invariant.</param>
        public virtual void ChangeFocusedText(string newText, int newCaretPosition, bool changeCaretBeforeText)
        {
            Debug.Assert(Focus is IFocusTextFocus);

            bool IsHandled = false;
            IFocusNodeState State = Focus.CellView.StateView.State;
            IFocusIndex ParentIndex = State.ParentIndex;
            int OldCaretPosition = CaretPosition;

            if (Focus is IFocusStringContentFocus AsStringContentFocus)
            {
                IFocusStringContentFocusableCellView CellView = AsStringContentFocus.CellView;
                IFocusTextValueFrame Frame = CellView.Frame as IFocusTextValueFrame;
                Debug.Assert(Frame != null);

                if (Frame.AutoFormat)
                    switch (AutoFormatMode)
                    {
                        case AutoFormatModes.None:
                            IsHandled = true;
                            break;

                        case AutoFormatModes.FirstOnly:
                            newText = StringHelper.FirstOnlyFormattedText(newText);
                            IsHandled = true;
                            break;

                        case AutoFormatModes.FirstOrAll:
                            newText = StringHelper.FirstOrAllFormattedText(newText);
                            IsHandled = true;
                            break;

                        case AutoFormatModes.AllLowercase:
                            newText = StringHelper.AllLowercaseFormattedText(newText);
                            IsHandled = true;
                            break;
                    }
                else
                    IsHandled = true;

                Controller.ChangeTextAndCaretPosition(ParentIndex, AsStringContentFocus.CellView.PropertyName, newText, OldCaretPosition, newCaretPosition, changeCaretBeforeText);
            }
            else if (Focus is IFocusCommentFocus AsCommentFocus)
            {
                Controller.ChangeCommentAndCaretPosition(ParentIndex, newText, OldCaretPosition, newCaretPosition, changeCaretBeforeText);
                IsHandled = true;
            }

            Debug.Assert(IsHandled);
        }

        /// <summary>
        /// Change auto formatting mode.
        /// </summary>
        public virtual void SetAutoFormatMode(AutoFormatModes mode)
        {
            if (AutoFormatMode != mode)
                AutoFormatMode = mode;
        }
    }
}
