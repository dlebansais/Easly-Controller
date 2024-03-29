﻿namespace EaslyController.Writeable
{
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using EaslyController.ReadOnly;
    using NotNullReflection;

    /// <summary>
    /// Inner for a list of nodes.
    /// </summary>
    public interface IWriteableListInner : IReadOnlyListInner, IWriteableCollectionInner
    {
        /// <summary>
        /// States of nodes in the list.
        /// </summary>
        new WriteablePlaceholderNodeStateReadOnlyList StateList { get; }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        new IWriteablePlaceholderNodeState FirstNodeState { get; }
    }

    /// <summary>
    /// Inner for a list of nodes.
    /// </summary>
    /// <typeparam name="TIndex">Type of the index.</typeparam>
    internal interface IWriteableListInner<out TIndex> : IReadOnlyListInner<TIndex>, IWriteableCollectionInner<TIndex>
        where TIndex : IWriteableBrowsingListNodeIndex
    {
        /// <summary>
        /// States of nodes in the list.
        /// </summary>
        new WriteablePlaceholderNodeStateReadOnlyList StateList { get; }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        new IWriteablePlaceholderNodeState FirstNodeState { get; }
    }

    /// <summary>
    /// Inner for a list of nodes.
    /// </summary>
    /// <typeparam name="TIndex">Type of the index as class.</typeparam>
    internal class WriteableListInner<TIndex> : ReadOnlyListInner<TIndex>, IWriteableListInner<TIndex>, IWriteableListInner
        where TIndex : IWriteableBrowsingListNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableListInner{TIndex}"/> class.
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
        public new WriteablePlaceholderNodeStateReadOnlyList StateList { get { return (WriteablePlaceholderNodeStateReadOnlyList)base.StateList; } }

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
        public virtual void Insert(WriteableInsertNodeOperation operation)
        {
            int InsertionIndex = operation.Index;
            Debug.Assert(InsertionIndex >= 0 && InsertionIndex <= StateList.Count);

            Node ParentNode = Owner.Node;
            Node Node = operation.Node;
            NodeTreeHelperList.InsertIntoList(ParentNode, PropertyName, InsertionIndex, Node);

            IWriteableBrowsingListNodeIndex BrowsingIndex = CreateBrowsingNodeIndex(Node, InsertionIndex);
            IWriteablePlaceholderNodeState ChildState = (IWriteablePlaceholderNodeState)CreateNodeState(BrowsingIndex);

            InsertInStateList(InsertionIndex, ChildState);

            operation.Update(BrowsingIndex, ChildState);

            while (++InsertionIndex < StateList.Count)
            {
                IWriteablePlaceholderNodeState State = (IWriteablePlaceholderNodeState)StateList[InsertionIndex];

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
        public virtual void Remove(WriteableRemoveNodeOperation operation)
        {
            int RemoveIndex = operation.Index;
            Debug.Assert(RemoveIndex >= 0 && RemoveIndex < StateList.Count);

            IWriteablePlaceholderNodeState OldChildState = (IWriteablePlaceholderNodeState)StateList[RemoveIndex];
            RemoveFromStateList(RemoveIndex);

            Node ParentNode = Owner.Node;
            NodeTreeHelperList.RemoveFromList(ParentNode, PropertyName, RemoveIndex);

            while (RemoveIndex < StateList.Count)
            {
                IWriteablePlaceholderNodeState State = (IWriteablePlaceholderNodeState)StateList[RemoveIndex];

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
            int Index = operation.Index;
            Debug.Assert(Index >= 0 && Index < StateList.Count);

            Node ParentNode = Owner.Node;

            IWriteableNodeState OldChildState = (IWriteableNodeState)StateList[Index];
            Node OldNode = OldChildState.Node;
            IWriteableBrowsingListNodeIndex OldBrowsingIndex = (IWriteableBrowsingListNodeIndex)OldChildState.ParentIndex;
            RemoveFromStateList(Index);

            NodeTreeHelperList.ReplaceNode(ParentNode, PropertyName, Index, operation.NewNode);

            IWriteableBrowsingListNodeIndex NewBrowsingIndex = CreateBrowsingNodeIndex(operation.NewNode, Index);
            IWriteablePlaceholderNodeState NewChildState = (IWriteablePlaceholderNodeState)CreateNodeState(NewBrowsingIndex);
            InsertInStateList(Index, NewChildState);

            operation.Update(OldBrowsingIndex, NewBrowsingIndex, OldNode, NewChildState);
        }

        /// <summary>
        /// Checks whether a node can be moved in a list.
        /// </summary>
        /// <param name="nodeIndex">Index of the node that would be moved.</param>
        /// <param name="direction">Direction of the move, relative to the current position of the item.</param>
        public virtual bool IsMoveable(IWriteableBrowsingCollectionNodeIndex nodeIndex, int direction)
        {
            bool IsHandled = false;
            bool Result = false;

            if (nodeIndex is IWriteableBrowsingListNodeIndex AsListIndex)
            {
                int NewPosition = AsListIndex.Index + direction;
                Result = NewPosition >= 0 && NewPosition < StateList.Count;

                IsHandled = true;
            }

            Debug.Assert(IsHandled);
            return Result;
        }

        /// <summary>
        /// Moves a node around in a list or block list. In a block list, the node stays in same block.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void Move(WriteableMoveNodeOperation operation)
        {
            int MoveIndex = operation.Index;
            int Direction = operation.Direction;
            Debug.Assert(MoveIndex >= 0 && MoveIndex < StateList.Count);
            Debug.Assert(MoveIndex + operation.Direction >= 0 && MoveIndex + operation.Direction < StateList.Count);

            IWriteableBrowsingListNodeIndex MovedNodeIndex = StateList[MoveIndex].ParentIndex as IWriteableBrowsingListNodeIndex;
            Debug.Assert(MovedNodeIndex != null);

            Node ParentNode = Owner.Node;

            MoveInStateList(MoveIndex, Direction);
            NodeTreeHelperList.MoveNode(ParentNode, PropertyName, MoveIndex, Direction);

            operation.Update((IWriteablePlaceholderNodeState)StateList[MoveIndex + Direction]);

            if (Direction > 0)
            {
                for (int i = MoveIndex; i < MoveIndex + Direction; i++)
                {
                    IWriteableBrowsingListNodeIndex ChildNodeIndex = StateList[i].ParentIndex as IWriteableBrowsingListNodeIndex;
                    Debug.Assert(ChildNodeIndex != null);

                    ChildNodeIndex.MoveDown();
                    MovedNodeIndex.MoveUp();
                }
            }
            else if (Direction < 0)
            {
                for (int i = MoveIndex; i > MoveIndex + Direction; i--)
                {
                    IWriteableBrowsingListNodeIndex ChildNodeIndex = StateList[i].ParentIndex as IWriteableBrowsingListNodeIndex;
                    Debug.Assert(ChildNodeIndex != null);

                    ChildNodeIndex.MoveUp();
                    MovedNodeIndex.MoveDown();
                }
            }
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxPlaceholderNodeStateList object.
        /// </summary>
        private protected override ReadOnlyPlaceholderNodeStateList CreateStateList()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<WriteableListInner<TIndex>>());
            return new WriteablePlaceholderNodeStateList();
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        private protected override IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<WriteableListInner<TIndex>>());
            return new WriteablePlaceholderNodeState<IWriteableInner<IWriteableBrowsingChildIndex>>((IWriteableNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndexList.
        /// </summary>
        private protected override ReadOnlyBrowsingListNodeIndexList CreateListNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<WriteableListInner<TIndex>>());
            return new WriteableBrowsingListNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndex object.
        /// </summary>
        private protected virtual IWriteableBrowsingListNodeIndex CreateBrowsingNodeIndex(Node node, int index)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<WriteableListInner<TIndex>>());
            return new WriteableBrowsingListNodeIndex(Owner.Node, node, PropertyName, index);
        }
        #endregion
    }
}
