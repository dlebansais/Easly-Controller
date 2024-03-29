﻿namespace EaslyController.ReadOnly
{
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using NotNullReflection;

    /// <summary>
    /// Inner for a list of nodes.
    /// </summary>
    public interface IReadOnlyListInner : IReadOnlyCollectionInner
    {
        /// <summary>
        /// States of nodes in the list.
        /// </summary>
        ReadOnlyPlaceholderNodeStateReadOnlyList StateList { get; }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        IReadOnlyPlaceholderNodeState FirstNodeState { get; }

        /// <summary>
        /// Gets the index of the node at the given position.
        /// </summary>
        /// <param name="index">Position of the node in the list.</param>
        /// <returns>The index of the node at position <paramref name="index"/>.</returns>
        IReadOnlyBrowsingListNodeIndex IndexAt(int index);

        /// <summary>
        /// Gets indexes for all nodes in the inner.
        /// </summary>
        ReadOnlyBrowsingListNodeIndexList AllIndexes();
    }

    /// <summary>
    /// Inner for a list of nodes.
    /// </summary>
    /// <typeparam name="TIndex">Type of the index.</typeparam>
    internal interface IReadOnlyListInner<out TIndex> : IReadOnlyCollectionInner<TIndex>
        where TIndex : IReadOnlyBrowsingListNodeIndex
    {
        /// <summary>
        /// States of nodes in the list.
        /// </summary>
        ReadOnlyPlaceholderNodeStateReadOnlyList StateList { get; }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        IReadOnlyPlaceholderNodeState FirstNodeState { get; }

        /// <summary>
        /// Gets the index of the node at the given position.
        /// </summary>
        /// <param name="index">Position of the node in the list.</param>
        /// <returns>The index of the node at position <paramref name="index"/>.</returns>
        IReadOnlyBrowsingListNodeIndex IndexAt(int index);

        /// <summary>
        /// Gets indexes for all nodes in the inner.
        /// </summary>
        ReadOnlyBrowsingListNodeIndexList AllIndexes();
    }

    /// <inheritdoc/>
    internal class ReadOnlyListInner<TIndex> : ReadOnlyCollectionInner<TIndex>, IReadOnlyListInner<TIndex>, IReadOnlyListInner, IReadOnlyCollectionInner
        where TIndex : IReadOnlyBrowsingListNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyListInner{TIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public ReadOnlyListInner(IReadOnlyNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
            _StateList = CreateStateList();
            StateList = _StateList.ToReadOnly();
        }

        /// <inheritdoc/>
        public override IReadOnlyNodeState InitChildState(IReadOnlyBrowsingChildIndex nodeIndex)
        {
            Debug.Assert(nodeIndex is IReadOnlyBrowsingListNodeIndex);
            return InitChildState((IReadOnlyBrowsingListNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Initializes a newly created state for a node in the inner.
        /// </summary>
        /// <param name="nodeIndex">Index of the node.</param>
        /// <returns>The created node state.</returns>
        private protected virtual IReadOnlyNodeState InitChildState(IReadOnlyBrowsingListNodeIndex nodeIndex)
        {
            Debug.Assert(nodeIndex.PropertyName == PropertyName);

            int Index = ((TIndex)nodeIndex).Index;
            Debug.Assert(Index == StateList.Count);

            IReadOnlyPlaceholderNodeState State = CreateNodeState(nodeIndex);
            InsertInStateList(Index, State);

            return State;
        }
        #endregion

        #region Properties
        /// <inheritdoc/>
        public override bool IsNeverEmpty { get { return NodeHelper.IsCollectionNeverEmpty(Owner.Node, PropertyName); } }

        /// <inheritdoc/>
        public override Type InterfaceType { get { return NodeTreeHelperList.ListItemType(Owner.Node, PropertyName); } }

        /// <summary>
        /// States of nodes in the list.
        /// </summary>
        public ReadOnlyPlaceholderNodeStateReadOnlyList StateList { get; }
        private ReadOnlyPlaceholderNodeStateList _StateList;
#pragma warning disable 1591
        [Conditional("DEBUG")]
        public void DebugGetStateList() { DebugObjects.AddReference(_StateList); }
#pragma warning restore 1591

        /// <inheritdoc/>
        public override int Count
        {
            get { return StateList.Count; }
        }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        public virtual IReadOnlyPlaceholderNodeState FirstNodeState
        {
            get
            {
                Debug.Assert(Count > 0);

                DebugGetStateList();
                return StateList[0];
            }
        }
        #endregion

        #region Client Interface
        /// <summary>
        /// Gets the index of the node at the given position.
        /// </summary>
        /// <param name="index">Position of the node in the list.</param>
        /// <returns>The index of the node at position <paramref name="index"/>.</returns>
        public virtual IReadOnlyBrowsingListNodeIndex IndexAt(int index)
        {
            Debug.Assert(index >= 0 && index < StateList.Count);

            return (IReadOnlyBrowsingListNodeIndex)StateList[index].ParentIndex;
        }

        /// <summary>
        /// Gets indexes for all nodes in the inner.
        /// </summary>
        public virtual ReadOnlyBrowsingListNodeIndexList AllIndexes()
        {
            ReadOnlyBrowsingListNodeIndexList Result = CreateListNodeIndexList();

            foreach (IReadOnlyPlaceholderNodeState NodeState in StateList)
            {
                IReadOnlyBrowsingListNodeIndex ParentIndex = NodeState.ParentIndex as IReadOnlyBrowsingListNodeIndex;
                Debug.Assert(ParentIndex != null);

                Result.Add(ParentIndex);
            }

            return Result;
        }

        /// <inheritdoc/>
        public override void CloneChildren(Node parentNode)
        {
            NodeTreeHelperList.ClearChildNodeList(parentNode, PropertyName);

            for (int i = 0; i < StateList.Count; i++)
            {
                IReadOnlyPlaceholderNodeState ChildState = StateList[i];

                // Clone all children recursively.
                Node ChildNodeClone = ChildState.CloneNode();
                Debug.Assert(ChildNodeClone != null);

                NodeTreeHelperList.InsertIntoList(parentNode, PropertyName, i, ChildNodeClone);
            }
        }

        /// <inheritdoc/>
        public override void Attach(ReadOnlyControllerView view, ReadOnlyAttachCallbackSet callbackSet)
        {
            foreach (IReadOnlyNodeState ChildState in StateList)
                ((IReadOnlyNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>)ChildState).Attach(view, callbackSet);
        }

        /// <inheritdoc/>
        public override void Detach(ReadOnlyControllerView view, ReadOnlyAttachCallbackSet callbackSet)
        {
            foreach (IReadOnlyNodeState ChildState in StateList)
                ((IReadOnlyNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>)ChildState).Detach(view, callbackSet);
        }
        #endregion

        #region Implementation
        private protected virtual void InsertInStateList(int index, IReadOnlyPlaceholderNodeState nodeState)
        {
            Debug.Assert(index >= 0 && index <= _StateList.Count);
            Debug.Assert(nodeState.ParentIndex is IReadOnlyBrowsingListNodeIndex);

            _StateList.Insert(index, nodeState);
        }

        private protected virtual void RemoveFromStateList(int index)
        {
            Debug.Assert(index >= 0 && index < _StateList.Count);

            _StateList.RemoveAt(index);
        }

        private protected virtual void MoveInStateList(int index, int direction)
        {
            Debug.Assert(index >= 0 && index < _StateList.Count);
            Debug.Assert(index + direction >= 0 && index + direction < _StateList.Count);

            IReadOnlyPlaceholderNodeState State = _StateList[index];
            _StateList.RemoveAt(index);
            _StateList.Insert(index + direction, State);
        }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            if (!comparer.IsSameType(other, out ReadOnlyListInner<TIndex> AsListInner))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsListInner))
                return comparer.Failed();

            if (!comparer.IsSameCount(StateList.Count, AsListInner.StateList.Count))
                return comparer.Failed();

            for (int i = 0; i < StateList.Count; i++)
                if (!comparer.VerifyEqual(StateList[i], AsListInner.StateList[i]))
                    return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <inheritdoc/>
        private protected virtual ReadOnlyPlaceholderNodeStateList CreateStateList()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<ReadOnlyListInner<TIndex>>());
            return new ReadOnlyPlaceholderNodeStateList();
        }

        /// <inheritdoc/>
        private protected virtual IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<ReadOnlyListInner<TIndex>>());
            return new ReadOnlyPlaceholderNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>(nodeIndex);
        }

        /// <inheritdoc/>
        private protected virtual ReadOnlyBrowsingListNodeIndexList CreateListNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<ReadOnlyListInner<TIndex>>());
            return new ReadOnlyBrowsingListNodeIndexList();
        }
        #endregion
    }
}
