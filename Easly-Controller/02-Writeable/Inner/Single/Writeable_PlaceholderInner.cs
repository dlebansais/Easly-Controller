namespace EaslyController.Writeable
{
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Inner for a child node.
    /// </summary>
    public interface IWriteablePlaceholderInner : IReadOnlyPlaceholderInner, IWriteableSingleInner
    {
        /// <summary>
        /// The state of the node.
        /// </summary>
        new IWriteablePlaceholderNodeState ChildState { get; }
    }

    /// <summary>
    /// Inner for a child node.
    /// </summary>
    /// <typeparam name="TIndex">Type of the index.</typeparam>
    internal interface IWriteablePlaceholderInner<out TIndex> : IReadOnlyPlaceholderInner<TIndex>, IWriteableSingleInner<TIndex>
        where TIndex : IWriteableBrowsingPlaceholderNodeIndex
    {
        /// <summary>
        /// The state of the node.
        /// </summary>
        new IWriteablePlaceholderNodeState ChildState { get; }
    }

    /// <summary>
    /// Inner for a child node.
    /// </summary>
    /// <typeparam name="TIndex">Type of the index as class.</typeparam>
    internal class WriteablePlaceholderInner<TIndex> : ReadOnlyPlaceholderInner<TIndex>, IWriteablePlaceholderInner<TIndex>, IWriteablePlaceholderInner
        where TIndex : IWriteableBrowsingPlaceholderNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteablePlaceholderInner{TIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public WriteablePlaceholderInner(IWriteableNodeState owner, string propertyName)
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
        /// The state of the optional node.
        /// </summary>
        public new IWriteablePlaceholderNodeState ChildState { get { return (IWriteablePlaceholderNodeState)base.ChildState; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Replaces a node.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void Replace(IWriteableReplaceOperation operation)
        {
            Node ParentNode = Owner.Node;

            IWriteableBrowsingPlaceholderNodeIndex OldBrowsingIndex = (IWriteableBrowsingPlaceholderNodeIndex)ChildState.ParentIndex;
            IWriteablePlaceholderNodeState OldChildState = (IWriteablePlaceholderNodeState)ChildState;
            Node OldNode = OldChildState.Node;

            NodeTreeHelperChild.SetChildNode(ParentNode, PropertyName, operation.NewNode);

            IWriteableBrowsingPlaceholderNodeIndex NewBrowsingIndex = CreateBrowsingNodeIndex(operation.NewNode);
            IWriteablePlaceholderNodeState NewChildState = (IWriteablePlaceholderNodeState)CreateNodeState(NewBrowsingIndex);
            SetChildState(NewChildState);

            operation.Update(OldBrowsingIndex, NewBrowsingIndex, OldNode, NewChildState);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        private protected override IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyBrowsingPlaceholderNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteablePlaceholderInner<TIndex>));
            return new WriteablePlaceholderNodeState<IWriteableInner<IWriteableBrowsingChildIndex>>((IWriteableBrowsingPlaceholderNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingPlaceholderNodeIndex object.
        /// </summary>
        private protected virtual IWriteableBrowsingPlaceholderNodeIndex CreateBrowsingNodeIndex(Node node)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteablePlaceholderInner<TIndex>));
            return new WriteableBrowsingPlaceholderNodeIndex(Owner.Node, node, PropertyName);
        }
        #endregion
    }
}
