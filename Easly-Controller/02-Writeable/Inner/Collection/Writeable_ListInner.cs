using BaseNode;
using BaseNodeHelper;
using EaslyController.ReadOnly;
using System;
using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Inner for a list of nodes.
    /// </summary>
    public interface IWriteableListInner : IReadOnlyListInner, IWriteableCollectionInner
    {
        /// <summary>
        /// States of nodes in the list.
        /// </summary>
        new IWriteablePlaceholderNodeStateReadOnlyList StateList { get; }
    }

    /// <summary>
    /// Inner for a list of nodes.
    /// </summary>
    public interface IWriteableListInner<out IIndex> : IReadOnlyListInner<IIndex>, IWriteableCollectionInner<IIndex>
        where IIndex : IWriteableBrowsingListNodeIndex
    {
        /// <summary>
        /// States of nodes in the list.
        /// </summary>
        new IWriteablePlaceholderNodeStateReadOnlyList StateList { get; }
    }

    /// <summary>
    /// Inner for a list of nodes.
    /// </summary>
    public class WriteableListInner<IIndex, TIndex> : ReadOnlyListInner<IIndex, TIndex>, IWriteableListInner<IIndex>, IWriteableListInner
        where IIndex : IWriteableBrowsingListNodeIndex
        where TIndex : WriteableBrowsingListNodeIndex, IIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableListInner{IIndex, TIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public WriteableListInner(IWriteableNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        public new IWriteableNodeState Owner { get { return (IWriteableNodeState)base.Owner; } }

        /// <summary>
        /// States of nodes in the list.
        /// </summary>
        public new IWriteablePlaceholderNodeStateReadOnlyList StateList { get { return (IWriteablePlaceholderNodeStateReadOnlyList)base.StateList; } }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        public new IWriteablePlaceholderNodeState FirstNodeState { get { return (IWriteablePlaceholderNodeState)base.FirstNodeState; } }
        #endregion

        #region Client Interface

        /// <summary>
        /// Inserts a new node in a list.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void Insert(IWriteableInsertNodeOperation operation)
        {
            Debug.Assert(operation != null);

            int InsertionIndex = operation.Index;
            Debug.Assert(InsertionIndex >= 0 && InsertionIndex <= StateList.Count);

            INode ParentNode = Owner.Node;
            INode Node = operation.Node;
            NodeTreeHelperList.InsertIntoList(ParentNode, PropertyName, InsertionIndex, Node);

            IWriteableBrowsingListNodeIndex BrowsingIndex = CreateBrowsingNodeIndex(Node, InsertionIndex);
            IWriteablePlaceholderNodeState ChildState = (IWriteablePlaceholderNodeState)CreateNodeState(BrowsingIndex);

            InsertInStateList(InsertionIndex, ChildState);

            operation.Update(BrowsingIndex, ChildState);

            while (++InsertionIndex < StateList.Count)
            {
                IWriteablePlaceholderNodeState State = StateList[InsertionIndex];

                IWriteableBrowsingListNodeIndex NodeIndex = State.ParentIndex as IWriteableBrowsingListNodeIndex;
                Debug.Assert(NodeIndex != null);
                Debug.Assert(NodeIndex.Index == InsertionIndex - 1);

                NodeIndex.MoveUp();
            }
        }

        /// <summary>
        /// Removes a node from a list or block list.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void Remove(IWriteableRemoveNodeOperation operation)
        {
            Debug.Assert(operation != null);

            int RemoveIndex = operation.Index;
            Debug.Assert(RemoveIndex >= 0 && RemoveIndex < StateList.Count);

            IWriteablePlaceholderNodeState OldChildState = StateList[RemoveIndex];
            RemoveFromStateList(RemoveIndex);

            INode ParentNode = Owner.Node;
            NodeTreeHelperList.RemoveFromList(ParentNode, PropertyName, RemoveIndex);

            while (RemoveIndex < StateList.Count)
            {
                IWriteablePlaceholderNodeState State = StateList[RemoveIndex];

                IWriteableBrowsingListNodeIndex NodeIndex = State.ParentIndex as IWriteableBrowsingListNodeIndex;
                Debug.Assert(NodeIndex != null);
                Debug.Assert(NodeIndex.Index == RemoveIndex + 1);

                NodeIndex.MoveDown();

                RemoveIndex++;
            }

            operation.Update(OldChildState);
        }

        /// <summary>
        /// Replaces a node.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void Replace(IWriteableReplaceOperation operation)
        {
            Debug.Assert(operation != null);

            if (operation.ReplacementIndex is IWriteableInsertionListNodeIndex AsListIndex)
                Replace(operation, AsListIndex);
            else
                throw new ArgumentOutOfRangeException(nameof(operation));
        }

        /// <summary></summary>
        protected virtual void Replace(IWriteableReplaceOperation operation, IWriteableInsertionListNodeIndex listIndex)
        {
            Debug.Assert(listIndex != null);

            int Index = listIndex.Index;
            Debug.Assert(Index >= 0 && Index < StateList.Count);

            INode ParentNode = Owner.Node;

            IWriteableNodeState OldChildState = StateList[Index];
            INode OldNode = OldChildState.Node;
            IWriteableBrowsingListNodeIndex OldBrowsingIndex = (IWriteableBrowsingListNodeIndex)OldChildState.ParentIndex;
            RemoveFromStateList(Index);

            NodeTreeHelperList.ReplaceNode(ParentNode, PropertyName, Index, listIndex.Node);

            IWriteableBrowsingListNodeIndex NewBrowsingIndex = (IWriteableBrowsingListNodeIndex)listIndex.ToBrowsingIndex();
            IWriteablePlaceholderNodeState NewChildState = (IWriteablePlaceholderNodeState)CreateNodeState(NewBrowsingIndex);
            InsertInStateList(listIndex.Index, NewChildState);

            operation.Update(OldBrowsingIndex, NewBrowsingIndex, OldNode, NewChildState);
        }

        /// <summary>
        /// Checks whether a node can be moved in a list.
        /// </summary>
        /// <param name="nodeIndex">Index of the node that would be moved.</param>
        /// <param name="direction">Direction of the move, relative to the current position of the item.</param>
        public virtual bool IsMoveable(IWriteableBrowsingCollectionNodeIndex nodeIndex, int direction)
        {
            if (nodeIndex is IWriteableBrowsingListNodeIndex AsListIndex)
                return IsMoveable(AsListIndex, direction);
            else
                throw new ArgumentOutOfRangeException(nameof(nodeIndex));
        }

        /// <summary></summary>
        protected virtual bool IsMoveable(IWriteableBrowsingListNodeIndex listIndex, int direction)
        {
            Debug.Assert(listIndex != null);

            int NewPosition = listIndex.Index + direction;
            bool Result = NewPosition >= 0 && NewPosition < StateList.Count;

            return Result;
        }

        /// <summary>
        /// Moves a node around in a list or block list. In a block list, the node stays in same block.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void Move(IWriteableMoveNodeOperation operation)
        {
            Debug.Assert(operation != null);

            if (operation.NodeIndex is IWriteableBrowsingListNodeIndex AsListIndex)
                Move(operation, AsListIndex);
            else
                throw new ArgumentOutOfRangeException(nameof(operation));
        }

        /// <summary></summary>
        protected virtual void Move(IWriteableMoveNodeOperation operation, IWriteableBrowsingListNodeIndex listIndex)
        {
            Debug.Assert(operation != null);

            int MoveIndex = operation.Index;
            int Direction = operation.Direction;

            Debug.Assert(listIndex != null);
            Debug.Assert(MoveIndex >= 0 && MoveIndex < StateList.Count);
            Debug.Assert(MoveIndex + operation.Direction >= 0 && MoveIndex + operation.Direction < StateList.Count);

            INode ParentNode = Owner.Node;

            MoveInStateList(MoveIndex, Direction);
            NodeTreeHelperList.MoveNode(ParentNode, PropertyName, MoveIndex, Direction);

            operation.Update(StateList[MoveIndex + Direction]);

            if (Direction > 0)
            {
                for (int i = MoveIndex; i < MoveIndex + Direction; i++)
                {
                    IWriteableBrowsingListNodeIndex ChildNodeIndex = StateList[i].ParentIndex as IWriteableBrowsingListNodeIndex;
                    Debug.Assert(ChildNodeIndex != null);

                    ChildNodeIndex.MoveDown();
                    listIndex.MoveUp();
                }
            }
            else if (Direction < 0)
            {
                for (int i = MoveIndex; i > MoveIndex + Direction; i--)
                {
                    IWriteableBrowsingListNodeIndex ChildNodeIndex = StateList[i].ParentIndex as IWriteableBrowsingListNodeIndex;
                    Debug.Assert(ChildNodeIndex != null);

                    ChildNodeIndex.MoveUp();
                    listIndex.MoveDown();
                }
            }
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxPlaceholderNodeStateList object.
        /// </summary>
        protected override IReadOnlyPlaceholderNodeStateList CreateStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableListInner<IIndex, TIndex>));
            return new WriteablePlaceholderNodeStateList();
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeStateReadOnlyList object.
        /// </summary>
        protected override IReadOnlyPlaceholderNodeStateReadOnlyList CreateStateListReadOnly(IReadOnlyPlaceholderNodeStateList stateList)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableListInner<IIndex, TIndex>));
            return new WriteablePlaceholderNodeStateReadOnlyList((IWriteablePlaceholderNodeStateList)stateList);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        protected override IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableListInner<IIndex, TIndex>));
            return new WriteablePlaceholderNodeState((IWriteableNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndex object.
        /// </summary>
        protected virtual IWriteableBrowsingListNodeIndex CreateBrowsingNodeIndex(INode node, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableListInner<IIndex, TIndex>));
            return new WriteableBrowsingListNodeIndex(Owner.Node, node, PropertyName, index);
        }
        #endregion
    }
}
