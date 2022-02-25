namespace EaslyController.ReadOnly
{
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using NotNullReflection;

    /// <summary>
    /// Inner for a child node.
    /// </summary>
    public interface IReadOnlyEmptyInner : IReadOnlySingleInner
    {
        /// <summary>
        /// The state of the node.
        /// </summary>
        IReadOnlyEmptyNodeState ChildState { get; }
    }

    /// <summary>
    /// Inner for a child node.
    /// </summary>
    /// <typeparam name="TIndex">Type of the index.</typeparam>
    internal interface IReadOnlyEmptyInner<out TIndex> : IReadOnlySingleInner<TIndex>
        where TIndex : IReadOnlyBrowsingChildIndex
    {
        /// <summary>
        /// The state of the node.
        /// </summary>
        IReadOnlyEmptyNodeState ChildState { get; }
    }

    /// <inheritdoc/>
    internal class ReadOnlyEmptyInner<TIndex> : ReadOnlySingleInner<TIndex>, IReadOnlyEmptyInner<TIndex>, IReadOnlyEmptyInner, IReadOnlySingleInner
        where TIndex : IReadOnlyBrowsingChildIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyEmptyInner{TIndex}"/> class.
        /// </summary>
        public ReadOnlyEmptyInner()
            : base()
        {
            ChildState = (IReadOnlyEmptyNodeState)ReadOnlyEmptyNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>.Empty;
        }

        /// <inheritdoc/>
        public override IReadOnlyNodeState InitChildState(IReadOnlyBrowsingChildIndex nodeIndex)
        {
            return InitChildState(nodeIndex);
        }
        #endregion

        #region Properties
        /// <inheritdoc/>
        public override Type InterfaceType { get { return NodeTreeHelperChild.ChildNodeType(Owner.Node, PropertyName); } }

        /// <summary>
        /// The state of the child node.
        /// </summary>
        public IReadOnlyEmptyNodeState ChildState { get; private set; }
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
        private protected virtual void SetChildState(IReadOnlyEmptyNodeState childState)
        {
            ChildState = childState;
        }
        #endregion
    }
}
