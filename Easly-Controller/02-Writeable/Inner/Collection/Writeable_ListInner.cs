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
        /// Inserts a new node in a list or block list.
        /// </summary>
        /// <param name="nodeIndex">Index of the node to insert.</param>
        /// <param name="browsingIndex">Index of the inserted node upon return.</param>
        /// <param name="childState">The inserted node state upon return.</param>
        public virtual void Insert(IWriteableInsertionCollectionNodeIndex nodeIndex, out IWriteableBrowsingCollectionNodeIndex browsingIndex, out IWriteablePlaceholderNodeState childState)
        {
            Debug.Assert(nodeIndex != null);

            if (nodeIndex is IWriteableInsertionListNodeIndex AsListIndex)
                Insert(AsListIndex, out browsingIndex, out childState);
            else
                throw new ArgumentOutOfRangeException(nameof(nodeIndex));
        }

        protected virtual void Insert(IWriteableInsertionListNodeIndex listIndex, out IWriteableBrowsingCollectionNodeIndex browsingIndex, out IWriteablePlaceholderNodeState childState)
        {
            Debug.Assert(listIndex != null);
            Debug.Assert(listIndex.Index >= 0 && listIndex.Index <= StateList.Count);

            int InsertionIndex = listIndex.Index;
            INode ParentNode = Owner.Node;
            NodeTreeHelperList.InsertIntoList(ParentNode, PropertyName, InsertionIndex, listIndex.Node);

            IWriteableBrowsingListNodeIndex BrowsingListIndex = (IWriteableBrowsingListNodeIndex)listIndex.ToBrowsingIndex();
            browsingIndex = BrowsingListIndex;

            childState = (IWriteablePlaceholderNodeState)CreateNodeState(BrowsingListIndex);
            InsertInStateList(InsertionIndex, childState);

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
        /// Removes a node from a list.
        /// </summary>
        /// <param name="nodeIndex">Index of the node to remove.</param>
        public virtual void Remove(IWriteableBrowsingCollectionNodeIndex nodeIndex)
        {
            Debug.Assert(nodeIndex != null);

            if (nodeIndex is IWriteableBrowsingListNodeIndex AsListIndex)
                Remove(AsListIndex);
            else
                throw new ArgumentOutOfRangeException(nameof(nodeIndex));
        }

        protected virtual void Remove(IWriteableBrowsingListNodeIndex listIndex)
        {
            Debug.Assert(listIndex != null);
            Debug.Assert(listIndex.Index >= 0 && listIndex.Index < StateList.Count);

            int RemoveIndex = listIndex.Index;
            INode ParentNode = Owner.Node;

            IWriteableNodeState OldChildState = StateList[RemoveIndex];
            RemoveFromStateList(RemoveIndex);

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
        }

        /// <summary>
        /// Replaces a node.
        /// </summary>
        /// <param name="nodeIndex">Index of the node to insert.</param>
        /// <param name="oldBrowsingIndex">Index of the replaced node upon return.</param>
        /// <param name="newBrowsingIndex">Index of the inserted node upon return.</param>
        /// <param name="childState">State of the inserted node upon return.</param>
        public virtual void Replace(IWriteableInsertionChildIndex nodeIndex, out IWriteableBrowsingChildIndex oldBrowsingIndex, out IWriteableBrowsingChildIndex newBrowsingIndex, out IWriteableNodeState childState)
        {
            Debug.Assert(nodeIndex != null);

            if (nodeIndex is IWriteableInsertionListNodeIndex AsListIndex)
                Replace(AsListIndex, out oldBrowsingIndex, out newBrowsingIndex, out childState);
            else
                throw new ArgumentOutOfRangeException(nameof(nodeIndex));
        }

        protected virtual void Replace(IWriteableInsertionListNodeIndex listIndex, out IWriteableBrowsingChildIndex oldBrowsingIndex, out IWriteableBrowsingChildIndex newBrowsingIndex, out IWriteableNodeState childState)
        {
            Debug.Assert(listIndex != null);
            Debug.Assert(listIndex.Index >= 0 && listIndex.Index < StateList.Count);

            INode ParentNode = Owner.Node;

            IWriteableNodeState OldChildState = StateList[listIndex.Index];
            oldBrowsingIndex = (IWriteableBrowsingChildIndex)OldChildState.ParentIndex;
            RemoveFromStateList(listIndex.Index);

            NodeTreeHelperList.ReplaceNode(ParentNode, PropertyName, listIndex.Index, listIndex.Node);

            IWriteableBrowsingListNodeIndex BrowsingListIndex = (IWriteableBrowsingListNodeIndex)listIndex.ToBrowsingIndex();
            newBrowsingIndex = BrowsingListIndex;

            IWriteablePlaceholderNodeState NewChildState = (IWriteablePlaceholderNodeState)CreateNodeState(BrowsingListIndex);
            InsertInStateList(listIndex.Index, NewChildState);

            childState = NewChildState;
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
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        protected override IIndex CreateNodeIndex(IReadOnlyPlaceholderNodeState state, string propertyName, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableListInner<IIndex, TIndex>));
            return (TIndex)new WriteableBrowsingListNodeIndex(Owner.Node, state.Node, propertyName, index);
        }
        #endregion
    }
}
