namespace EaslyController.ReadOnly
{
    using System;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;

    /// <summary>
    /// Inner for an optional node.
    /// </summary>
    public interface IReadOnlyOptionalInner : IReadOnlySingleInner
    {
        /// <summary>
        /// The state of the node.
        /// </summary>
        IReadOnlyOptionalNodeState ChildState { get; }

        /// <summary>
        /// True if the optional node is provided.
        /// </summary>
        bool IsAssigned { get; }
    }

    /// <summary>
    /// Inner for an optional node.
    /// </summary>
    /// <typeparam name="TIndex">Type of the index.</typeparam>
    internal interface IReadOnlyOptionalInner<out TIndex> : IReadOnlySingleInner<TIndex>
        where TIndex : IReadOnlyBrowsingOptionalNodeIndex
    {
        /// <summary>
        /// The state of the node.
        /// </summary>
        IReadOnlyOptionalNodeState ChildState { get; }

        /// <summary>
        /// True if the optional node is provided.
        /// </summary>
        bool IsAssigned { get; }
    }

    /// <inheritdoc/>
    internal class ReadOnlyOptionalInner<TIndex> : ReadOnlySingleInner<TIndex>, IReadOnlyOptionalInner<TIndex>, IReadOnlyOptionalInner, IReadOnlySingleInner
        where TIndex : IReadOnlyBrowsingOptionalNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyOptionalInner{TIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public ReadOnlyOptionalInner(IReadOnlyNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
            ChildState = null;
        }

        /// <inheritdoc/>
        public override IReadOnlyNodeState InitChildState(IReadOnlyBrowsingChildIndex nodeIndex)
        {
            Debug.Assert(nodeIndex is IReadOnlyBrowsingOptionalNodeIndex);
            return InitChildState((IReadOnlyBrowsingOptionalNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Initializes a newly created state for the node in the inner, provided or not.
        /// </summary>
        /// <param name="nodeIndex">Index of the node.</param>
        /// <returns>The created node state.</returns>
        private protected virtual IReadOnlyOptionalNodeState InitChildState(IReadOnlyBrowsingOptionalNodeIndex nodeIndex)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(nodeIndex.PropertyName == PropertyName);
            Debug.Assert(ChildState == null);

            IReadOnlyOptionalNodeState State = CreateNodeState(nodeIndex);
            SetChildState(State);

            return State;
        }
        #endregion

        #region Properties
        /// <inheritdoc/>
        public override Type InterfaceType { get { return NodeTreeHelperOptional.OptionalChildInterfaceType(Owner.Node, PropertyName); } }

        /// <summary>
        /// True if the optional node is provided.
        /// </summary>
        public bool IsAssigned { get { return NodeTreeHelperOptional.IsChildNodeAssigned(Owner.Node, PropertyName); } }

        /// <summary>
        /// The state of the optional node.
        /// </summary>
        public IReadOnlyOptionalNodeState ChildState { get; private set; }
        #endregion

        #region Client Interface
        /// <inheritdoc/>
        public override void CloneChildren(Node parentNode)
        {
            Debug.Assert(parentNode != null);

            // Clone the child recursively.
            Node ChildNodeClone = ChildState.CloneNode();

            // If the original is set, set the clone too.
            if (ChildNodeClone != null)
            {
                NodeTreeHelperOptional.SetOptionalChildNode(parentNode, PropertyName, ChildNodeClone);
                if (!IsAssigned)
                    NodeTreeHelperOptional.UnassignChildNode(parentNode, PropertyName);
            }
        }

        /// <inheritdoc/>
        public override void Attach(ReadOnlyControllerView view, ReadOnlyAttachCallbackSet callbackSet)
        {
            ((IReadOnlyOptionalNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>)ChildState).Attach(view, callbackSet);
        }

        /// <inheritdoc/>
        public override void Detach(ReadOnlyControllerView view, ReadOnlyAttachCallbackSet callbackSet)
        {
            ((IReadOnlyOptionalNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>)ChildState).Detach(view, callbackSet);
        }
        #endregion

        #region Implementation
        private protected virtual void SetChildState(IReadOnlyOptionalNodeState childState)
        {
            Debug.Assert(childState != null);

            ChildState = childState;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxOptionalNodeState object.
        /// </summary>
        private protected virtual IReadOnlyOptionalNodeState CreateNodeState(IReadOnlyBrowsingOptionalNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyOptionalInner<TIndex>));
            return new ReadOnlyOptionalNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>(nodeIndex);
        }
        #endregion
    }
}
