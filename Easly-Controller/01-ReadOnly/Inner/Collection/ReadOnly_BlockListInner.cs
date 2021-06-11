namespace EaslyController.ReadOnly
{
    using System;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;

    /// <summary>
    /// Inner for a block list.
    /// </summary>
    public interface IReadOnlyBlockListInner : IReadOnlyCollectionInner
    {
        /// <summary>
        /// Block type for all blocks in the inner.
        /// </summary>
        Type BlockType { get; }

        /// <summary>
        /// Class type for all nodes in the inner.
        /// </summary>
        Type ItemType { get; }

        /// <summary>
        /// States of blocks in the block list.
        /// </summary>
        IReadOnlyBlockStateReadOnlyList BlockStateList { get; }

        /// <summary>
        /// Checks if the inner has no blocks.
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Checks if the inner has only ont child node.
        /// </summary>
        bool IsSingle { get; }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        IReadOnlyPlaceholderNodeState FirstNodeState { get; }

        /// <summary>
        /// Called when a block state is created.
        /// </summary>
        event Action<IReadOnlyBlockState> BlockStateCreated;

        /// <summary>
        /// Called when a block state is removed.
        /// </summary>
        event Action<IReadOnlyBlockState> BlockStateRemoved;

        /// <summary>
        /// Gets the index of the node at the given position.
        /// </summary>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        /// <param name="index">Position of the node in the block.</param>
        /// <returns>The index of the node at position <paramref name="blockIndex"/> and <paramref name="index"/>.</returns>
        IReadOnlyBrowsingExistingBlockNodeIndex IndexAt(int blockIndex, int index);

        /// <summary>
        /// Gets indexes for all nodes in the inner.
        /// </summary>
        IReadOnlyBrowsingBlockNodeIndexList AllIndexes();
    }

    /// <summary>
    /// Inner for a block list.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    internal interface IReadOnlyBlockListInner<out IIndex> : IReadOnlyCollectionInner<IIndex>
        where IIndex : IReadOnlyBrowsingBlockNodeIndex
    {
        /// <summary>
        /// Block type for all blocks in the inner.
        /// </summary>
        Type BlockType { get; }

        /// <summary>
        /// Class type for all nodes in the inner.
        /// </summary>
        Type ItemType { get; }

        /// <summary>
        /// States of blocks in the block list.
        /// </summary>
        IReadOnlyBlockStateReadOnlyList BlockStateList { get; }

        /// <summary>
        /// Checks if the inner has no blocks.
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Checks if the inner has only ont child node.
        /// </summary>
        bool IsSingle { get; }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        IReadOnlyPlaceholderNodeState FirstNodeState { get; }

        /// <summary>
        /// Called when a block state is created.
        /// </summary>
        event Action<IReadOnlyBlockState> BlockStateCreated;

        /// <summary>
        /// Called when a block state is removed.
        /// </summary>
        event Action<IReadOnlyBlockState> BlockStateRemoved;

        /// <summary>
        /// Gets the index of the node at the given position.
        /// </summary>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        /// <param name="index">Position of the node in the block.</param>
        /// <returns>The index of the node at position <paramref name="blockIndex"/> and <paramref name="index"/>.</returns>
        IReadOnlyBrowsingExistingBlockNodeIndex IndexAt(int blockIndex, int index);

        /// <summary>
        /// Gets indexes for all nodes in the inner.
        /// </summary>
        IReadOnlyBrowsingBlockNodeIndexList AllIndexes();

        /// <summary>
        /// Creates and initializes a new block state in the inner.
        /// </summary>
        /// <param name="newBlockIndex">Index of the new block state to create.</param>
        /// <returns>The created block state.</returns>
        IReadOnlyBlockState InitNewBlock(IReadOnlyBrowsingNewBlockNodeIndex newBlockIndex);

        /// <summary>
        /// Requests that the notification that a block has been removed be sent.
        /// </summary>
        /// <param name="blockState">The removed block.</param>
        void NotifyBlockStateRemoved(IReadOnlyBlockState blockState);
    }

    /// <summary>
    /// Inner for a block list.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index as interface.</typeparam>
    /// <typeparam name="TIndex">Type of the index as class.</typeparam>
    internal class ReadOnlyBlockListInner<IIndex, TIndex> : ReadOnlyCollectionInner<IIndex, TIndex>, IReadOnlyBlockListInner<IIndex>, IReadOnlyBlockListInner
        where IIndex : IReadOnlyBrowsingBlockNodeIndex
        where TIndex : ReadOnlyBrowsingBlockNodeIndex, IIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyBlockListInner{IIndex, TIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public ReadOnlyBlockListInner(IReadOnlyNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
            _BlockStateList = CreateBlockStateList();
            BlockStateList = _BlockStateList.ToReadOnly();
        }

        /// <summary>
        /// Creates and initializes a new block state in the inner.
        /// </summary>
        /// <param name="newBlockIndex">Index of the new block state to create.</param>
        /// <returns>The created block state.</returns>
        public virtual IReadOnlyBlockState InitNewBlock(IReadOnlyBrowsingNewBlockNodeIndex newBlockIndex)
        {
            Debug.Assert(newBlockIndex != null);
            Debug.Assert(newBlockIndex.PropertyName == PropertyName);

            int BlockIndex = newBlockIndex.BlockIndex;
            Debug.Assert(BlockIndex == BlockStateList.Count);

            NodeTreeHelperBlockList.GetChildBlock(Owner.Node, PropertyName, BlockIndex, out IBlock ChildBlock);

            IReadOnlyBlockState BlockState = CreateBlockState(newBlockIndex, ChildBlock);
            Debug.Assert(BlockState.ToString() != null); // For code coverage.
            InsertInBlockStateList(BlockIndex, BlockState);

            return BlockState;
        }

        /// <summary>
        /// Initializes a newly created state for a node in the inner.
        /// </summary>
        /// <param name="nodeIndex">Index of the node.</param>
        /// <returns>The created node state.</returns>
        public override IReadOnlyNodeState InitChildState(IReadOnlyBrowsingChildIndex nodeIndex)
        {
            Debug.Assert(nodeIndex is IReadOnlyBrowsingExistingBlockNodeIndex);
            return InitChildState((IReadOnlyBrowsingExistingBlockNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Initializes a newly created state for a node in the inner.
        /// </summary>
        /// <param name="existingNodeIndex">Index of the node.</param>
        /// <returns>The created node state.</returns>
        private protected virtual IReadOnlyPlaceholderNodeState InitChildState(IReadOnlyBrowsingExistingBlockNodeIndex existingNodeIndex)
        {
            Debug.Assert(existingNodeIndex != null);
            Debug.Assert(existingNodeIndex.PropertyName == PropertyName);

            int BlockIndex = existingNodeIndex.BlockIndex;
            int Index = existingNodeIndex.Index;

            Debug.Assert(BlockIndex < BlockStateList.Count);

            IReadOnlyPlaceholderNodeState State = CreateNodeState(existingNodeIndex);
            IReadOnlyBlockState CurrentBlock = BlockStateList[BlockIndex];
            ((IReadOnlyBlockState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>)CurrentBlock).InitNodeState(State);

            return State;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Checks if the inner must have at list one item.
        /// </summary>
        public override bool IsNeverEmpty { get { return NodeHelper.IsCollectionNeverEmpty(Owner.Node, PropertyName); } }

        /// <summary>
        /// Checks if the inner has no blocks.
        /// </summary>
        public bool IsEmpty { get { return BlockStateList.Count == 0; } }

        /// <summary>
        /// Checks if the inner has only ont child node.
        /// </summary>
        public bool IsSingle { get { return BlockStateList.Count == 1 && BlockStateList[0].StateList.Count == 1; } }

        /// <summary>
        /// Interface type for all nodes in the inner.
        /// </summary>
        public override Type InterfaceType { get { return NodeTreeHelperBlockList.BlockListInterfaceType(Owner.Node, PropertyName); } }

        /// <summary>
        /// Block type for all blocks in the inner.
        /// </summary>
        public virtual Type BlockType { get { return NodeTreeHelperBlockList.BlockListBlockType(Owner.Node, PropertyName); } }

        /// <summary>
        /// Class type for all nodes in the inner. Must inherit from <see cref="InterfaceType"/>.
        /// </summary>
        public virtual Type ItemType { get { return NodeTreeHelperBlockList.BlockListItemType(Owner.Node, PropertyName); } }

        /// <summary>
        /// States of blocks in the block list.
        /// </summary>
        public IReadOnlyBlockStateReadOnlyList BlockStateList { get; }
        private IReadOnlyBlockStateList _BlockStateList;
#pragma warning disable 1591
        [Conditional("DEBUG")]
        public void DebugGetBlockStateList() { DebugObjects.AddReference(_BlockStateList); }
#pragma warning restore 1591

        /// <summary>
        /// Called when a block state is created.
        /// </summary>
        public event Action<IReadOnlyBlockState> BlockStateCreated
        {
            add { AddBlockStateCreatedDelegate(value); }
            remove { RemoveBlockStateCreatedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IReadOnlyBlockState> BlockStateCreatedHandler;
        private protected virtual void AddBlockStateCreatedDelegate(Action<IReadOnlyBlockState> handler) { BlockStateCreatedHandler += handler; }
        private protected virtual void RemoveBlockStateCreatedDelegate(Action<IReadOnlyBlockState> handler) { BlockStateCreatedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a block state is removed.
        /// </summary>
        public event Action<IReadOnlyBlockState> BlockStateRemoved
        {
            add { AddBlockStateRemovedDelegate(value); }
            remove { RemoveBlockStateRemovedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IReadOnlyBlockState> BlockStateRemovedHandler;
        private protected virtual void AddBlockStateRemovedDelegate(Action<IReadOnlyBlockState> handler) { BlockStateRemovedHandler += handler; }
        private protected virtual void RemoveBlockStateRemovedDelegate(Action<IReadOnlyBlockState> handler) { BlockStateRemovedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Count of all node states in the inner.
        /// </summary>
        public override int Count
        {
            get
            {
                int Result = 0;

                foreach (IReadOnlyBlockState Block in BlockStateList)
                    Result += Block.StateList.Count;

                return Result;
            }
        }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        public virtual IReadOnlyPlaceholderNodeState FirstNodeState
        {
            get
            {
                Debug.Assert(BlockStateList.Count > 0);
                Debug.Assert(BlockStateList[0].StateList.Count > 0);

                DebugGetBlockStateList();
                return BlockStateList[0].StateList[0];
            }
        }
        #endregion

        #region Client Interface
        /// <summary>
        /// Gets the index of the node at the given position.
        /// </summary>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        /// <param name="index">Position of the node in the block.</param>
        /// <returns>The index of the node at position <paramref name="blockIndex"/> and <paramref name="index"/>.</returns>
        public virtual IReadOnlyBrowsingExistingBlockNodeIndex IndexAt(int blockIndex, int index)
        {
            Debug.Assert(blockIndex >= 0 && blockIndex < BlockStateList.Count);

            IReadOnlyBlockState BlockState = BlockStateList[blockIndex];

            Debug.Assert(index >= 0 && index < BlockState.StateList.Count);

            return (IReadOnlyBrowsingExistingBlockNodeIndex)BlockState.StateList[index].ParentIndex;
        }

        /// <summary>
        /// Gets indexes for all nodes in the inner.
        /// </summary>
        public virtual IReadOnlyBrowsingBlockNodeIndexList AllIndexes()
        {
            IReadOnlyBrowsingBlockNodeIndexList Result = CreateBlockNodeIndexList();

            foreach (IReadOnlyBlockState BlockState in BlockStateList)
                foreach (IReadOnlyPlaceholderNodeState NodeState in BlockState.StateList)
                {
                    IReadOnlyBrowsingBlockNodeIndex ParentIndex = NodeState.ParentIndex as IReadOnlyBrowsingBlockNodeIndex;
                    Debug.Assert(ParentIndex != null);

                    Result.Add(ParentIndex);
                }

            return Result;
        }

        /// <summary>
        /// Creates a clone of all children of the inner, using <paramref name="parentNode"/> as their parent.
        /// </summary>
        /// <param name="parentNode">The node that will contains references to cloned children upon return.</param>
        public override void CloneChildren(INode parentNode)
        {
            Debug.Assert(parentNode != null);

            NodeTreeHelperBlockList.ClearChildBlockList(parentNode, PropertyName);

            // Clone and insert all blocks. This will clone all children recursively.
            for (int BlockIndex = 0; BlockIndex < BlockStateList.Count; BlockIndex++)
            {
                IReadOnlyBlockState BlockState = BlockStateList[BlockIndex];
                ((IReadOnlyBlockState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>)BlockState).CloneBlock(parentNode, BlockIndex);
            }

            // Copy comments.
            IBlockList BlockList = NodeTreeHelperBlockList.GetBlockList(Owner.Node, PropertyName);
            IBlockList NewBlockList = NodeTreeHelperBlockList.GetBlockList(parentNode, PropertyName);
            NodeTreeHelper.CopyDocumentation(BlockList, NewBlockList, cloneCommentGuid: true);
        }

        /// <summary>
        /// Attach a view to the inner.
        /// </summary>
        /// <param name="view">The attaching view.</param>
        /// <param name="callbackSet">The set of callbacks to call when enumerating existing states.</param>
        public override void Attach(IReadOnlyControllerView view, IReadOnlyAttachCallbackSet callbackSet)
        {
            callbackSet.OnBlockListInnerAttached(this);

            foreach (IReadOnlyBlockState BlockState in BlockStateList)
                ((IReadOnlyBlockState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>)BlockState).Attach(view, callbackSet);
        }

        /// <summary>
        /// Detach a view from the inner.
        /// </summary>
        /// <param name="view">The attaching view.</param>
        /// <param name="callbackSet">The set of callbacks to no longer call when enumerating existing states.</param>
        public override void Detach(IReadOnlyControllerView view, IReadOnlyAttachCallbackSet callbackSet)
        {
            foreach (IReadOnlyBlockState BlockState in BlockStateList)
                ((IReadOnlyBlockState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>)BlockState).Detach(view, callbackSet);

            callbackSet.OnBlockListInnerDetached(this);
        }

        /// <summary>
        /// Requests that the notification that a block has been removed be sent.
        /// </summary>
        /// <param name="blockState">The removed block.</param>
        public virtual void NotifyBlockStateRemoved(IReadOnlyBlockState blockState)
        {
            BlockStateRemovedHandler?.Invoke(blockState);
        }
        #endregion

        #region Descendant Interface
        private protected virtual void InsertInBlockStateList(int blockIndex, IReadOnlyBlockState blockState)
        {
            Debug.Assert(blockIndex >= 0 && blockIndex <= BlockStateList.Count);
            Debug.Assert(blockState != null);

            _BlockStateList.Insert(blockIndex, blockState);

            NotifyBlockStateCreated(BlockStateList[blockIndex]);
        }

        private protected virtual void RemoveFromBlockStateList(int blockIndex)
        {
            Debug.Assert(blockIndex >= 0 && blockIndex < BlockStateList.Count);

            NotifyBlockStateRemoved(BlockStateList[blockIndex]);

            _BlockStateList.RemoveAt(blockIndex);
        }

        private protected virtual void MoveInBlockStateList(int blockIndex, int direction)
        {
            Debug.Assert(blockIndex >= 0 && blockIndex < BlockStateList.Count);
            Debug.Assert(blockIndex + direction >= 0 && blockIndex + direction < BlockStateList.Count);

            IReadOnlyBlockState BlockState = _BlockStateList[blockIndex];
            _BlockStateList.RemoveAt(blockIndex);
            _BlockStateList.Insert(blockIndex + direction, BlockState);
        }

        private protected virtual void NotifyBlockStateCreated(IReadOnlyBlockState blockState)
        {
            BlockStateCreatedHandler?.Invoke(blockState);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="ReadOnlyBlockListInner{IIndex,TIndex}"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out ReadOnlyBlockListInner<IIndex, TIndex> AsBlockListInner))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsBlockListInner))
                return comparer.Failed();

            if (!comparer.IsSameCount(BlockStateList.Count, AsBlockListInner.BlockStateList.Count))
                return comparer.Failed();

            for (int i = 0; i < BlockStateList.Count; i++)
                if (!comparer.VerifyEqual(BlockStateList[i], AsBlockListInner.BlockStateList[i]))
                    return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxBlockStateList object.
        /// </summary>
        private protected virtual IReadOnlyBlockStateList CreateBlockStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockListInner<IIndex, TIndex>));
            return new ReadOnlyBlockStateList();
        }

        /// <summary>
        /// Creates a IxxxBlockState object.
        /// </summary>
        private protected virtual IReadOnlyBlockState CreateBlockState(IReadOnlyBrowsingNewBlockNodeIndex nodeIndex, IBlock childBlock)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockListInner<IIndex, TIndex>));
            return new ReadOnlyBlockState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>(this, nodeIndex, childBlock);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        private protected virtual IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockListInner<IIndex, TIndex>));
            return new ReadOnlyPlaceholderNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>(nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingBlockNodeIndexList.
        /// </summary>
        private protected virtual IReadOnlyBrowsingBlockNodeIndexList CreateBlockNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockListInner<IIndex, TIndex>));
            return new ReadOnlyBrowsingBlockNodeIndexList();
        }
        #endregion
    }
}
