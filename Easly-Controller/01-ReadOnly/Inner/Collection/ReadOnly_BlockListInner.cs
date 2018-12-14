using BaseNode;
using BaseNodeHelper;
using System;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBlockListInner : IReadOnlyCollectionInner
    {
        Type ItemType { get; }
        IReadOnlyBlockStateReadOnlyList BlockStateList { get; }
    }

    public interface IReadOnlyBlockListInner<out IIndex> : IReadOnlyCollectionInner<IIndex>
        where IIndex : IReadOnlyBrowsingBlockNodeIndex
    {
        Type ItemType { get; }
        IReadOnlyBlockStateReadOnlyList BlockStateList { get; }
        IReadOnlyBlockState InitNewBlock(IReadOnlyBrowsingNewBlockNodeIndex nodeIndex);
    }

    public class ReadOnlyBlockListInner<IIndex, TIndex> : ReadOnlyCollectionInner<IIndex, TIndex>, IReadOnlyBlockListInner<IIndex>, IReadOnlyBlockListInner
        where IIndex : IReadOnlyBrowsingBlockNodeIndex
        where TIndex : ReadOnlyBrowsingBlockNodeIndex, IIndex
    {
        #region Init
        public ReadOnlyBlockListInner(IReadOnlyNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
            _BlockStateList = CreateBlockStateList();
            BlockStateList = CreateBlockStateListReadOnly(_BlockStateList);
        }

        public virtual IReadOnlyBlockState InitNewBlock(IReadOnlyBrowsingNewBlockNodeIndex nodeIndex)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(nodeIndex.PropertyName == PropertyName);

            int BlockIndex = nodeIndex.BlockIndex;
            Debug.Assert(BlockIndex == BlockStateList.Count);

            NodeTreeHelper.GetChildBlock(Owner.Node, PropertyName, BlockIndex, out IBlock ChildBlock);

            IReadOnlyBlockState BlockState = CreateBlockState(ChildBlock);
            InsertInBlockStateList(BlockIndex, BlockState);

            return BlockState;
        }

        public override IReadOnlyNodeState InitChildState(IReadOnlyBrowsingChildIndex nodeIndex)
        {
            Debug.Assert(nodeIndex is IReadOnlyBrowsingBlockNodeIndex);
            return InitChildState((IReadOnlyBrowsingBlockNodeIndex)nodeIndex);
        }

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
        public override Type InterfaceType { get { return NodeTreeHelper.BlockListInterfaceType(Owner.Node, PropertyName); } }
        public virtual Type ItemType { get { return NodeTreeHelper.BlockListItemType(Owner.Node, PropertyName); } }
        public IReadOnlyBlockStateReadOnlyList BlockStateList { get; }
        private IReadOnlyBlockStateList _BlockStateList;

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
        public override void CloneChildren(INode parentNode)
        {
            Debug.Assert(parentNode != null);

            NodeHelper.InitializeEmptyBlockList(parentNode, PropertyName, InterfaceType, ItemType);

            for (int i = 0; i < BlockStateList.Count; i++)
            {
                IReadOnlyBlockState BlockState = BlockStateList[i];
                BlockState.CloneBlock(parentNode, i);
            }

            IBlockList BlockList = NodeTreeHelper.GetBlockList(Owner.Node, PropertyName);
            IBlockList NewBlockList = NodeTreeHelper.GetBlockList(parentNode, PropertyName);
            NodeTreeHelper.CopyDocumentation(BlockList, NewBlockList);
        }
        #endregion

        #region BlockStateList
        protected virtual void InsertInBlockStateList(int blockIndex, IReadOnlyBlockState blockState)
        {
            Debug.Assert(blockIndex >= 0 && blockIndex <= BlockStateList.Count);
            Debug.Assert(blockState != null);

            _BlockStateList.Insert(blockIndex, blockState);
        }

        protected virtual void RemoveFromBlockStateList(int blockIndex)
        {
            Debug.Assert(blockIndex >= 0 && blockIndex < BlockStateList.Count);

            _BlockStateList.RemoveAt(blockIndex);
        }
        #endregion

        #region Create Methods
        protected virtual IReadOnlyBlockStateList CreateBlockStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockListInner<IIndex, TIndex>));
            return new ReadOnlyBlockStateList();
        }

        protected virtual IReadOnlyBlockStateReadOnlyList CreateBlockStateListReadOnly(IReadOnlyBlockStateList blockStateList)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockListInner<IIndex, TIndex>));
            return new ReadOnlyBlockStateReadOnlyList(blockStateList);
        }

        protected virtual IReadOnlyBlockState CreateBlockState(IBlock childBlock)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockListInner<IIndex, TIndex>));
            return new ReadOnlyBlockState(this, childBlock);
        }

        protected virtual IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockListInner<IIndex, TIndex>));
            return new ReadOnlyPlaceholderNodeState(nodeIndex);
        }

        protected virtual IIndex CreateNodeIndex(IReadOnlyPlaceholderNodeState state, string propertyName, int blockIndex, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockListInner<IIndex, TIndex>));
            return (TIndex)(ReadOnlyBrowsingBlockNodeIndex)new ReadOnlyBrowsingExistingBlockNodeIndex(Owner.Node, state.Node, propertyName, blockIndex, index);
        }
        #endregion
    }
}
