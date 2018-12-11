using BaseNode;
using BaseNodeHelper;
using System;
using System.Collections.Generic;
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
        void SetBlock(IReadOnlyBrowsingNewBlockNodeIndex nodeIndex, Func<IReadOnlyBlockState, IReadOnlyPatternState> createPatternState, Func<IReadOnlyBlockState, IReadOnlySourceState> createSourceState, bool isEnumerating);
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
        #endregion

        #region Build Table Interface
        public override void Set(IReadOnlyBrowsingChildNodeIndex nodeIndex, IReadOnlyNodeState childState, bool isEnumerating)
        {
            Debug.Assert(childState != null);

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

            ReadOnlyBlockState CurrentBlock = (ReadOnlyBlockState)BlockStateList[BlockIndex];
            Debug.Assert(isEnumerating ? (Index < CurrentBlock.StateList.Count) : (Index == CurrentBlock.StateList.Count));

            CurrentBlock.SetState(childState, isEnumerating);
        }

        public virtual void SetBlock(IReadOnlyBrowsingNewBlockNodeIndex nodeIndex, Func<IReadOnlyBlockState, IReadOnlyPatternState> createPatternState, Func<IReadOnlyBlockState, IReadOnlySourceState> createSourceState, bool isEnumerating)
        {
            string PropertyName = nodeIndex.PropertyName;
            int BlockIndex = nodeIndex.BlockIndex;
            IReadOnlyNodeState ParentState = Owner;

            NodeTreeHelper.GetChildBlock(ParentState.Node, PropertyName, BlockIndex, out IBlock ChildBlock);

            IReadOnlyBlockState NewBlock;
            if (isEnumerating)
            {
                Debug.Assert(BlockIndex < BlockStateList.Count);
                NewBlock = BlockStateList[BlockIndex];
            }
            else
            {
                Debug.Assert(BlockIndex == BlockStateList.Count);
                NewBlock = CreateBlockState(ChildBlock, createPatternState, createSourceState);
            }

            NewBlock.Init(createPatternState, createSourceState, isEnumerating);

            InsertInBlockStateList(BlockIndex, NewBlock, isEnumerating);
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

        public override IReadOnlyNodeState FirstNodeState()
        {
            Debug.Assert(BlockStateList.Count > 0);
            Debug.Assert(BlockStateList[0].StateList.Count > 0);

            return BlockStateList[0].StateList[0];
        }
        #endregion

        #region BlockStateList
        protected virtual void InitBlockStateList()
        {
            _BlockStateList = CreateBlockStateList();
            BlockStateList = CreateBlockStateListReadOnly(_BlockStateList);
        }

        protected virtual void InsertInBlockStateList(int blockIndex, IReadOnlyBlockState blockState, bool isEnumerating)
        {
            if (isEnumerating)
                Debug.Assert(_BlockStateList[blockIndex] == blockState);
            else
                _BlockStateList.Insert(blockIndex, blockState);
        }

        protected virtual void RemoveFromBlockStateList(int blockIndex)
        {
            _BlockStateList.RemoveAt(blockIndex);
        }

        public IReadOnlyBlockStateReadOnlyList BlockStateList { get; protected set; }
        protected IReadOnlyBlockStateList _BlockStateList;
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

        protected virtual IReadOnlyBlockState CreateBlockState(IBlock childBlock, Func<IReadOnlyBlockState, IReadOnlyPatternState> createPatternState, Func<IReadOnlyBlockState, IReadOnlySourceState> createSourceState)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockListInner<IIndex, TIndex>));
            return new ReadOnlyBlockState(this, childBlock, createPatternState, createSourceState);
        }

        protected virtual IIndex CreateNodeIndex(IReadOnlyNodeState state, string propertyName, int blockIndex, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockListInner<IIndex, TIndex>));
            return (TIndex)(ReadOnlyBrowsingBlockNodeIndex)new ReadOnlyBrowsingExistingBlockNodeIndex(Owner.Node, state.Node, propertyName, blockIndex, index);
        }
        #endregion
    }
}
