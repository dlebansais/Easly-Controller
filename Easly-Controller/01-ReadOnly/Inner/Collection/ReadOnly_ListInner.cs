using BaseNode;
using BaseNodeHelper;
using System;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// Inner for a list of nodes.
    /// </summary>
    public interface IReadOnlyListInner : IReadOnlyCollectionInner
    {
        /// <summary>
        /// States of nodes in the list.
        /// </summary>
        IReadOnlyPlaceholderNodeStateReadOnlyList StateList { get; }

        /// <summary>
        /// Gets the index of the node at the given position.
        /// </summary>
        /// <param name="index">Position of the node in the list.</param>
        /// <returns>The index of the node at position <paramref name="index"/>.</returns>
        IReadOnlyBrowsingListNodeIndex IndexAt(int index);
    }

    /// <summary>
    /// Inner for a list of nodes.
    /// </summary>
    public interface IReadOnlyListInner<out IIndex> : IReadOnlyCollectionInner<IIndex>
        where IIndex : IReadOnlyBrowsingListNodeIndex
    {
        /// <summary>
        /// States of nodes in the list.
        /// </summary>
        IReadOnlyPlaceholderNodeStateReadOnlyList StateList { get; }

        /// <summary>
        /// Gets the index of the node at the given position.
        /// </summary>
        /// <param name="index">Position of the node in the list.</param>
        /// <returns>The index of the node at position <paramref name="index"/>.</returns>
        IReadOnlyBrowsingListNodeIndex IndexAt(int index);
    }

    /// <summary>
    /// Inner for a list of nodes.
    /// </summary>
    public class ReadOnlyListInner<IIndex, TIndex> : ReadOnlyCollectionInner<IIndex, TIndex>, IReadOnlyListInner<IIndex>, IReadOnlyListInner
        where IIndex : IReadOnlyBrowsingListNodeIndex
        where TIndex : ReadOnlyBrowsingListNodeIndex, IIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyListInner{IIndex, TIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public ReadOnlyListInner(IReadOnlyNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
            _StateList = CreateStateList();
            StateList = CreateStateListReadOnly(_StateList);
        }

        /// <summary>
        /// Initializes a newly created state for a node in the inner.
        /// </summary>
        /// <param name="nodeIndex">Index of the node.</param>
        /// <returns>The created node state.</returns>
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
        protected virtual IReadOnlyNodeState InitChildState(IReadOnlyBrowsingListNodeIndex nodeIndex)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(nodeIndex.PropertyName == PropertyName);

            int Index = ((IIndex)nodeIndex).Index;
            Debug.Assert(Index == StateList.Count);

            IReadOnlyPlaceholderNodeState State = CreateNodeState(nodeIndex);
            InsertInStateList(Index, State);

            return State;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Interface type for all nodes in the inner.
        /// </summary>
        public override Type InterfaceType { get { return NodeTreeHelperList.ListInterfaceType(Owner.Node, PropertyName); } }

        /// <summary>
        /// States of nodes in the list.
        /// </summary>
        public IReadOnlyPlaceholderNodeStateReadOnlyList StateList { get; }
        private IReadOnlyPlaceholderNodeStateList _StateList;

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
        public override IReadOnlyPlaceholderNodeState FirstNodeState
        {
            get
            {
                Debug.Assert(Count > 0);

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
        /// Creates a clone of all children of the inner, using <paramref name="parentNode"/> as their parent.
        /// </summary>
        /// <param name="parentNode">The node that will contains references to cloned children upon return.</param>
        public override void CloneChildren(INode parentNode)
        {
            Debug.Assert(parentNode != null);

            for (int i = 0; i < StateList.Count; i++)
            {
                IReadOnlyPlaceholderNodeState ChildState = StateList[i];

                // Clone all children recursively.
                INode ChildNodeClone = ChildState.CloneNode();
                Debug.Assert(ChildNodeClone != null);

                NodeTreeHelperList.InsertIntoList(parentNode, PropertyName, i, ChildNodeClone);
            }
        }

        /// <summary>
        /// Attach a view to the inner.
        /// </summary>
        /// <param name="view">The attaching view.</param>
        /// <param name="callbackSet">The set of callbacks to call when enumerating existing states.</param>
        public override void Attach(IReadOnlyControllerView view, IReadOnlyAttachCallbackSet callbackSet)
        {
            foreach (IReadOnlyNodeState ChildState in StateList)
                ChildState.Attach(view, callbackSet);
        }
        #endregion

        #region Implementation
        protected virtual void InsertInStateList(int index, IReadOnlyPlaceholderNodeState nodeState)
        {
            Debug.Assert(index >= 0 && index <= _StateList.Count);
            Debug.Assert(nodeState != null);
            Debug.Assert(nodeState.ParentIndex is IReadOnlyBrowsingListNodeIndex);

            _StateList.Insert(index, nodeState);
        }

        protected virtual void RemoveFromStateList(int index)
        {
            Debug.Assert(index >= 0 && index < _StateList.Count);

            _StateList.RemoveAt(index);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxPlaceholderNodeStateList object.
        /// </summary>
        protected virtual IReadOnlyPlaceholderNodeStateList CreateStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyListInner<IIndex, TIndex>));
            return new ReadOnlyPlaceholderNodeStateList();
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeStateReadOnlyList object.
        /// </summary>
        protected virtual IReadOnlyPlaceholderNodeStateReadOnlyList CreateStateListReadOnly(IReadOnlyPlaceholderNodeStateList stateList)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyListInner<IIndex, TIndex>));
            return new ReadOnlyPlaceholderNodeStateReadOnlyList(stateList);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        protected virtual IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyListInner<IIndex, TIndex>));
            return new ReadOnlyPlaceholderNodeState(nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        protected virtual IIndex CreateNodeIndex(IReadOnlyPlaceholderNodeState state, string propertyName, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyListInner<IIndex, TIndex>));
            return (TIndex)new ReadOnlyBrowsingListNodeIndex(Owner.Node, state.Node, propertyName, index);
        }
        #endregion
    }
}
