namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using EaslyController.Controller;
    using EaslyController.Frame;
    using EaslyController.Writeable;

    /// <summary>
    /// View of a IxxxController.
    /// </summary>
    public partial class FocusControllerView : FrameControllerView, IFocusControllerView, IFocusInternalControllerView
    {
        /// <summary>
        /// Handler called every time a block state is inserted in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlockStateInserted(IWriteableInsertBlockOperation operation)
        {
            base.OnBlockStateInserted(operation);

            IFocusBlockState BlockState = ((IFocusInsertBlockOperation)operation).BlockState;

            Debug.Assert(BlockState != null);
            Debug.Assert(BlockStateViewTable.ContainsKey(BlockState));

            Debug.Assert(StateViewTable.ContainsKey(BlockState.PatternState));
            Debug.Assert(StateViewTable.ContainsKey(BlockState.SourceState));

            Debug.Assert(BlockState.StateList.Count == 1);

            IFocusPlaceholderNodeState ChildState = ((IFocusInsertBlockOperation)operation).ChildState;
            Debug.Assert(ChildState == BlockState.StateList[0]);
            Debug.Assert(ChildState.ParentIndex == ((IFocusInsertBlockOperation)operation).BrowsingIndex);
            Debug.Assert(StateViewTable.ContainsKey(ChildState));
        }

        /// <summary>
        /// Handler called every time a block state is removed from the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlockStateRemoved(IWriteableRemoveBlockOperation operation)
        {
            base.OnBlockStateRemoved(operation);

            IFocusBlockState BlockState = ((IFocusRemoveBlockOperation)operation).BlockState;

            Debug.Assert(BlockState != null);
            Debug.Assert(!BlockStateViewTable.ContainsKey(BlockState));

            Debug.Assert(!StateViewTable.ContainsKey(BlockState.PatternState));
            Debug.Assert(!StateViewTable.ContainsKey(BlockState.SourceState));

            IFocusNodeState RemovedState = ((IFocusRemoveBlockOperation)operation).RemovedState;
            Debug.Assert(!StateViewTable.ContainsKey(RemovedState));

            Debug.Assert(BlockState.StateList.Count == 0);
        }

        /// <summary>
        /// Handler called every time a block view must be removed from the controller view.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlockViewRemoved(IWriteableRemoveBlockViewOperation operation)
        {
            base.OnBlockViewRemoved(operation);

            IFocusBlockState BlockState = ((IFocusRemoveBlockViewOperation)operation).BlockState;

            Debug.Assert(BlockState != null);
            Debug.Assert(!BlockStateViewTable.ContainsKey(BlockState));

            Debug.Assert(!StateViewTable.ContainsKey(BlockState.PatternState));
            Debug.Assert(!StateViewTable.ContainsKey(BlockState.SourceState));

            foreach (IFocusNodeState State in BlockState.StateList)
                Debug.Assert(!StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a state is inserted in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnStateInserted(IWriteableInsertNodeOperation operation)
        {
            base.OnStateInserted(operation);

            IFocusNodeState ChildState = ((IFocusInsertNodeOperation)operation).ChildState;
            Debug.Assert(ChildState != null);
            Debug.Assert(StateViewTable.ContainsKey(ChildState));

            IFocusBrowsingCollectionNodeIndex BrowsingIndex = ((IFocusInsertNodeOperation)operation).BrowsingIndex;
            Debug.Assert(ChildState.ParentIndex == BrowsingIndex);

            MoveFocusToState(ChildState);
        }

        /// <summary>
        /// Handler called every time a state is removed from the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnStateRemoved(IWriteableRemoveNodeOperation operation)
        {
            base.OnStateRemoved(operation);

            IFocusPlaceholderNodeState RemovedState = ((IFocusRemoveNodeOperation)operation).RemovedState;
            Debug.Assert(RemovedState != null);
            Debug.Assert(!StateViewTable.ContainsKey(RemovedState));
        }

        /// <summary>
        /// Handler called every time a state is inserted in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnStateReplaced(IWriteableReplaceOperation operation)
        {
            base.OnStateReplaced(operation);

            IFocusNodeState NewChildState = ((IFocusReplaceOperation)operation).NewChildState;
            Debug.Assert(NewChildState != null);
            Debug.Assert(StateViewTable.ContainsKey(NewChildState));

            IFocusBrowsingChildIndex OldBrowsingIndex = ((IFocusReplaceOperation)operation).OldBrowsingIndex;
            Debug.Assert(OldBrowsingIndex != null);
            Debug.Assert(NewChildState.ParentIndex != OldBrowsingIndex);

            IFocusBrowsingChildIndex NewBrowsingIndex = ((IFocusReplaceOperation)operation).NewBrowsingIndex;
            Debug.Assert(NewBrowsingIndex != null);
            Debug.Assert(NewChildState.ParentIndex == NewBrowsingIndex);
        }

        /// <summary>
        /// Handler called every time a state is assigned in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnStateAssigned(IWriteableAssignmentOperation operation)
        {
            base.OnStateAssigned(operation);

            IFocusOptionalNodeState State = ((IFocusAssignmentOperation)operation).State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a state is unassigned in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnStateUnassigned(IWriteableAssignmentOperation operation)
        {
            base.OnStateUnassigned(operation);

            IFocusOptionalNodeState State = ((IFocusAssignmentOperation)operation).State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a discrete value is changed in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnDiscreteValueChanged(IWriteableChangeDiscreteValueOperation operation)
        {
            base.OnDiscreteValueChanged(operation);

            IFocusNodeState State = ((IFocusChangeDiscreteValueOperation)operation).State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a text is changed in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnTextChanged(IWriteableChangeTextOperation operation)
        {
            bool IsSameFocus = IsSameChangeTextOperationFocus((IFocusChangeTextOperation)operation);
            int NewCaretPosition = ((IFocusChangeCaretOperation)operation).NewCaretPosition;
            bool ChangeCaretBeforeText = ((IFocusChangeCaretOperation)operation).ChangeCaretBeforeText;

            if (IsSameFocus && NewCaretPosition >= 0 && ChangeCaretBeforeText)
                CaretPosition = NewCaretPosition;

            base.OnTextChanged(operation);

            if (IsSameFocus && NewCaretPosition >= 0 && !ChangeCaretBeforeText)
                CaretPosition = NewCaretPosition;

            IFocusNodeState State = ((IFocusChangeTextOperation)operation).State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));

            ResetSelection();
            CaretAnchorPosition = CaretPosition;
            CheckCaretInvariant();
        }

        private protected virtual bool IsSameChangeTextOperationFocus(IFocusChangeTextOperation operation)
        {
            Node Node = null;
            if (operation.State.ParentIndex is IFocusNodeIndex AsNodeIndex)
                Node = AsNodeIndex.Node;
            else if (operation.State is IFocusOptionalNodeState AsOptionalNodeState && AsOptionalNodeState.ParentInner.IsAssigned)
                Node = AsOptionalNodeState.Node;

            Debug.Assert(Node != null);
            string PropertyName = operation.PropertyName;

            Node FocusedNode = null;
            IFocusFrame FocusedFrame = null;
            Focus.GetLocationInSourceCode(out FocusedNode, out FocusedFrame);

            return FocusedNode == Node && FocusedFrame is IFocusTextValueFrame AsTextValueFrame && AsTextValueFrame.PropertyName == PropertyName;
        }

        /// <summary>
        /// Handler called every time a comment is changed in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnCommentChanged(IWriteableChangeCommentOperation operation)
        {
            bool IsSameFocus = IsSameChangeCommentOperationFocus((IFocusChangeCommentOperation)operation);
            int NewCaretPosition = ((IFocusChangeCaretOperation)operation).NewCaretPosition;
            bool ChangeCaretBeforeText = ((IFocusChangeCaretOperation)operation).ChangeCaretBeforeText;

            if (IsSameFocus && NewCaretPosition >= 0 && ChangeCaretBeforeText)
                CaretPosition = NewCaretPosition;

            base.OnCommentChanged(operation);

            if (IsSameFocus && NewCaretPosition >= 0 && !ChangeCaretBeforeText)
                CaretPosition = NewCaretPosition;

            IFocusNodeState State = ((IFocusChangeCommentOperation)operation).State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));

            ResetSelection();
            CheckCaretInvariant();
        }

        private protected virtual bool IsSameChangeCommentOperationFocus(IFocusChangeCommentOperation operation)
        {
            Node Node = null;
            if (operation.State.ParentIndex is IFocusNodeIndex AsNodeIndex)
                Node = AsNodeIndex.Node;
            else if (operation.State is IFocusOptionalNodeState AsOptionalNodeState && AsOptionalNodeState.ParentInner.IsAssigned)
                Node = AsOptionalNodeState.Node;

            Debug.Assert(Node != null);

            Node FocusedNode = null;
            IFocusFrame FocusedFrame = null;
            Focus.GetLocationInSourceCode(out FocusedNode, out FocusedFrame);

            return FocusedNode == Node && FocusedFrame is IFocusCommentFrame;
        }

        /// <summary>
        /// Handler called every time a block state is changed in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlockStateChanged(IWriteableChangeBlockOperation operation)
        {
            base.OnBlockStateChanged(operation);

            IFocusBlockState BlockState = ((IFocusChangeBlockOperation)operation).BlockState;
            Debug.Assert(BlockState != null);
            Debug.Assert(BlockStateViewTable.ContainsKey(BlockState));
        }

        /// <summary>
        /// Handler called every time a state is moved in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnStateMoved(IWriteableMoveNodeOperation operation)
        {
            base.OnStateMoved(operation);

            IFocusPlaceholderNodeState State = ((IFocusMoveNodeOperation)operation).State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a block state is moved in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlockStateMoved(IWriteableMoveBlockOperation operation)
        {
            base.OnBlockStateMoved(operation);

            IFocusBlockState BlockState = ((IFocusMoveBlockOperation)operation).BlockState;
            Debug.Assert(BlockState != null);
            Debug.Assert(BlockStateViewTable.ContainsKey(BlockState));
        }

        /// <summary>
        /// Handler called every time a block split in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlockSplit(IWriteableSplitBlockOperation operation)
        {
            base.OnBlockSplit(operation);

            IFocusBlockState BlockState = ((IFocusSplitBlockOperation)operation).BlockState;
            Debug.Assert(BlockState != null);
            Debug.Assert(BlockStateViewTable.ContainsKey(BlockState));
        }

        /// <summary>
        /// Handler called every time two blocks are merged.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlocksMerged(IWriteableMergeBlocksOperation operation)
        {
            base.OnBlocksMerged(operation);

            IFocusBlockState BlockState = ((IFocusMergeBlocksOperation)operation).BlockState;
            Debug.Assert(BlockState != null);
            Debug.Assert(!BlockStateViewTable.ContainsKey(BlockState));
        }

        /// <summary>
        /// Handler called to refresh views.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnGenericRefresh(IWriteableGenericRefreshOperation operation)
        {
            base.OnGenericRefresh(operation);

            IFocusNodeState RefreshState = ((IFocusGenericRefreshOperation)operation).RefreshState;
            Debug.Assert(RefreshState != null);
            Debug.Assert(StateViewTable.ContainsKey(RefreshState));
        }

        private protected override IFrameCellViewTreeContext InitializedCellViewTreeContext(IFrameNodeStateView stateView)
        {
            IFocusCellViewTreeContext Context = (IFocusCellViewTreeContext)CreateCellViewTreeContext(stateView);
            ForcedCommentStateView = null;

            IList<IFocusFrameSelectorList> SelectorStack = ((IFocusNodeStateView)stateView).GetSelectorStack();
            foreach (IFocusFrameSelectorList Selectors in SelectorStack)
                Context.AddSelectors(Selectors);

            return Context;
        }

        private protected override void CloseCellViewTreeContext(IFrameCellViewTreeContext context)
        {
            Debug.Assert(((IFocusCellViewTreeContext)context).ForcedCommentStateView == null);
        }

        private protected override void Refresh(IFrameNodeState state)
        {
            Node FocusedNode = null;
            IFocusFrame FocusedFrame = null;

            if (Focus != null)
                Focus.GetLocationInSourceCode(out FocusedNode, out FocusedFrame);

            base.Refresh(state);

            UpdateFocusChain((IFocusNodeState)state, FocusedNode, FocusedFrame);
        }

        private protected virtual void UpdateFocusChain(IFocusNodeState state, Node focusedNode, IFocusFrame focusedFrame)
        {
            IFocusFocusList NewFocusChain = CreateFocusChain();
            IFocusNodeState RootState = Controller.RootState;
            IFocusNodeStateView RootStateView = StateViewTable[RootState];

            IFocusFocus MatchingFocus = null;
            RootStateView.UpdateFocusChain(NewFocusChain, focusedNode, focusedFrame, ref MatchingFocus);

            // Ensured by all templates having at least one preferred (hence focusable) frame.
            Debug.Assert(NewFocusChain.Count > 0);

            // First run, initialize the focus to the first focusable cell.
            if (Focus == null)
            {
                Debug.Assert(FocusChain == null);
                Focus = NewFocusChain[0];
                ResetCaretPosition(0, true);
            }
            else if (MatchingFocus != null)
            {
                Focus = MatchingFocus;
                UpdateMaxCaretPosition(); // The focus didn't change, but the content may have.
            }
            else
                RecoverFocus(state, NewFocusChain); // The focus has forcibly changed.

            FocusChain = NewFocusChain;
            DebugObjects.AddReference(NewFocusChain);

            Debug.Assert(Focus != null);
            Debug.Assert(FocusChain.Contains(Focus));

            SelectionAnchor = Focus.CellView.StateView;
            SelectionExtension = 0;
        }

        private protected virtual void RecoverFocus(IFocusNodeState state, IFocusFocusList newFocusChain)
        {
            IFocusNodeState CurrentState = state;
            List<IFocusNodeStateView> StateViewList = new List<IFocusNodeStateView>();
            IFocusNodeStateView MainStateView = null;
            List<IFocusFocus> SameStateFocusableList = new List<IFocusFocus>();

            // Get the state that should have the focus and all its children.
            while (CurrentState != null && !GetFocusedStateAndChildren(newFocusChain, CurrentState, out MainStateView, out StateViewList, out SameStateFocusableList))
                CurrentState = CurrentState.ParentState;

            Debug.Assert(SameStateFocusableList.Count > 0);

            // Now that we have found candidates, try to select the original frame.
            bool IsFrameChanged = true;

            IFocusFrame Frame = Focus.CellView.Frame;
            foreach (IFocusFocus CellFocus in SameStateFocusableList)
                if (CellFocus.CellView.Frame == Frame)
                {
                    IsFrameChanged = false;
                    Focus = CellFocus;
                    break;
                }

            // If the frame has changed, use a preferred frame.
            if (IsFrameChanged)
                FindPreferredFrame(MainStateView, SameStateFocusableList);

            ResetCaretPosition(0, true);
        }

        private protected virtual void FindPreferredFrame(IFocusNodeStateView mainStateView, List<IFocusFocus> sameStateFocusableList)
        {
            bool IsFrameSet = false;
            IFocusNodeTemplate Template = mainStateView.Template as IFocusNodeTemplate;
            Debug.Assert(Template != null);

            Template.GetPreferredFrame(out IFocusNodeFrame FirstPreferredFrame, out IFocusNodeFrame LastPreferredFrame);
            Debug.Assert(FirstPreferredFrame != null);
            Debug.Assert(LastPreferredFrame != null);

            foreach (IFocusFocus CellFocus in sameStateFocusableList)
            {
                IFocusFocusableCellView CellView = CellFocus.CellView;
                if (CellView.Frame == FirstPreferredFrame || CellView.Frame == LastPreferredFrame)
                {
                    IsFrameSet = true;
                    Focus = CellFocus;
                    break;
                }
            }

            // If none of the preferred frames are visible, use the first focusable cell.
            if (!IsFrameSet)
                Focus = sameStateFocusableList[0];
        }

        private protected virtual bool GetFocusedStateAndChildren(IFocusFocusList newFocusChain, IFocusNodeState state, out IFocusNodeStateView mainStateView, out List<IFocusNodeStateView> stateViewList, out List<IFocusFocus> sameStateFocusableList)
        {
            mainStateView = StateViewTable[state];

            stateViewList = new List<IFocusNodeStateView>();
            sameStateFocusableList = new List<IFocusFocus>();
            GetChildrenStateView(mainStateView, stateViewList);

            // Find all focusable cells belonging to these states.
            foreach (IFocusFocus NewFocus in newFocusChain)
            {
                IFocusFocusableCellView CellView = NewFocus.CellView;

                foreach (IFocusNodeStateView StateView in stateViewList)
                    if (CellView.StateView == StateView && !sameStateFocusableList.Contains(NewFocus))
                    {
                        sameStateFocusableList.Add(NewFocus);
                        break;
                    }
            }

            if (sameStateFocusableList.Count > 0)
                return true;

            // If it doesn't work, try the parent state, down to the root (in case of a removal or unassign).
            return false;
        }

        private protected virtual void GetChildrenStateView(IFocusNodeStateView stateView, List<IFocusNodeStateView> stateViewList)
        {
            stateViewList.Add(stateView);

            foreach (KeyValuePair<string, IFocusInner> Entry in stateView.State.InnerTable)
            {
                bool IsHandled = false;
                IFocusTemplate Template;
                IFocusAssignableCellViewReadOnlyDictionary<string> CellViewTable;
                IFocusCellViewCollection EmbeddingCellView;

                if (Entry.Value is IFocusPlaceholderInner<IFocusBrowsingPlaceholderNodeIndex> AsPlaceholderInner)
                {
                    Debug.Assert(StateViewTable.ContainsKey(AsPlaceholderInner.ChildState));
                    IFocusNodeStateView ChildStateView = StateViewTable[AsPlaceholderInner.ChildState];
                    Debug.Assert(ChildStateView != null);

                    GetChildrenStateView(ChildStateView, stateViewList);

                    Template = ChildStateView.Template;
                    CellViewTable = ChildStateView.CellViewTable;

                    IsHandled = true;
                }
                else if (Entry.Value is IFocusOptionalInner<IFocusBrowsingOptionalNodeIndex> AsOptionalInner)
                {
                    if (AsOptionalInner.IsAssigned)
                    {
                        Debug.Assert(StateViewTable.ContainsKey(AsOptionalInner.ChildState));
                        IFocusOptionalNodeStateView ChildStateView = StateViewTable[AsOptionalInner.ChildState] as IFocusOptionalNodeStateView;
                        Debug.Assert(ChildStateView != null);

                        GetChildrenStateView(ChildStateView, stateViewList);

                        Template = ChildStateView.Template;
                        CellViewTable = ChildStateView.CellViewTable;
                    }

                    IsHandled = true;
                }
                else if (Entry.Value is IFocusListInner<IFocusBrowsingListNodeIndex> AsListInner)
                {
                    foreach (IFocusNodeState ChildState in AsListInner.StateList)
                    {
                        IFocusNodeStateView ChildStateView = StateViewTable[ChildState];
                        GetChildrenStateView(ChildStateView, stateViewList);

                        Template = ChildStateView.Template;
                        CellViewTable = ChildStateView.CellViewTable;
                    }

                    IsHandled = true;
                }
                else if (Entry.Value is IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> AsBlockListInner)
                {
                    foreach (IFocusBlockState BlockState in AsBlockListInner.BlockStateList)
                    {
                        IFocusNodeStateView PatternStateView = StateViewTable[BlockState.PatternState];
                        GetChildrenStateView(PatternStateView, stateViewList);
                        Template = PatternStateView.Template;
                        CellViewTable = PatternStateView.CellViewTable;

                        IFocusNodeStateView SourceStateView = StateViewTable[BlockState.SourceState];
                        GetChildrenStateView(SourceStateView, stateViewList);
                        Template = SourceStateView.Template;
                        CellViewTable = SourceStateView.CellViewTable;

                        IFocusBlockStateView BlockStateView = BlockStateViewTable[BlockState];
                        Template = BlockStateView.Template;
                        EmbeddingCellView = BlockStateView.EmbeddingCellView;

                        foreach (IFocusNodeState ChildState in BlockState.StateList)
                        {
                            IFocusNodeStateView ChildStateView = StateViewTable[ChildState];
                            GetChildrenStateView(ChildStateView, stateViewList);

                            Template = ChildStateView.Template;
                            CellViewTable = ChildStateView.CellViewTable;
                        }
                    }

                    IsHandled = true;
                }

                Debug.Assert(IsHandled);
            }
        }

        private protected virtual void ResetCaretPosition(int direction, bool resetCaretAnchor)
        {
            if (Focus is IFocusTextFocus AsTextFocus)
            {
                string Text = GetFocusedText(AsTextFocus);

                MaxCaretPosition = Text.Length;

                if (direction >= 0)
                    CaretPosition = 0;
                else
                    CaretPosition = MaxCaretPosition;

                if (resetCaretAnchor)
                    CaretAnchorPosition = CaretPosition;

                CheckCaretInvariant(Text);
            }
            else
            {
                CaretPosition = -1;
                if (resetCaretAnchor)
                    CaretAnchorPosition = -1;
                MaxCaretPosition = -1;
            }
        }

        private protected virtual void UpdateMaxCaretPosition()
        {
            if (Focus is IFocusTextFocus AsTextFocus)
            {
                string Text = GetFocusedText(AsTextFocus);

                MaxCaretPosition = Text.Length;
                CheckCaretInvariant(Text);
            }
        }

        private protected virtual void CheckCaretInvariant()
        {
            if (Focus is IFocusTextFocus AsTextFocus)
            {
                string Text = GetFocusedText(AsTextFocus);
                CheckCaretInvariant(Text);
            }
            else
            {
                Debug.Assert(CaretPosition == -1);
                Debug.Assert(CaretAnchorPosition == -1);
                Debug.Assert(MaxCaretPosition == -1);
            }
        }

        private protected virtual void CheckCaretInvariant(string text)
        {
            Debug.Assert(text != null);
            Debug.Assert(CaretPosition >= 0);
            Debug.Assert(MaxCaretPosition == text.Length);
            Debug.Assert(CaretPosition <= MaxCaretPosition);
        }

        private protected virtual void ResetSelection()
        {
            if (Focus is IFocusDiscreteContentFocus AsDiscreteContentFocus)
            {
                IFocusDiscreteContentFocusableCellView CellView = AsDiscreteContentFocus.CellView;
                SelectDiscreteContent(CellView.StateView.State, CellView.PropertyName);
            }
            else
                ClearSelection();

            SelectionAnchor = Focus.CellView.StateView;
            SelectionExtension = 0;
        }

        private protected virtual void SetTextSelection(IFocusTextFocus textFocus)
        {
            bool IsHandled = false;

            int Start = CaretAnchorPosition >= 0 ? CaretAnchorPosition : 0;
            int End = CaretPosition;

            if (textFocus is IFocusStringContentFocus AsStringContentFocus)
            {
                IFocusStringContentFocusableCellView CellView = AsStringContentFocus.CellView;
                SelectStringContent(CellView.StateView, CellView.PropertyName, Start, End);
                IsHandled = true;
            }
            else if (textFocus is IFocusCommentFocus AsCommentFocus)
            {
                IFocusCommentCellView CellView = AsCommentFocus.CellView;
                SelectComment(CellView.StateView, Start, End);
                IsHandled = true;
            }

            Debug.Assert(IsHandled);
        }

        private protected virtual void SetTextFullSelection(IFocusTextFocus textFocus)
        {
            bool IsHandled = false;

            if (textFocus is IFocusStringContentFocus AsStringContentFocus)
            {
                string Text = GetFocusedStringContent(AsStringContentFocus);
                IFocusStringContentFocusableCellView CellView = AsStringContentFocus.CellView;
                SelectStringContent(CellView.StateView, CellView.PropertyName, 0, Text.Length);
                IsHandled = true;
            }
            else if (textFocus is IFocusCommentFocus AsCommentFocus)
            {
                string Text = GetFocusedCommentText(AsCommentFocus);
                IFocusCommentCellView CellView = AsCommentFocus.CellView;
                SelectComment(CellView.StateView, 0, Text.Length);
                IsHandled = true;
            }

            Debug.Assert(IsHandled);
        }

        private protected override IFrameFrame GetAssociatedFrame(IFrameInner<IFrameBrowsingChildIndex> inner)
        {
            IFocusNodeState Owner = ((IFocusInner<IFocusBrowsingChildIndex>)inner).Owner;
            IFocusNodeStateView StateView = StateViewTable[Owner];
            IList<IFocusFrameSelectorList> SelectorStack = StateView.GetSelectorStack();

            IFocusFrame AssociatedFrame = TemplateSet.InnerToFrame((IFocusInner<IFocusBrowsingChildIndex>)inner, SelectorStack) as IFocusFrame;

            return AssociatedFrame;
        }

        private protected override void ValidateBlockCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameBlockCellView blockCellView)
        {
            Debug.Assert(((IFocusBlockCellView)blockCellView).StateView == (IFocusNodeStateView)stateView);
            Debug.Assert(((IFocusBlockCellView)blockCellView).ParentCellView == (IFocusCellViewCollection)parentCellView);
        }

        private protected override void ValidateContainerCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView, IFrameContainerCellView containerCellView)
        {
            Debug.Assert(((IFocusContainerCellView)containerCellView).StateView == (IFocusNodeStateView)stateView);
            Debug.Assert(((IFocusContainerCellView)containerCellView).ParentCellView == (IFocusCellViewCollection)parentCellView);
            Debug.Assert(((IFocusContainerCellView)containerCellView).ChildStateView == (IFocusNodeStateView)childStateView);
        }

        private protected virtual string GetFocusedText(IFocusTextFocus textCellFocus)
        {
            string Text = null;

            if (Focus is IFocusStringContentFocus AsTextFocus)
                Text = GetFocusedStringContent(AsTextFocus);
            else if (Focus is IFocusCommentFocus AsCommentFocus)
            {
                Text = GetFocusedCommentText(AsCommentFocus);
                if (Text == null)
                    Text = string.Empty;
            }

            Debug.Assert(Text != null);
            return Text;
        }

        private protected virtual string GetFocusedStringContent(IFocusStringContentFocus textCellFocus)
        {
            IFocusStringContentFocusableCellView CellView = textCellFocus.CellView;
            Node Node = CellView.StateView.State.Node;
            string PropertyName = CellView.PropertyName;

            return NodeTreeHelper.GetString(Node, PropertyName);
        }

        private protected virtual string GetFocusedCommentText(IFocusCommentFocus commentFocus)
        {
            IFocusCommentCellView CellView = commentFocus.CellView;
            Document Documentation = CellView.StateView.State.Node.Documentation;

            return CommentHelper.Get(Documentation);
        }

        private protected IFocusNodeStateView ForcedCommentStateView { get; private set; }
    }
}
