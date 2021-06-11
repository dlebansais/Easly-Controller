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
        /// Extends the selection by one step, starting from the focus.
        /// </summary>
        /// <param name="isChanged">True upon return is the selection was changed.</param>
        public virtual void ExtendSelection(out bool isChanged)
        {
            isChanged = false;

            if (Selection is IFocusNodeSelection AsNodeSelection && AsNodeSelection.StateView == RootStateView)
                return;

            int OldSelectionExtension = SelectionExtension;
            ResetSelection();

            SelectionExtension = OldSelectionExtension + 1;
            Debug.Assert(SelectionExtension > 0);

            isChanged = true;

            for (int i = 0; i < SelectionExtension; i++)
                ExtendSelectionOneStep();

            // Debug.WriteLine($"<{SelectionExtension}> {Selection}");
        }

        /// <summary>
        /// Reduces the selection by one step, ending at the focus.
        /// </summary>
        /// <param name="isChanged">True upon return is the selection was changed.</param>
        public virtual void ReduceSelection(out bool isChanged)
        {
            isChanged = false;

            if (SelectionExtension == 0)
                return;

            int OldSelectionExtension = SelectionExtension;
            ResetSelection();

            SelectionExtension = OldSelectionExtension - 1;
            Debug.Assert(SelectionExtension >= 0);

            isChanged = true;

            for (int i = 0; i < SelectionExtension; i++)
                ExtendSelectionOneStep();

            // Debug.WriteLine($"<{SelectionExtension}> {Selection}");
        }

        private protected virtual void ExtendSelectionOneStep()
        {
            if (Selection == EmptySelection)
            {
                if (Focus is IFocusTextFocus AsTextFocus)
                    SetTextFullSelection(AsTextFocus);
                else
                    SelectNode(Focus.CellView.StateView.State);
            }
            else
                switch (Selection)
                {
                    case IFocusStringContentSelection AsStringContentSelection:
                        ExtendSelectionStringContent(AsStringContentSelection);
                        break;

                    case IFocusCommentSelection AsCommentSelection:
                        ExtendSelectionComment(AsCommentSelection);
                        break;

                    case IFocusDiscreteContentSelection AsDiscreteContentSelection:
                        SelectNode(AsDiscreteContentSelection.StateView.State);
                        break;

                    case IFocusNodeListSelection AsNodeListSelection:
                        ExtendSelectionNodeList(AsNodeListSelection);
                        break;

                    case IFocusBlockNodeListSelection AsBlockNodeListSelection:
                        ExtendSelectionBlockNodeList(AsBlockNodeListSelection);
                        break;

                    case IFocusBlockListSelection AsBlockListSelection:
                        ExtendSelectionBlockList(AsBlockListSelection);
                        break;

                    default:
                        ExtendSelectionOther();
                        break;
                }

            Debug.Assert(Selection != EmptySelection);
        }

        private protected virtual void ExtendSelectionStringContent(IFocusStringContentSelection selection)
        {
            IFocusStringContentFocus AsStringContentFocus = Focus as IFocusStringContentFocus;
            Debug.Assert(AsStringContentFocus != null);
            string Text = GetFocusedStringContent(AsStringContentFocus);

            Debug.Assert(selection.Start <= selection.End);

            int SelectedCount = selection.End - selection.Start;
            Debug.Assert(SelectedCount == Text.Length);

            SelectNode(selection.StateView.State);
        }

        private protected virtual void ExtendSelectionComment(IFocusCommentSelection selection)
        {
            IFocusCommentFocus AsCommentFocus = Focus as IFocusCommentFocus;
            Debug.Assert(AsCommentFocus != null);
            string Text = GetFocusedCommentText(AsCommentFocus);

            Debug.Assert(selection.Start <= selection.End);

            int SelectedCount = selection.End - selection.Start;
            Debug.Assert(SelectedCount == Text.Length);

            SelectNode(selection.StateView.State);
        }

        private protected virtual void ExtendSelectionNodeList(IFocusNodeListSelection selection)
        {
            string PropertyName = selection.PropertyName;
            IFocusNodeState State = selection.StateView.State;
            IFocusListInner ListInner = State.PropertyToInner(PropertyName) as IFocusListInner;

            Debug.Assert(ListInner != null);
            Debug.Assert(selection.StartIndex <= selection.EndIndex);

            int SelectedCount = selection.EndIndex - selection.StartIndex + 1;
            if (SelectedCount < ListInner.StateList.Count)
                selection.Update(0, ListInner.StateList.Count - 1);
            else
                SelectNode(selection.StateView.State);
        }

        private protected virtual void ExtendSelectionBlockNodeList(IFocusBlockNodeListSelection selection)
        {
            string PropertyName = selection.PropertyName;
            IFocusNodeState State = selection.StateView.State;
            IFocusBlockListInner BlockListInner = State.PropertyToInner(PropertyName) as IFocusBlockListInner;

            Debug.Assert(BlockListInner != null);
            Debug.Assert(selection.BlockIndex < BlockListInner.BlockStateList.Count);
            Debug.Assert(selection.StartIndex <= selection.EndIndex);

            IFocusBlockState BlockState = BlockListInner.BlockStateList[selection.BlockIndex];
            int SelectedCount = selection.EndIndex - selection.StartIndex + 1;
            if (SelectedCount < BlockState.StateList.Count)
                selection.Update(0, BlockState.StateList.Count - 1);
            else
                SelectBlockList(selection.StateView.State, selection.PropertyName, selection.BlockIndex, selection.BlockIndex);
        }

        private protected virtual void ExtendSelectionBlockList(IFocusBlockListSelection selection)
        {
            string PropertyName = selection.PropertyName;
            IFocusNodeState State = selection.StateView.State;
            IFocusBlockListInner BlockListInner = State.PropertyToInner(PropertyName) as IFocusBlockListInner;

            Debug.Assert(BlockListInner != null);
            Debug.Assert(selection.StartIndex <= selection.EndIndex);

            int SelectedCount = selection.EndIndex - selection.StartIndex + 1;
            if (SelectedCount < BlockListInner.BlockStateList.Count)
                selection.Update(0, BlockListInner.BlockStateList.Count - 1);
            else
                SelectNode(selection.StateView.State);
        }

        private protected virtual void ExtendSelectionOther()
        {
            IFocusNodeSelection AsNodeSelection = Selection as IFocusNodeSelection;
            Debug.Assert(AsNodeSelection != null);

            IFocusNodeState State = AsNodeSelection.StateView.State;
            IFocusNodeState ParentState = AsNodeSelection.StateView.State.ParentState;
            Debug.Assert(ParentState != null);

            if (State.ParentInner is IFocusListInner AsListInner && State.ParentIndex is IFocusBrowsingListNodeIndex AsListNodeIndex)
                SelectNodeList(ParentState, AsListInner.PropertyName, AsListNodeIndex.Index, AsListNodeIndex.Index);
            else if (State.ParentInner is IFocusBlockListInner AsBlockListInner && State.ParentIndex is IFocusBrowsingExistingBlockNodeIndex AsBlockNodeIndex)
                SelectBlockNodeList(ParentState, AsBlockListInner.PropertyName, AsBlockNodeIndex.BlockIndex, AsBlockNodeIndex.Index, AsBlockNodeIndex.Index);
            else
            {
                IFocusBlockState BlockState;
                if (State is IFocusPatternState AsPatternState)
                    BlockState = AsPatternState.ParentBlockState;
                else if (State is IFocusSourceState AsSourceState)
                    BlockState = AsSourceState.ParentBlockState;
                else
                    BlockState = null;

                if (BlockState != null)
                {
                    int BlockIndex = BlockState.ParentInner.BlockStateList.IndexOf(BlockState);
                    Debug.Assert(BlockIndex >= 0);

                    SelectBlockList(ParentState, BlockState.ParentInner.PropertyName, BlockIndex, BlockIndex);
                }
                else
                    SelectNode(ParentState);
            }
        }

        /// <summary>
        /// Clears the selection.
        /// </summary>
        public virtual void ClearSelection()
        {
            Selection = EmptySelection;
        }

        /// <summary>
        /// Selects the specified discrete content.
        /// </summary>
        /// <param name="state">The state with a discrete content property.</param>
        /// <param name="propertyName">The property name.</param>
        public virtual void SelectDiscreteContent(IFocusNodeState state, string propertyName)
        {
            Debug.Assert(StateViewTable.ContainsKey(state));
            Debug.Assert(state.ValuePropertyTypeTable.ContainsKey(propertyName));
            Debug.Assert(state.ValuePropertyTypeTable[propertyName] == ValuePropertyType.Boolean || state.ValuePropertyTypeTable[propertyName] == ValuePropertyType.Enum);

            IFocusNodeStateView stateView = StateViewTable[state];
            Selection = CreateDiscreteContentSelection(stateView, propertyName);
        }

        /// <summary>
        /// Selects the specified string content.
        /// </summary>
        /// <param name="state">The state with a string content property.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="start">Index of the first character of the selection.</param>
        /// <param name="end">Index following the last character of the selection.</param>
        public virtual void SelectStringContent(IFocusNodeState state, string propertyName, int start, int end)
        {
            Debug.Assert(StateViewTable.ContainsKey(state));
            Debug.Assert(state.ValuePropertyTypeTable.ContainsKey(propertyName));
            Debug.Assert(state.ValuePropertyTypeTable[propertyName] == ValuePropertyType.String);

            IFocusNodeStateView StateView = StateViewTable[state];

            int OldFocusIndex = FocusChain.IndexOf(Focus);
            int NewFocusIndex = -1;
            for (int i = 0; i < FocusChain.Count; i++)
            {
                IFocusFocus Item = FocusChain[i];
                if (Item.CellView is IFocusStringContentFocusableCellView AsStringContentFocusableCellView)
                {
                    if (AsStringContentFocusableCellView.StateView == StateView && AsStringContentFocusableCellView.PropertyName == propertyName)
                    {
                        NewFocusIndex = i;
                        break;
                    }
                }
            }

            Debug.Assert(NewFocusIndex >= 0);

            if (OldFocusIndex != NewFocusIndex)
                ChangeFocus(NewFocusIndex - OldFocusIndex, OldFocusIndex, NewFocusIndex, true, out bool IsRefreshed);

            SelectStringContent(StateView, propertyName, start, end);

            CaretAnchorPosition = start;
            CaretPosition = end;

            CheckCaretInvariant();
        }

        /// <summary></summary>
        protected virtual void SelectStringContent(IFocusNodeStateView stateView, string propertyName, int start, int end)
        {
            Selection = CreateStringContentSelection(stateView, propertyName, start, end);
        }

        /// <summary>
        /// Selects the specified string content.
        /// </summary>
        /// <param name="state">The state with the comment to select.</param>
        /// <param name="start">Index of the first character of the selection.</param>
        /// <param name="end">Index following the last character of the selection.</param>
        public virtual void SelectComment(IFocusNodeState state, int start, int end)
        {
            Debug.Assert(StateViewTable.ContainsKey(state));

            IFocusNodeStateView StateView = StateViewTable[state];
            SelectComment(StateView, start, end);

            CaretAnchorPosition = start;
            CaretPosition = end;

            CheckCaretInvariant();
        }

        /// <summary></summary>
        protected virtual void SelectComment(IFocusNodeStateView stateView, int start, int end)
        {
            Selection = CreateCommentSelection(stateView, start, end);
        }

        /// <summary>
        /// Selects the specified node.
        /// </summary>
        /// <param name="state">The state with the node to select.</param>
        public virtual void SelectNode(IFocusNodeState state)
        {
            Debug.Assert(StateViewTable.ContainsKey(state));

            IFocusNodeStateView stateView = StateViewTable[state];
            Selection = CreateNodeSelection(stateView);
        }

        /// <summary>
        /// Selects the specified list of nodes in a node list.
        /// </summary>
        /// <param name="state">The state with a node list property.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="startIndex">Index of the first node of the selection.</param>
        /// <param name="endIndex">Index following the last node of the selection.</param>
        public virtual void SelectNodeList(IFocusNodeState state, string propertyName, int startIndex, int endIndex)
        {
            Debug.Assert(StateViewTable.ContainsKey(state));
            Debug.Assert(state.InnerTable.ContainsKey(propertyName));

            IFocusListInner ParentInner = state.InnerTable[propertyName] as IFocusListInner;
            Debug.Assert(ParentInner != null);
            Debug.Assert(startIndex >= 0 && startIndex < ParentInner.StateList.Count);
            Debug.Assert(endIndex >= 0 && endIndex <= ParentInner.StateList.Count);

            IFocusNodeStateView stateView = StateViewTable[state];
            Selection = CreateNodeListSelection(stateView, propertyName, startIndex, endIndex);
        }

        /// <summary>
        /// Selects the specified list of nodes in a block of a block list.
        /// </summary>
        /// <param name="state">The state with a node list property.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="blockIndex">Index of the block.</param>
        /// <param name="startIndex">Index of the first node of the selection.</param>
        /// <param name="endIndex">Index of the last node of the selection.</param>
        public virtual void SelectBlockNodeList(IFocusNodeState state, string propertyName, int blockIndex, int startIndex, int endIndex)
        {
            Debug.Assert(StateViewTable.ContainsKey(state));
            Debug.Assert(state.InnerTable.ContainsKey(propertyName));

            IFocusBlockListInner ParentInner = state.InnerTable[propertyName] as IFocusBlockListInner;
            Debug.Assert(ParentInner != null);
            Debug.Assert(blockIndex >= 0 && blockIndex < ParentInner.BlockStateList.Count);

            IFocusBlockState BlockState = ParentInner.BlockStateList[blockIndex];
            Debug.Assert(startIndex >= 0 && startIndex < BlockState.StateList.Count);
            Debug.Assert(endIndex >= 0 && endIndex <= BlockState.StateList.Count);

            IFocusNodeStateView stateView = StateViewTable[state];
            Selection = CreateBlockNodeListSelection(stateView, propertyName, blockIndex, startIndex, endIndex);
        }

        /// <summary>
        /// Selects the specified list of blocks in a block list.
        /// </summary>
        /// <param name="state">The state with a node list property.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="startIndex">Index of the first block of the selection.</param>
        /// <param name="endIndex">Index of the last block of the selection.</param>
        public virtual void SelectBlockList(IFocusNodeState state, string propertyName, int startIndex, int endIndex)
        {
            Debug.Assert(StateViewTable.ContainsKey(state));
            Debug.Assert(state.InnerTable.ContainsKey(propertyName));

            IFocusBlockListInner ParentInner = state.InnerTable[propertyName] as IFocusBlockListInner;
            Debug.Assert(ParentInner != null);
            Debug.Assert(startIndex >= 0 && startIndex < ParentInner.BlockStateList.Count);
            Debug.Assert(endIndex >= 0 && endIndex <= ParentInner.BlockStateList.Count);

            IFocusNodeStateView stateView = StateViewTable[state];
            Selection = CreateBlockListSelection(stateView, propertyName, startIndex, endIndex);
        }

        /// <summary>
        /// Copy the selection in the clipboard.
        /// </summary>
        /// <param name="dataObject">The clipboard data object that can already contain other custom formats.</param>
        public virtual void CopySelection(IDataObject dataObject)
        {
            Selection.Copy(dataObject);
        }

        /// <summary>
        /// Copy the selection in the clipboard then removes it.
        /// </summary>
        /// <param name="dataObject">The clipboard data object that can already contain other custom formats.</param>
        /// <param name="isDeleted">True if something was deleted.</param>
        public virtual void CutSelection(IDataObject dataObject, out bool isDeleted)
        {
            Selection.Cut(dataObject, out isDeleted);

            if (isDeleted)
                ResetSelection();
        }

        /// <summary>
        /// Replaces the selection with the content of the clipboard.
        /// </summary>
        /// <param name="isChanged">True if something was replaced or added.</param>
        public virtual void PasteSelection(out bool isChanged)
        {
            Selection.Paste(out isChanged);
        }

        /// <summary>
        /// Deletes the selection.
        /// </summary>
        /// <param name="isDeleted">True if something was deleted.</param>
        public virtual void DeleteSelection(out bool isDeleted)
        {
            Selection.Delete(out isDeleted);

            if (isDeleted)
                ResetSelection();
        }
    }
}
