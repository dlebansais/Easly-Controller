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
        private protected virtual void SetAnchoredSelection()
        {
            IFocusNodeState AnchorState = SelectionAnchor.State;
            IFocusNodeState FocusedState = Focus.CellView.StateView.State;

            if (AnchorState == FocusedState)
            {
                if (Focus is IFocusTextFocus AsTextFocus)
                    SetTextSelection(AsTextFocus);
                else
                    ResetSelection();
            }
            else
                SetAnchoredNodeSelection();
        }

        private protected virtual void SetAnchoredNodeSelection()
        {
            IFocusNodeState AnchorState = SelectionAnchor.State;
            IFocusNodeState FocusedState = Focus.CellView.StateView.State;

            GetFirstFocusedChildState(out IFocusNodeState State, out IReadOnlyIndex FirstFocusedIndex);
            Debug.Assert(State != null);

            bool IsAnchorChild = Controller.IsChildState(State, AnchorState, out IReadOnlyIndex FirstAnchorIndex);
            Debug.Assert(IsAnchorChild);

            bool IsFromPatternOrSource = (AnchorState is IFocusPatternState) || (AnchorState is IFocusSourceState) || (FocusedState is IFocusPatternState) || (FocusedState is IFocusSourceState);

            if (FirstFocusedIndex is IFocusBrowsingListNodeIndex AsFocusListNodeIndex && FirstAnchorIndex is IFocusBrowsingListNodeIndex AsAnchorListNodeIndex && AsFocusListNodeIndex.PropertyName == AsAnchorListNodeIndex.PropertyName)
                SelectNodeList(State, AsFocusListNodeIndex.PropertyName, AsFocusListNodeIndex.Index, AsAnchorListNodeIndex.Index);
            else if (FirstFocusedIndex is IFocusBrowsingExistingBlockNodeIndex AsFocusBlockListNodeIndex && FirstAnchorIndex is IFocusBrowsingExistingBlockNodeIndex AsAnchorBlockListNodeIndex && AsFocusBlockListNodeIndex.PropertyName == AsAnchorBlockListNodeIndex.PropertyName)
            {
                if (AsFocusBlockListNodeIndex.BlockIndex == AsAnchorBlockListNodeIndex.BlockIndex && !IsFromPatternOrSource)
                    SelectBlockNodeList(State, AsFocusBlockListNodeIndex.PropertyName, AsFocusBlockListNodeIndex.BlockIndex, AsFocusBlockListNodeIndex.Index, AsAnchorBlockListNodeIndex.Index);
                else
                    SelectBlockList(State, AsFocusBlockListNodeIndex.PropertyName, AsFocusBlockListNodeIndex.BlockIndex, AsAnchorBlockListNodeIndex.BlockIndex);
            }
            else
                SelectNode(State);
        }

        private void GetFirstFocusedChildState(out IFocusNodeState state, out IReadOnlyIndex firstFocusedIndex)
        {
            state = SelectionAnchor.State;
            firstFocusedIndex = null;

            IFocusNodeState FocusedState = Focus.CellView.StateView.State;

            while (state != null && !Controller.IsChildState(state, FocusedState, out firstFocusedIndex))
                state = state.ParentState;
        }
    }
}
