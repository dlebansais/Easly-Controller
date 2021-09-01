namespace EaslyController.ReadOnly
{
    using System;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;

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
        ReadOnlyBrowsingListNodeIndex IndexAt(int index);

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
        where TIndex : ReadOnlyBrowsingListNodeIndex
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
        ReadOnlyBrowsingListNodeIndex IndexAt(int index);

        /// <summary>
        /// Gets indexes for all nodes in the inner.
        /// </summary>
        ReadOnlyBrowsingListNodeIndexList AllIndexes();
    }

    /// <summary>
    /// Inner for a list of nodes.
    /// </summary>
    /// <typeparam name="TIndex">Type of the index as class.</typeparam>
    internal class ReadOnlyListInner<TIndex> : ReadOnlyCollectionInner<TIndex>, IReadOnlyListInner<TIndex>, IReadOnlyListInner, IReadOnlyCollectionInner
        where TIndex : ReadOnlyBrowsingListNodeIndex
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

        /// <summary>
        /// Initializes a newly created state for a node in the inner.
        /// </summary>
        /// <param name="nodeIndex">Index of the node.</param>
        /// <returns>The created node state.</returns>
        public override IReadOnlyNodeState InitChildState(IReadOnlyBrowsingChildIndex nodeIndex)
        {
            Debug.Assert(nodeIndex is ReadOnlyBrowsingListNodeIndex);
            return InitChildState((ReadOnlyBrowsingListNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Initializes a newly created state for a node in the inner.
        /// </summary>
        /// <param name="nodeIndex">Index of the node.</param>
        /// <returns>The created node state.</returns>
        private protected virtual IReadOnlyNodeState InitChildState(ReadOnlyBrowsingListNodeIndex nodeIndex)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(nodeIndex.PropertyName == PropertyName);

            int Index = ((TIndex)nodeIndex).Index;
            Debug.Assert(Index == StateList.Count);

            IReadOnlyPlaceholderNodeState State = CreateNodeState(nodeIndex);
            InsertInStateList(Index, State);

            return State;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Checks if the inner must have at list one item.
        /// </summary>
        public override bool IsNeverEmpty { get { return NodeHelper.IsCollectionNeverEmpty(Owner.Node, PropertyName); } }

        /// <summary>
        /// Interface type for all nodes in the inner.
        /// </summary>
        public override Type InterfaceType { get { return NodeTreeHelperList.ListInterfaceType(Owner.Node, PropertyName); } }

        /// <summary>
        /// States of nodes in the list.
        /// </summary>
        public ReadOnlyPlaceholderNodeStateReadOnlyList StateList { get; }
        private ReadOnlyPlaceholderNodeStateList _StateList;
#pragma warning disable 1591
        [Conditional("DEBUG")]
        public void DebugGetStateList() { DebugObjects.AddReference(_StateList); }
#pragma warning restore 1591

        /// <summary>
        /// Count of all node states in the inner.
        /// </summary>
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
        public virtual ReadOnlyBrowsingListNodeIndex IndexAt(int index)
        {
            Debug.Assert(index >= 0 && index < StateList.Count);

            return (ReadOnlyBrowsingListNodeIndex)StateList[index].ParentIndex;
        }

        /// <summary>
        /// Gets indexes for all nodes in the inner.
        /// </summary>
        public virtual ReadOnlyBrowsingListNodeIndexList AllIndexes()
        {
            ReadOnlyBrowsingListNodeIndexList Result = CreateListNodeIndexList();

            foreach (IReadOnlyPlaceholderNodeState NodeState in StateList)
            {
                ReadOnlyBrowsingListNodeIndex ParentIndex = NodeState.ParentIndex as ReadOnlyBrowsingListNodeIndex;
                Debug.Assert(ParentIndex != null);

                Result.Add(ParentIndex);
            }

            return Result;
        }

        /// <summary>
        /// Creates a clone of all children of the inner, using <paramref name="parentNode"/> as their parent.
        /// </summary>
        /// <param name="parentNode">The node that will contains references to cloned children upon return.</param>
        public override void CloneChildren(Node parentNode)
        {
            Debug.Assert(parentNode != null);

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

        /// <summary>
        /// Attach a view to the inner.
        /// </summary>
        /// <param name="view">The attaching view.</param>
        /// <param name="callbackSet">The set of callbacks to call when enumerating existing states.</param>
        public override void Attach(ReadOnlyControllerView view, ReadOnlyAttachCallbackSet callbackSet)
        {
            foreach (IReadOnlyNodeState ChildState in StateList)
                ((ReadOnlyNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>)ChildState).Attach(view, callbackSet);
        }

        /// <summary>
        /// Detach a view from the inner.
        /// </summary>
        /// <param name="view">The attaching view.</param>
        /// <param name="callbackSet">The set of callbacks to no longer call when enumerating existing states.</param>
        public override void Detach(ReadOnlyControllerView view, ReadOnlyAttachCallbackSet callbackSet)
        {
            foreach (IReadOnlyNodeState ChildState in StateList)
                ((ReadOnlyNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>)ChildState).Detach(view, callbackSet);
        }
        #endregion

        #region Implementation
        private protected virtual void InsertInStateList(int index, IReadOnlyPlaceholderNodeState nodeState)
        {
            Debug.Assert(index >= 0 && index <= _StateList.Count);
            Debug.Assert(nodeState != null);
            Debug.Assert(nodeState.ParentIndex is ReadOnlyBrowsingListNodeIndex);

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
        /// <summary>
        /// Compares two <see cref="ReadOnlyListInner{TIndex}"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

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
        /// <summary>
        /// Creates a IxxxPlaceholderNodeStateList object.
        /// </summary>
        private protected virtual ReadOnlyPlaceholderNodeStateList CreateStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyListInner<TIndex>));
            return new ReadOnlyPlaceholderNodeStateList();
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        private protected virtual IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyListInner<TIndex>));
            return new ReadOnlyPlaceholderNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>(nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndexList.
        /// </summary>
        private protected virtual ReadOnlyBrowsingListNodeIndexList CreateListNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyListInner<TIndex>));
            return new ReadOnlyBrowsingListNodeIndexList();
        }
        #endregion
    }
}
