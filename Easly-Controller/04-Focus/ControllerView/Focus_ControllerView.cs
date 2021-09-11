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
    public partial class FocusControllerView : FrameControllerView, IFocusInternalControllerView
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
        private protected override ReadOnlyNodeStateViewDictionary CreateStateViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusNodeStateViewDictionary();
        }

        /// <summary>
        /// Creates a IxxxBlockStateViewDictionary object.
        /// </summary>
        private protected override ReadOnlyBlockStateViewDictionary CreateBlockStateViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusBlockStateViewDictionary();
        }

        /// <summary>
        /// Creates a IxxxAttachCallbackSet object.
        /// </summary>
        private protected override ReadOnlyAttachCallbackSet CreateCallbackSet()
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
        private protected override ReadOnlyPlaceholderNodeStateView CreatePlaceholderNodeStateView(IReadOnlyPlaceholderNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusPlaceholderNodeStateView(this, (IFocusPlaceholderNodeState)state);
        }

        /// <summary>
        /// Creates a IxxxOptionalNodeStateView object.
        /// </summary>
        private protected override ReadOnlyOptionalNodeStateView CreateOptionalNodeStateView(IReadOnlyOptionalNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusOptionalNodeStateView(this, (IFocusOptionalNodeState)state);
        }

        /// <summary>
        /// Creates a IxxxPatternStateView object.
        /// </summary>
        private protected override ReadOnlyPatternStateView CreatePatternStateView(IReadOnlyPatternState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusPatternStateView(this, (IFocusPatternState)state);
        }

        /// <summary>
        /// Creates a IxxxSourceStateView object.
        /// </summary>
        private protected override ReadOnlySourceStateView CreateSourceStateView(IReadOnlySourceState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusSourceStateView(this, (IFocusSourceState)state);
        }

        /// <summary>
        /// Creates a IxxxBlockStateView object.
        /// </summary>
        private protected override ReadOnlyBlockStateView CreateBlockStateView(IReadOnlyBlockState blockState)
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
        private protected override IFrameBlockCellView CreateBlockCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, FrameBlockStateView blockStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusBlockCellView((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView, (FocusBlockStateView)blockStateView);
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
        private protected virtual FocusFocusList CreateFocusChain()
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
