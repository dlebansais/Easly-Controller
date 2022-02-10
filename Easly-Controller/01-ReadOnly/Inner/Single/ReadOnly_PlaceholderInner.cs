namespace EaslyController.ReadOnly
{
    using System;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;

    /// <summary>
    /// Inner for a child node.
    /// </summary>
    public interface IReadOnlyPlaceholderInner : IReadOnlySingleInner
    {
        /// <summary>
        /// The state of the node.
        /// </summary>
        IReadOnlyPlaceholderNodeState ChildState { get; }
    }

    /// <summary>
    /// Inner for a child node.
    /// </summary>
    /// <typeparam name="TIndex">Type of the index.</typeparam>
    internal interface IReadOnlyPlaceholderInner<out TIndex> : IReadOnlySingleInner<TIndex>
        where TIndex : IReadOnlyBrowsingPlaceholderNodeIndex
    {
        /// <summary>
        /// The state of the node.
        /// </summary>
        IReadOnlyPlaceholderNodeState ChildState { get; }
    }

    /// <inheritdoc/>
    internal class ReadOnlyPlaceholderInner<TIndex> : ReadOnlySingleInner<TIndex>, IReadOnlyPlaceholderInner<TIndex>, IReadOnlyPlaceholderInner, IReadOnlySingleInner
        where TIndex : IReadOnlyBrowsingPlaceholderNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyPlaceholderInner{TIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public ReadOnlyPlaceholderInner(IReadOnlyNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
            ChildState = null;
        }

        /// <inheritdoc/>
        public override IReadOnlyNodeState InitChildState(IReadOnlyBrowsingChildIndex nodeIndex)
        {
            Debug.Assert(nodeIndex is IReadOnlyBrowsingPlaceholderNodeIndex);
            return InitChildState((IReadOnlyBrowsingPlaceholderNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Initializes a newly created state for the node in the inner.
        /// </summary>
        /// <param name="nodeIndex">Index of the node.</param>
        /// <returns>The created node state.</returns>
        private protected virtual IReadOnlyPlaceholderNodeState InitChildState(IReadOnlyBrowsingPlaceholderNodeIndex nodeIndex)
        {
            Debug.Assert(nodeIndex.PropertyName == PropertyName);
            Debug.Assert(ChildState == null);

            IReadOnlyPlaceholderNodeState State = CreateNodeState(nodeIndex);
            SetChildState(State);

            return State;
        }
        #endregion

        #region Properties
        /// <inheritdoc/>
        public override Type InterfaceType { get { return NodeTreeHelperChild.ChildNodeType(Owner.Node, PropertyName); } }

        /// <summary>
        /// The state of the child node.
        /// </summary>
        public IReadOnlyPlaceholderNodeState ChildState { get; private set; }
        #endregion

        #region Client Interface
        /// <inheritdoc/>
        public override void CloneChildren(Node parentNode)
        {
            // Clone the child recursively.
            Node ChildNodeClone = ChildState.CloneNode();
            Debug.Assert(ChildNodeClone != null);

            // Set the clone in the parent.
            NodeTreeHelperChild.SetChildNode(parentNode, PropertyName, ChildNodeClone);
        }

        /// <inheritdoc/>
        public override void Attach(ReadOnlyControllerView view, ReadOnlyAttachCallbackSet callbackSet)
        {
            ((IReadOnlyNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>)ChildState).Attach(view, callbackSet);
        }

        /// <inheritdoc/>
        public override void Detach(ReadOnlyControllerView view, ReadOnlyAttachCallbackSet callbackSet)
        {
            ((IReadOnlyNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>)ChildState).Detach(view, callbackSet);
        }
        #endregion

        #region Implementation
        private protected virtual void SetChildState(IReadOnlyPlaceholderNodeState childState)
        {
            ChildState = childState;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        private protected virtual IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyBrowsingPlaceholderNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyPlaceholderInner<TIndex>));
            return new ReadOnlyPlaceholderNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>(nodeIndex);
        }
        #endregion
    }
}
