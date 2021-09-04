namespace EaslyController.Writeable
{
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Inner for an optional node.
    /// </summary>
    public interface IWriteableOptionalInner : IReadOnlyOptionalInner, IWriteableSingleInner
    {
        /// <summary>
        /// The state of the optional node.
        /// </summary>
        new IWriteableOptionalNodeState ChildState { get; }

        /// <summary>
        /// Assign the optional node.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        void Assign(WriteableAssignmentOperation operation);

        /// <summary>
        /// Unassign the optional node.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        void Unassign(WriteableAssignmentOperation operation);
    }

    /// <summary>
    /// Inner for an optional node.
    /// </summary>
    /// <typeparam name="TIndex">Type of the index.</typeparam>
    internal interface IWriteableOptionalInner<out TIndex> : IReadOnlyOptionalInner<TIndex>, IWriteableSingleInner<TIndex>
        where TIndex : WriteableBrowsingOptionalNodeIndex
    {
        /// <summary>
        /// The state of the optional node.
        /// </summary>
        new IWriteableOptionalNodeState ChildState { get; }

        /// <summary>
        /// Assign the optional node.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        void Assign(WriteableAssignmentOperation operation);

        /// <summary>
        /// Unassign the optional node.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        void Unassign(WriteableAssignmentOperation operation);
    }

    /// <summary>
    /// Inner for an optional node.
    /// </summary>
    /// <typeparam name="TIndex">Type of the index as class.</typeparam>
    internal class WriteableOptionalInner<TIndex> : ReadOnlyOptionalInner<TIndex>, IWriteableOptionalInner<TIndex>, IWriteableOptionalInner
        where TIndex : WriteableBrowsingOptionalNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableOptionalInner{TIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public WriteableOptionalInner(IWriteableNodeState owner, string propertyName)
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
        public new IWriteableOptionalNodeState ChildState { get { return (IWriteableOptionalNodeState)base.ChildState; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Replaces a node.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void Replace(WriteableReplaceOperation operation)
        {
            Debug.Assert(operation != null);

            if (operation.NewNode != null)
                ReplaceOptional(operation);
            else
                ClearOptional(operation);
        }

        private protected virtual void ReplaceOptional(WriteableReplaceOperation operation)
        {
            Node ParentNode = Owner.Node;

            WriteableBrowsingOptionalNodeIndex OldBrowsingIndex = (WriteableBrowsingOptionalNodeIndex)ChildState.ParentIndex;
            Node OldNode = ChildState.Optional.HasItem ? ChildState.Node : null;

            NodeTreeHelperOptional.SetOptionalChildNode(ParentNode, PropertyName, operation.NewNode);

            WriteableBrowsingOptionalNodeIndex NewBrowsingIndex = CreateBrowsingNodeIndex();
            IWriteableOptionalNodeState NewChildState = (IWriteableOptionalNodeState)CreateNodeState(NewBrowsingIndex);
            SetChildState(NewChildState);

            operation.Update(OldBrowsingIndex, NewBrowsingIndex, OldNode, NewChildState);
        }

        private protected virtual void ClearOptional(WriteableReplaceOperation operation)
        {
            Node ParentNode = Owner.Node;

            WriteableBrowsingOptionalNodeIndex OldBrowsingIndex = (WriteableBrowsingOptionalNodeIndex)ChildState.ParentIndex;
            Node OldNode = ChildState.Optional.HasItem ? ChildState.Node : null;

            NodeTreeHelperOptional.ClearOptionalChildNode(ParentNode, PropertyName);

            WriteableBrowsingOptionalNodeIndex NewBrowsingIndex = CreateBrowsingNodeIndex();
            IWriteableOptionalNodeState NewChildState = (IWriteableOptionalNodeState)CreateNodeState(NewBrowsingIndex);
            SetChildState(NewChildState);

            operation.Update(OldBrowsingIndex, NewBrowsingIndex, OldNode, NewChildState);
        }

        /// <summary>
        /// Assign the optional node.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public void Assign(WriteableAssignmentOperation operation)
        {
            NodeTreeHelperOptional.AssignChildNode(Owner.Node, PropertyName);

            operation.Update(ChildState);
        }

        /// <summary>
        /// Unassign the optional node.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public void Unassign(WriteableAssignmentOperation operation)
        {
            NodeTreeHelperOptional.UnassignChildNode(Owner.Node, PropertyName);

            operation.Update(ChildState);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxOptionalNodeState object.
        /// </summary>
        private protected override IReadOnlyOptionalNodeState CreateNodeState(IReadOnlyBrowsingOptionalNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalInner<TIndex>));
            return new WriteableOptionalNodeState<IWriteableInner<IWriteableBrowsingChildIndex>>((IWriteableBrowsingOptionalNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingOptionalNodeIndex object.
        /// </summary>
        private protected virtual WriteableBrowsingOptionalNodeIndex CreateBrowsingNodeIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalInner<TIndex>));
            return new WriteableBrowsingOptionalNodeIndex(Owner.Node, PropertyName);
        }
        #endregion
    }
}
