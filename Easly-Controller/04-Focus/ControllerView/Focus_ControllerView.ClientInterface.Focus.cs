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
    public partial class FocusControllerView : FrameControllerView, IFocusInternalControllerView
    {
        /// <summary>
        /// Moves the current focus in the focus chain.
        /// </summary>
        /// <param name="direction">The change in position, relative to the current position.</param>
        /// <param name="resetAnchor">If true, resets the selected text anchor.</param>
        /// <param name="isMoved">True upon return if the focus was moved.</param>
        public virtual void MoveFocus(int direction, bool resetAnchor, out bool isMoved)
        {
            ulong OldFocusHash = FocusHash;

            int OldFocusIndex = FocusChain.IndexOf(Focus);
            Debug.Assert(OldFocusIndex >= 0 && OldFocusIndex < FocusChain.Count);

            int NewFocusIndex = OldFocusIndex + direction;
            if (NewFocusIndex < 0)
                NewFocusIndex = 0;
            else if (NewFocusIndex >= FocusChain.Count)
                NewFocusIndex = FocusChain.Count - 1;

            Debug.Assert(NewFocusIndex >= 0 && NewFocusIndex < FocusChain.Count);

            if (OldFocusIndex != NewFocusIndex)
            {
                ChangeFocus(direction, OldFocusIndex, NewFocusIndex, resetAnchor, out bool IsRefreshed);
                isMoved = true;
            }
            else
                isMoved = false;

            Debug.Assert(isMoved || OldFocusHash == FocusHash);
        }

        private protected virtual void ChangeFocus(int direction, int oldIndex, int newIndex, bool resetAnchor, out bool isRefreshed)
        {
            isRefreshed = false;

            Focus = FocusChain[newIndex];

            if (FocusChain[oldIndex] is IFocusCommentFocus AsCommentFocus)
            {
                string Text = GetFocusedCommentText(AsCommentFocus);
                if (Text == null)
                {
                    Refresh(Controller.RootState);
                    isRefreshed = true;
                }
            }

            ResetCaretPosition(direction, resetAnchor);

            if (resetAnchor)
                ResetSelection();
            else
                SetAnchoredSelection();
        }

        private protected virtual void MoveFocusToState(IFocusNodeState state)
        {
            List<IFocusNodeStateView> StateViewList = new List<IFocusNodeStateView>();
            IFocusNodeStateView MainStateView = null;
            List<IFocusFocus> SameStateFocusableList = new List<IFocusFocus>();

            // Get the state that should have the focus and all its children.
            if (GetFocusedStateAndChildren(FocusChain, state, out MainStateView, out StateViewList, out SameStateFocusableList))
            {
                Debug.Assert(SameStateFocusableList.Count > 0);

                // Use a preferred frame.
                FindPreferredFrame(MainStateView, SameStateFocusableList);

                ResetCaretPosition(0, true);
            }
        }
    }
}
