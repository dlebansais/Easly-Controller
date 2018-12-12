using BaseNode;
using BaseNodeHelper;
using System;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBlockListInner : IReadOnlyCollectionInner
    {
        IReadOnlyBlockStateReadOnlyList BlockStateList { get; }
    }

    public interface IReadOnlyBlockListInner<out IIndex> : IReadOnlyCollectionInner<IIndex>
        where IIndex : IReadOnlyBrowsingBlockNodeIndex
    {
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
            InitBlockStateList();
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

        public override IReadOnlyNodeState InitChildState(IReadOnlyBrowsingChildNodeIndex nodeIndex)
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

            IReadOnlyNodeState State = CreateNodeState(nodeIndex);
            IReadOnlyBlockState CurrentBlock = BlockStateList[BlockIndex];
            CurrentBlock.InitNodeState(State);

            return State;
        }
        #endregion

        #region Properties
        public override Type ItemType { get { return NodeTreeHelper.BlockListInterfaceType(Owner.Node, PropertyName); } }

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

        public override IReadOnlyNodeState FirstNodeState
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
        public override IIndex IndexOf(IReadOnlyNodeState childState)
        {
            GetIndexOf(childState, out int BlockIndex, out int Index);
            Debug.Assert(BlockIndex >= 0 && Index >= 0);

            return CreateNodeIndex(childState, PropertyName, BlockIndex, Index);
        }

        private void GetIndexOf(IReadOnlyNodeState childState, out int stateBlockIndex, out int stateIndex)
        {
            for (int BlockIndex = 0; BlockIndex < BlockStateList.Count; BlockIndex++)
            {
                IReadOnlyBlockState Block = BlockStateList[BlockIndex];
                IReadOnlyNodeStateReadOnlyList StateList = Block.StateList;

                for (int Index = 0; Index < StateList.Count; Index++)
                    if (StateList[Index] == childState)
                    {
                        stateBlockIndex = BlockIndex;
                        stateIndex = Index;
                        return;
                    }
            }

            stateBlockIndex = -1;
            stateIndex = -1;
        }
        #endregion

        #region BlockStateList
        protected virtual void InitBlockStateList()
        {
            _BlockStateList = CreateBlockStateList();
            BlockStateList = CreateBlockStateListReadOnly(_BlockStateList);
        }

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

        public IReadOnlyBlockStateReadOnlyList BlockStateList { get; protected set; }
        private IReadOnlyBlockStateList _BlockStateList;
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

        protected virtual IReadOnlyNodeState CreateNodeState(IReadOnlyNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockListInner<IIndex, TIndex>));
            return new ReadOnlyNodeState(nodeIndex.Node);
        }

        protected virtual IIndex CreateNodeIndex(IReadOnlyNodeState state, string propertyName, int blockIndex, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockListInner<IIndex, TIndex>));
            return (TIndex)(ReadOnlyBrowsingBlockNodeIndex)new ReadOnlyBrowsingExistingBlockNodeIndex(Owner.Node, state.Node, propertyName, blockIndex, index);
        }
        #endregion
    }
}
