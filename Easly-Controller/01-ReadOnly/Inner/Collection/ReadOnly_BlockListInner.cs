using BaseNode;
using BaseNodeHelper;
using System;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// Inner for a block list.
    /// </summary>
    public interface IReadOnlyBlockListInner : IReadOnlyCollectionInner
    {
        /// <summary>
        /// Class type for all nodes in the inner.
        /// </summary>
        Type ItemType { get; }

        /// <summary>
        /// States of blocks in the block list.
        /// </summary>
        IReadOnlyBlockStateReadOnlyList BlockStateList { get; }

        /// <summary>
        /// Called when a block state is created.
        /// </summary>
        event Action<IReadOnlyBlockState> BlockStateCreated;

        /// <summary>
        /// Called when a block state is removed.
        /// </summary>
        event Action<IReadOnlyBlockState> BlockStateRemoved;
    }

    /// <summary>
    /// Inner for a block list.
    /// </summary>
    public interface IReadOnlyBlockListInner<out IIndex> : IReadOnlyCollectionInner<IIndex>
        where IIndex : IReadOnlyBrowsingBlockNodeIndex
    {
        /// <summary>
        /// Class type for all nodes in the inner.
        /// </summary>
        Type ItemType { get; }

        /// <summary>
        /// States of blocks in the block list.
        /// </summary>
        IReadOnlyBlockStateReadOnlyList BlockStateList { get; }

        /// <summary>
        /// Creates and initializes a new block state in the inner.
        /// </summary>
        /// <param name="nodeIndex">Index of the new block state to create.</param>
        /// <returns>The created block state.</returns>
        IReadOnlyBlockState InitNewBlock(IReadOnlyBrowsingNewBlockNodeIndex nodeIndex);
    }

    /// <summary>
    /// Inner for a block list.
    /// </summary>
    public class ReadOnlyBlockListInner<IIndex, TIndex> : ReadOnlyCollectionInner<IIndex, TIndex>, IReadOnlyBlockListInner<IIndex>, IReadOnlyBlockListInner
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
            BlockStateList = CreateBlockStateListReadOnly(_BlockStateList);
        }

        /// <summary>
        /// Creates and initializes a new block state in the inner.
        /// </summary>
        /// <param name="nodeIndex">Index of the new block state to create.</param>
        /// <returns>The created block state.</returns>
        public virtual IReadOnlyBlockState InitNewBlock(IReadOnlyBrowsingNewBlockNodeIndex nodeIndex)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(nodeIndex.PropertyName == PropertyName);

            int BlockIndex = nodeIndex.BlockIndex;
            Debug.Assert(BlockIndex == BlockStateList.Count);

            NodeTreeHelper.GetChildBlock(Owner.Node, PropertyName, BlockIndex, out IBlock ChildBlock);

            IReadOnlyBlockState BlockState = CreateBlockState(nodeIndex, ChildBlock);
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
            Debug.Assert(nodeIndex is IReadOnlyBrowsingBlockNodeIndex);
            return InitChildState((IReadOnlyBrowsingBlockNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Initializes a newly created state for a node in the inner.
        /// </summary>
        /// <param name="nodeIndex">Index of the node.</param>
        /// <returns>The created node state.</returns>
        protected virtual IReadOnlyPlaceholderNodeState InitChildState(IReadOnlyBrowsingBlockNodeIndex nodeIndex)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(nodeIndex.PropertyName == PropertyName);

            int BlockIndex;
            int Index;

            switch (nodeIndex)
            {
                case IReadOnlyBrowsingNewBlockNodeIndex AsNewBlockIndex:
                    BlockIndex = AsNewBlockIndex.BlockIndex;
                    Index = 0;
                    break;

                case IReadOnlyBrowsingExistingBlockNodeIndex AsExistingBlockIndex:
                    BlockIndex = AsExistingBlockIndex.BlockIndex;
                    Index = AsExistingBlockIndex.Index;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(nodeIndex));
            }

            Debug.Assert(BlockIndex < BlockStateList.Count);

            IReadOnlyPlaceholderNodeState State = CreateNodeState(nodeIndex);
            IReadOnlyBlockState CurrentBlock = BlockStateList[BlockIndex];
            CurrentBlock.InitNodeState(State);

            return State;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Interface type for all nodes in the inner.
        /// </summary>
        public override Type InterfaceType { get { return NodeTreeHelper.BlockListInterfaceType(Owner.Node, PropertyName); } }

        /// <summary>
        /// Class type for all nodes in the inner. Must inherit from <see cref="InterfaceType"/>.
        /// </summary>
        public virtual Type ItemType { get { return NodeTreeHelper.BlockListItemType(Owner.Node, PropertyName); } }

        /// <summary>
        /// States of blocks in the block list.
        /// </summary>
        public IReadOnlyBlockStateReadOnlyList BlockStateList { get; }
        private IReadOnlyBlockStateList _BlockStateList;

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
        public override IReadOnlyPlaceholderNodeState FirstNodeState
        {
            get
            {
                Debug.Assert(BlockStateList.Count > 0);
                Debug.Assert(BlockStateList[0].StateList.Count > 0);

                return BlockStateList[0].StateList[0];
            }
        }
        #endregion

        #region Client Interface
        /// <summary>
        /// Creates a clone of all children of the inner, using <paramref name="parentNode"/> as their parent.
        /// </summary>
        /// <param name="parentNode">The node that will contains references to cloned children upon return.</param>
        public override void CloneChildren(INode parentNode)
        {
            Debug.Assert(parentNode != null);

            // Create an empty block list in the parent.
            NodeHelper.InitializeEmptyBlockList(parentNode, PropertyName, InterfaceType, ItemType);

            // Clone and insert all blocks. This will clone all children recursively.
            foreach (IReadOnlyBlockState BlockState in BlockStateList)
                BlockState.CloneBlock(parentNode);

            // Copy comments.
            IBlockList BlockList = NodeTreeHelper.GetBlockList(Owner.Node, PropertyName);
            IBlockList NewBlockList = NodeTreeHelper.GetBlockList(parentNode, PropertyName);
            NodeTreeHelper.CopyDocumentation(BlockList, NewBlockList);
        }

        /// <summary>
        /// Attach a view to the inner.
        /// </summary>
        /// <param name="view">The attaching view.</param>
        /// <param name="callbackSet">The set of callbacks to call when enumerating existing states.</param>
        public override void Attach(IReadOnlyControllerView view, IReadOnlyAttachCallbackSet callbackSet)
        {
            callbackSet.BlockListInnerAttachedHandler(this);

            foreach (IReadOnlyBlockState BlockState in BlockStateList)
                BlockState.Attach(view, callbackSet);
        }

        /// <summary>
        /// Called when a block state is created.
        /// </summary>
        public event Action<IReadOnlyBlockState> BlockStateCreated;

        protected virtual void NotifyBlockStateCreated(IReadOnlyBlockState blockState)
        {
            BlockStateCreated?.Invoke(blockState);
        }

        /// <summary>
        /// Called when a block state is removed.
        /// </summary>
        public event Action<IReadOnlyBlockState> BlockStateRemoved;

        protected virtual void NotifyBlockStateRemoved(IReadOnlyBlockState blockState)
        {
            BlockStateRemoved?.Invoke(blockState);
        }
        #endregion

        #region BlockStateList
        protected virtual void InsertInBlockStateList(int blockIndex, IReadOnlyBlockState blockState)
        {
            Debug.Assert(blockIndex >= 0 && blockIndex <= BlockStateList.Count);
            Debug.Assert(blockState != null);

            _BlockStateList.Insert(blockIndex, blockState);

            NotifyBlockStateCreated(BlockStateList[blockIndex]);
        }

        protected virtual void RemoveFromBlockStateList(int blockIndex)
        {
            Debug.Assert(blockIndex >= 0 && blockIndex < BlockStateList.Count);

            NotifyBlockStateRemoved(BlockStateList[blockIndex]);

            _BlockStateList.RemoveAt(blockIndex);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxBlockStateList object.
        /// </summary>
        protected virtual IReadOnlyBlockStateList CreateBlockStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockListInner<IIndex, TIndex>));
            return new ReadOnlyBlockStateList();
        }

        /// <summary>
        /// Creates a IxxxBlockStateReadOnlyList object.
        /// </summary>
        protected virtual IReadOnlyBlockStateReadOnlyList CreateBlockStateListReadOnly(IReadOnlyBlockStateList blockStateList)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockListInner<IIndex, TIndex>));
            return new ReadOnlyBlockStateReadOnlyList(blockStateList);
        }

        /// <summary>
        /// Creates a IxxxBlockState object.
        /// </summary>
        protected virtual IReadOnlyBlockState CreateBlockState(IReadOnlyBrowsingNewBlockNodeIndex nodeIndex, IBlock childBlock)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockListInner<IIndex, TIndex>));
            return new ReadOnlyBlockState(this, nodeIndex, childBlock);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        protected virtual IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockListInner<IIndex, TIndex>));
            return new ReadOnlyPlaceholderNodeState(nodeIndex);
        }

        /// <summary>
        /// Creates an index object.
        /// </summary>
        protected virtual IIndex CreateNodeIndex(IReadOnlyPlaceholderNodeState state, string propertyName, int blockIndex, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockListInner<IIndex, TIndex>));
            return (TIndex)(IReadOnlyBrowsingBlockNodeIndex)new ReadOnlyBrowsingExistingBlockNodeIndex(Owner.Node, state.Node, propertyName, blockIndex, index);
        }
        #endregion
    }
}
