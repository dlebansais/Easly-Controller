namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows;
    using BaseNode;
    using EaslyController.Constants;
    using EaslyController.Controller;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;

    /// <summary>
    /// View of a IxxxController.
    /// </summary>
    public interface IFocusControllerView : IFrameControllerView
    {
        /// <summary>
        /// The controller.
        /// </summary>
        new IFocusController Controller { get; }

        /// <summary>
        /// Table of views of each state in the controller.
        /// </summary>
        new IFocusStateViewDictionary StateViewTable { get; }

        /// <summary>
        /// Table of views of each block state in the controller.
        /// </summary>
        new IFocusBlockStateViewDictionary BlockStateViewTable { get; }

        /// <summary>
        /// State view of the root state.
        /// </summary>
        new IFocusNodeStateView RootStateView { get; }

        /// <summary>
        /// Template set describing the node tree.
        /// </summary>
        new IFocusTemplateSet TemplateSet { get; }

        /// <summary>
        /// Cell view with the focus.
        /// </summary>
        IFocusFocus Focus { get; }

        /// <summary>
        /// Lowest valid value for <see cref="MoveFocus"/>.
        /// </summary>
        int MinFocusMove { get; }

        /// <summary>
        /// Highest valid value for <see cref="MoveFocus"/>.
        /// </summary>
        int MaxFocusMove { get; }

        /// <summary>
        /// Position of the caret in a text with the focus, -1 otherwise.
        /// </summary>
        int CaretPosition { get; }

        /// <summary>
        /// Position of the caret anchor in a text with the focus, -1 otherwise.
        /// </summary>
        int CaretAnchorPosition { get; }

        /// <summary>
        /// Max position of the caret in a text with the focus, -1 otherwise.
        /// </summary>
        int MaxCaretPosition { get; }

        /// <summary>
        /// Current caret mode when editing text.
        /// </summary>
        CaretModes CaretMode { get; }

        /// <summary>
        /// Current text if the focus is on a string property or comment. Null otherwise.
        /// </summary>
        string FocusedText { get; }

        /// <summary>
        /// Indicates if the node with the focus has all its frames forced to visible.
        /// </summary>
        bool IsUserVisible { get; }

        /// <summary>
        /// The current selection.
        /// </summary>
        IFocusSelection Selection { get; }

        /// <summary>
        /// The anchor to use to calculate the selection.
        /// </summary>
        IFocusNodeStateView SelectionAnchor { get; }

        /// <summary>
        /// Gets how extended is the selection.
        /// </summary>
        int SelectionExtension { get; }

        /// <summary>
        /// True if the selection is empty.
        /// </summary>
        bool IsSelectionEmpty { get; }

        /// <summary>
        /// Current auto formatting mode.
        /// </summary>
        AutoFormatModes AutoFormatMode { get; }

        /// <summary>
        /// Moves the current focus in the focus chain.
        /// </summary>
        /// <param name="direction">The change in position, relative to the current position.</param>
        /// <param name="resetAnchor">If true, resets the selected text anchor.</param>
        /// <param name="isMoved">True upon return if the focus was moved.</param>
        void MoveFocus(int direction, bool resetAnchor, out bool isMoved);

        /// <summary>
        /// Changes the caret position. Does nothing if the focus isn't on a string property.
        /// </summary>
        /// <param name="position">The new position.</param>
        /// <param name="resetAnchor">If true, resets the selected text anchor.</param>
        /// <param name="isMoved">True if the position was changed. False otherwise.</param>
        void SetCaretPosition(int position, bool resetAnchor, out bool isMoved);

        /// <summary>
        /// Changes the caret mode.
        /// </summary>
        /// <param name="mode">The new mode.</param>
        /// <param name="isChanged">True if the mode was changed.</param>
        void SetCaretMode(CaretModes mode, out bool isChanged);

        /// <summary>
        /// Sets the node with the focus to have all its frames visible.
        /// If another node had this flag set, it is reset, regardless of the value of <paramref name="isUserVisible"/>.
        /// </summary>
        void SetUserVisible(bool isUserVisible);

        /// <summary>
        /// Force the comment attached to the node with the focus to show, if empty, and move the focus to this comment.
        /// </summary>
        void ForceShowComment(out bool isMoved);

        /// <summary>
        /// Checks if a new item can be inserted at the focus.
        /// </summary>
        /// <param name="inner">Inner to use to insert the new item upon return.</param>
        /// <param name="index">Index of the new item to insert upon return.</param>
        /// <returns>True if a new item can be inserted at the focus.</returns>
        bool IsNewItemInsertable(out IFocusCollectionInner inner, out IFocusInsertionCollectionNodeIndex index);

        /// <summary>
        /// Checks if an existing item can be removed at the focus.
        /// </summary>
        /// <param name="inner">Inner to use to remove the item upon return.</param>
        /// <param name="index">Index of the item to remove upon return.</param>
        /// <returns>True if an item can be removed at the focus.</returns>
        bool IsItemRemoveable(out IFocusCollectionInner inner, out IFocusBrowsingCollectionNodeIndex index);

        /// <summary>
        /// Checks if an existing block at the focus can be split in two.
        /// </summary>
        /// <param name="inner">Inner to use to split the block upon return.</param>
        /// <param name="index">Index of the block to split upon return.</param>
        /// <returns>True if a block can be split at the focus.</returns>
        bool IsItemSplittable(out IFocusBlockListInner inner, out IFocusBrowsingExistingBlockNodeIndex index);

        /// <summary>
        /// Checks if two existing blocks at the focus can be merged.
        /// </summary>
        /// <param name="inner">Inner to use to merge the blocks upon return.</param>
        /// <param name="index">Index of the last item in the block to merge upon return.</param>
        /// <returns>True if two blocks can be merged at the focus.</returns>
        bool IsItemMergeable(out IFocusBlockListInner inner, out IFocusBrowsingExistingBlockNodeIndex index);

        /// <summary>
        /// Checks if an existing item at the focus can be moved up or down.
        /// </summary>
        /// <param name="direction">Direction of the move, relative to the current position of the item.</param>
        /// <param name="inner">Inner to use to move the item upon return.</param>
        /// <param name="index">Index of the item to move upon return.</param>
        /// <returns>True if an item can be moved at the focus.</returns>
        bool IsItemMoveable(int direction, out IFocusCollectionInner inner, out IFocusBrowsingCollectionNodeIndex index);

        /// <summary>
        /// Checks if an existing block at the focus can be moved up or down.
        /// </summary>
        /// <param name="direction">Direction of the move, relative to the current position of the item.</param>
        /// <param name="inner">Inner to use to move the block upon return.</param>
        /// <param name="blockIndex">Index of the block to move upon return.</param>
        /// <returns>True if an item can be moved at the focus.</returns>
        bool IsBlockMoveable(int direction, out IFocusBlockListInner inner, out int blockIndex);

        /// <summary>
        /// Checks if an existing item at the focus or above that can be cycled through.
        /// Such items are features and bodies.
        /// </summary>
        /// <param name="state">State that can be replaced the item upon return.</param>
        /// <param name="cyclePosition">Position of the current node in the cycle upon return.</param>
        /// <returns>True if an item can be cycled through at the focus.</returns>
        bool IsItemCyclableThrough(out IFocusCyclableNodeState state, out int cyclePosition);

        /// <summary>
        /// Checks if a node can be simplified.
        /// </summary>
        /// <param name="inner">Inner to use to replace the node upon return.</param>
        /// <param name="index">Index of the simpler node upon return.</param>
        /// <returns>True if a node can be simplified at the focus.</returns>
        bool IsItemSimplifiable(out IFocusInner inner, out IFocusInsertionChildIndex index);

        /// <summary>
        /// Checks if a node can be complexified.
        /// </summary>
        /// <param name="indexTable">List of indexes of more complex nodes upon return.</param>
        /// <returns>True if a node can be complexified at the focus.</returns>
        bool IsItemComplexifiable(out IDictionary<IFocusInner, IList<IFocusInsertionChildNodeIndex>> indexTable);

        /// <summary>
        /// Checks if an existing identifier at the focus can be split in two.
        /// </summary>
        /// <param name="inner">Inner to use to split the identifier upon return.</param>
        /// <param name="replaceIndex">Index of the identifier to replace upon return.</param>
        /// <param name="insertIndex">Index of the identifier to insert upon return.</param>
        /// <returns>True if an identifier can be split at the focus.</returns>
        bool IsIdentifierSplittable(out IFocusListInner inner, out IFocusInsertionListNodeIndex replaceIndex, out IFocusInsertionListNodeIndex insertIndex);

        /// <summary>
        /// Checks if an existing block can have its replication status changed.
        /// </summary>
        /// <param name="inner">Inner to use to change the replication status upon return.</param>
        /// <param name="blockIndex">Index of the block that can be changed upon return.</param>
        /// <param name="replication">The current replication status upon return.</param>
        /// <returns>True if an existing block can have its replication status changed at the focus.</returns>
        bool IsReplicationModifiable(out IFocusBlockListInner inner, out int blockIndex, out ReplicationStatus replication);

        /// <summary>
        /// Changes the value of a text. The caret position is also moved for this view and other views where the caret is at the same focus and position.
        /// </summary>
        /// <param name="newText">The new text.</param>
        /// <param name="newCaretPosition">The new caret position.</param>
        /// <param name="changeCaretBeforeText">True if the caret should be changed before the text, to preserve the caret invariant.</param>
        void ChangeFocusedText(string newText, int newCaretPosition, bool changeCaretBeforeText);

        /// <summary>
        /// Extends the selection by one step, starting from the focus.
        /// </summary>
        /// <param name="isChanged">True upon return is the selection was changed.</param>
        void ExtendSelection(out bool isChanged);

        /// <summary>
        /// Reduces the selection by one step, ending at the focus.
        /// </summary>
        /// <param name="isChanged">True upon return is the selection was changed.</param>
        void ReduceSelection(out bool isChanged);

        /// <summary>
        /// Clears the selection.
        /// </summary>
        void ClearSelection();

        /// <summary>
        /// Selects the specified discrete content.
        /// </summary>
        /// <param name="state">The state with a discrete content property.</param>
        /// <param name="propertyName">The property name.</param>
        void SelectDiscreteContent(IFocusNodeState state, string propertyName);

        /// <summary>
        /// Selects the specified string content.
        /// </summary>
        /// <param name="state">The state with a string content property.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="start">Index of the first character of the selection.</param>
        /// <param name="end">Index following the last character of the selection.</param>
        void SelectStringContent(IFocusNodeState state, string propertyName, int start, int end);

        /// <summary>
        /// Selects the specified string content.
        /// </summary>
        /// <param name="state">The state with the comment to select.</param>
        /// <param name="start">Index of the first character of the selection.</param>
        /// <param name="end">Index following the last character of the selection.</param>
        void SelectComment(IFocusNodeState state, int start, int end);

        /// <summary>
        /// Selects the specified node.
        /// </summary>
        /// <param name="state">The state with the node to select.</param>
        void SelectNode(IFocusNodeState state);

        /// <summary>
        /// Selects the specified list of nodes in a node list.
        /// </summary>
        /// <param name="state">The state with a node list property.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="startIndex">Index of the first node of the selection.</param>
        /// <param name="endIndex">Index following the last node of the selection.</param>
        void SelectNodeList(IFocusNodeState state, string propertyName, int startIndex, int endIndex);

        /// <summary>
        /// Selects the specified list of nodes in a block of a block list.
        /// </summary>
        /// <param name="state">The state with a node list property.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="blockIndex">Index of the block.</param>
        /// <param name="startIndex">Index of the first node of the selection.</param>
        /// <param name="endIndex">Index of the last node of the selection.</param>
        void SelectBlockNodeList(IFocusNodeState state, string propertyName, int blockIndex, int startIndex, int endIndex);

        /// <summary>
        /// Selects the specified list of blocks in a block list.
        /// </summary>
        /// <param name="state">The state with a node list property.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="startIndex">Index of the first block of the selection.</param>
        /// <param name="endIndex">Index of the last block of the selection.</param>
        void SelectBlockList(IFocusNodeState state, string propertyName, int startIndex, int endIndex);

        /// <summary>
        /// Copy the selection in the clipboard.
        /// </summary>
        /// <param name="dataObject">The clipboard data object that can already contain other custom formats.</param>
        void CopySelection(IDataObject dataObject);

        /// <summary>
        /// Copy the selection in the clipboard then removes it.
        /// </summary>
        /// <param name="dataObject">The clipboard data object that can already contain other custom formats.</param>
        /// <param name="isDeleted">True if something was deleted.</param>
        void CutSelection(IDataObject dataObject, out bool isDeleted);

        /// <summary>
        /// Replaces the selection with the content of the clipboard.
        /// </summary>
        /// <param name="isChanged">True if something was replaced or added.</param>
        void PasteSelection(out bool isChanged);

        /// <summary>
        /// Deletes the selection.
        /// </summary>
        /// <param name="isDeleted">True if something was deleted.</param>
        void DeleteSelection(out bool isDeleted);

        /// <summary>
        /// Change auto formatting mode.
        /// </summary>
        void SetAutoFormatMode(AutoFormatModes mode);
    }

    /// <summary>
    /// View of a IxxxController.
    /// </summary>
    public partial class FocusControllerView : FrameControllerView, IFocusControllerView, IFocusInternalControllerView
    {
        #region Debugging
        /// <summary>
        /// Compares two <see cref="FocusControllerView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FocusControllerView AsControllerView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsControllerView))
                return comparer.Failed();

            return true;
        }

        /// <summary></summary>
        protected virtual ulong FocusHash
        {
            get
            {
                ulong Hash = 0;

#if DEBUG
                foreach (IFocusFocus Item in FocusChain)
                    if (Item != Focus)
                        MergeHash(ref Hash, ((FocusFocus)Item).Hash);

                if (CaretPosition >= 0)
                {
                    Debug.Assert(CaretPosition <= MaxCaretPosition);

                    MergeHash(ref Hash, (ulong)CaretPosition);
                    MergeHash(ref Hash, (ulong)MaxCaretPosition);
                }
                else
                {
                    Debug.Assert(CaretPosition == -1);
                    Debug.Assert(MaxCaretPosition == -1);
                }

                MergeHash(ref Hash, (ulong)CaretAnchorPosition);
                MergeHash(ref Hash, (ulong)CaretMode);
#endif

                return Hash;
            }
        }

        /// <summary></summary>
        protected virtual void MergeHash(ref ulong hash, ulong value)
        {
            hash ^= CRC32.Get(value);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxStateViewDictionary object.
        /// </summary>
        private protected override IReadOnlyStateViewDictionary CreateStateViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusStateViewDictionary();
        }

        /// <summary>
        /// Creates a IxxxBlockStateViewDictionary object.
        /// </summary>
        private protected override IReadOnlyBlockStateViewDictionary CreateBlockStateViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusBlockStateViewDictionary();
        }

        /// <summary>
        /// Creates a IxxxAttachCallbackSet object.
        /// </summary>
        private protected override IReadOnlyAttachCallbackSet CreateCallbackSet()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusAttachCallbackSet()
            {
                NodeStateAttachedHandler = OnNodeStateCreated,
                NodeStateDetachedHandler = OnNodeStateRemoved,
                BlockListInnerAttachedHandler = OnBlockListInnerCreated,
                BlockListInnerDetachedHandler = OnBlockListInnerRemoved,
                BlockStateAttachedHandler = OnBlockStateCreated,
                BlockStateDetachedHandler = OnBlockStateRemoved,
            };
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeStateView object.
        /// </summary>
        private protected override IReadOnlyPlaceholderNodeStateView CreatePlaceholderNodeStateView(IReadOnlyPlaceholderNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusPlaceholderNodeStateView(this, (IFocusPlaceholderNodeState)state);
        }

        /// <summary>
        /// Creates a IxxxOptionalNodeStateView object.
        /// </summary>
        private protected override IReadOnlyOptionalNodeStateView CreateOptionalNodeStateView(IReadOnlyOptionalNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusOptionalNodeStateView(this, (IFocusOptionalNodeState)state);
        }

        /// <summary>
        /// Creates a IxxxPatternStateView object.
        /// </summary>
        private protected override IReadOnlyPatternStateView CreatePatternStateView(IReadOnlyPatternState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusPatternStateView(this, (IFocusPatternState)state);
        }

        /// <summary>
        /// Creates a IxxxSourceStateView object.
        /// </summary>
        private protected override IReadOnlySourceStateView CreateSourceStateView(IReadOnlySourceState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusSourceStateView(this, (IFocusSourceState)state);
        }

        /// <summary>
        /// Creates a IxxxBlockStateView object.
        /// </summary>
        private protected override IReadOnlyBlockStateView CreateBlockStateView(IReadOnlyBlockState blockState)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusBlockStateView(this, (IFocusBlockState)blockState);
        }

        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        private protected override IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView, IFrameFrame frame)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusContainerCellView((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView, (IFocusNodeStateView)childStateView, (IFocusFrame)frame);
        }

        /// <summary>
        /// Creates a IxxxBlockCellView object.
        /// </summary>
        private protected override IFrameBlockCellView CreateBlockCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameBlockStateView blockStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusBlockCellView((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView, (IFocusBlockStateView)blockStateView);
        }

        /// <summary>
        /// Creates a IxxxCellViewTreeContext object.
        /// </summary>
        private protected override IFrameCellViewTreeContext CreateCellViewTreeContext(IFrameNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusCellViewTreeContext(this, (IFocusNodeStateView)stateView, ForcedCommentStateView);
        }

        /// <summary>
        /// Creates a IxxxFocusList object.
        /// </summary>
        private protected virtual IFocusFocusList CreateFocusChain()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusFocusList();
        }

        /// <summary>
        /// Creates a IxxxInsertionNewBlockNodeIndex object.
        /// </summary>
        private protected virtual IFocusInsertionNewBlockNodeIndex CreateNewBlockNodeIndex(Node parentNode, string propertyName, Node node, int blockIndex, Pattern patternNode, Identifier sourceNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusInsertionNewBlockNodeIndex(parentNode, propertyName, node, 0, patternNode, sourceNode);
        }

        /// <summary>
        /// Creates a IxxxInsertionExistingBlockNodeIndex object.
        /// </summary>
        private protected virtual IFocusInsertionExistingBlockNodeIndex CreateExistingBlockNodeIndex(Node parentNode, string propertyName, Node node, int blockIndex, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusInsertionExistingBlockNodeIndex(parentNode, propertyName, node, blockIndex, index);
        }

        /// <summary>
        /// Creates a IxxxInsertionListNodeIndex object.
        /// </summary>
        private protected virtual IFocusInsertionListNodeIndex CreateListNodeIndex(Node parentNode, string propertyName, Node node, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusInsertionListNodeIndex(parentNode, propertyName, node, index);
        }

        /// <summary>
        /// Creates a IxxxEmptySelection object.
        /// </summary>
        private protected virtual IFocusEmptySelection CreateEmptySelection()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusEmptySelection(RootStateView);
        }

        /// <summary>
        /// Creates a IxxxDiscreteContentSelection object.
        /// </summary>
        private protected virtual IFocusDiscreteContentSelection CreateDiscreteContentSelection(IFocusNodeStateView stateView, string propertyName)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusDiscreteContentSelection(stateView, propertyName);
        }

        /// <summary>
        /// Creates a IxxxStringContentSelection object.
        /// </summary>
        private protected virtual IFocusStringContentSelection CreateStringContentSelection(IFocusNodeStateView stateView, string propertyName, int start, int end)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusStringContentSelection(stateView, propertyName, start, end);
        }

        /// <summary>
        /// Creates a IxxxCommentSelection object.
        /// </summary>
        private protected virtual IFocusCommentSelection CreateCommentSelection(IFocusNodeStateView stateView, int start, int end)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusCommentSelection(stateView, start, end);
        }

        /// <summary>
        /// Creates a IxxxNodeSelection object.
        /// </summary>
        private protected virtual IFocusNodeSelection CreateNodeSelection(IFocusNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusNodeSelection(stateView);
        }

        /// <summary>
        /// Creates a IxxxListNodeSelection object.
        /// </summary>
        private protected virtual IFocusNodeListSelection CreateNodeListSelection(IFocusNodeStateView stateView, string propertyName, int startIndex, int endIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusNodeListSelection(stateView, propertyName, startIndex, endIndex);
        }

        /// <summary>
        /// Creates a IxxxBlockListNodeSelection object.
        /// </summary>
        private protected virtual IFocusBlockNodeListSelection CreateBlockNodeListSelection(IFocusNodeStateView stateView, string propertyName, int blockIndex, int startIndex, int endIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusBlockNodeListSelection(stateView, propertyName, blockIndex, startIndex, endIndex);
        }

        /// <summary>
        /// Creates a IxxxBlockSelection object.
        /// </summary>
        private protected virtual IFocusBlockListSelection CreateBlockListSelection(IFocusNodeStateView stateView, string propertyName, int startIndex, int endIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusBlockListSelection(stateView, propertyName, startIndex, endIndex);
        }
        #endregion
    }
}
