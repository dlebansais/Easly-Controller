using BaseNode;
using BaseNodeHelper;
using EaslyController.ReadOnly;
using System;
using System.Diagnostics;

namespace EaslyController.Writeable
{
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
        void Assign(IWriteableAssignmentOperation operation);

        /// <summary>
        /// Unassign the optional node.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        void Unassign(IWriteableAssignmentOperation operation);
    }

    /// <summary>
    /// Inner for an optional node.
    /// </summary>
    public interface IWriteableOptionalInner<out IIndex> : IReadOnlyOptionalInner<IIndex>, IWriteableSingleInner<IIndex>
        where IIndex : IWriteableBrowsingOptionalNodeIndex
    {
        /// <summary>
        /// The state of the optional node.
        /// </summary>
        new IWriteableOptionalNodeState ChildState { get; }

        /// <summary>
        /// Assign the optional node.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        void Assign(IWriteableAssignmentOperation operation);

        /// <summary>
        /// Unassign the optional node.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        void Unassign(IWriteableAssignmentOperation operation);
    }

    /// <summary>
    /// Inner for an optional node.
    /// </summary>
    public class WriteableOptionalInner<IIndex, TIndex> : ReadOnlyOptionalInner<IIndex, TIndex>, IWriteableOptionalInner<IIndex>, IWriteableOptionalInner
        where IIndex : IWriteableBrowsingOptionalNodeIndex
        where TIndex : WriteableBrowsingOptionalNodeIndex, IIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableOptionalInner{IIndex, TIndex}"/> class.
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
        IWriteableNodeState IWriteableSingleInner.ChildState { get { return ChildState; } }
        IWriteableNodeState IWriteableSingleInner<IIndex>.ChildState { get { return ChildState; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Replaces a node.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void Replace(IWriteableReplaceOperation operation)
        {
            Debug.Assert(operation != null);

            if (operation.NewNode != null)
                ReplaceOptional(operation);
            else
                ClearOptional(operation);
        }

        /// <summary></summary>
        protected virtual void ReplaceOptional(IWriteableReplaceOperation operation)
        {
            INode ParentNode = Owner.Node;

            WriteableBrowsingOptionalNodeIndex OldBrowsingIndex = (WriteableBrowsingOptionalNodeIndex)ChildState.ParentIndex;
            INode OldNode = ChildState.Optional.HasItem ? ChildState.Node : null;

            NodeTreeHelperOptional.SetOptionalChildNode(ParentNode, PropertyName, operation.NewNode);

            IWriteableBrowsingOptionalNodeIndex NewBrowsingIndex = CreateBrowsingNodeIndex();
            IWriteableOptionalNodeState NewChildState = (IWriteableOptionalNodeState)CreateNodeState(NewBrowsingIndex);
            SetChildState(NewChildState);

            operation.Update(OldBrowsingIndex, NewBrowsingIndex, OldNode, NewChildState);
        }

        /// <summary></summary>
        protected virtual void ClearOptional(IWriteableReplaceOperation operation)
        {
            INode ParentNode = Owner.Node;

            WriteableBrowsingOptionalNodeIndex OldBrowsingIndex = (WriteableBrowsingOptionalNodeIndex)ChildState.ParentIndex;
            INode OldNode = ChildState.Optional.HasItem ? ChildState.Node : null;

            NodeTreeHelperOptional.ClearOptionalChildNode(ParentNode, PropertyName);

            IWriteableBrowsingOptionalNodeIndex NewBrowsingIndex = CreateBrowsingNodeIndex();
            IWriteableOptionalNodeState NewChildState = (IWriteableOptionalNodeState)CreateNodeState(NewBrowsingIndex);
            SetChildState(NewChildState);

            operation.Update(OldBrowsingIndex, NewBrowsingIndex, OldNode, NewChildState);
        }

        /// <summary>
        /// Assign the optional node.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public void Assign(IWriteableAssignmentOperation operation)
        {
            NodeTreeHelperOptional.AssignChildNode(Owner.Node, PropertyName);

            operation.Update(ChildState);
        }

        /// <summary>
        /// Unassign the optional node.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public void Unassign(IWriteableAssignmentOperation operation)
        {
            NodeTreeHelperOptional.UnassignChildNode(Owner.Node, PropertyName);

            operation.Update(ChildState);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxOptionalNodeState object.
        /// </summary>
        protected override IReadOnlyOptionalNodeState CreateNodeState(IReadOnlyBrowsingOptionalNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalInner<IIndex, TIndex>));
            return new WriteableOptionalNodeState((IWriteableBrowsingOptionalNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingOptionalNodeIndex object.
        /// </summary>
        protected virtual IWriteableBrowsingOptionalNodeIndex CreateBrowsingNodeIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalInner<IIndex, TIndex>));
            return new WriteableBrowsingOptionalNodeIndex(Owner.Node, PropertyName);
        }
        #endregion
    }
}
