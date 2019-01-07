using BaseNode;
using BaseNodeHelper;
using EaslyController.ReadOnly;
using System;
using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Inner for a child node.
    /// </summary>
    public interface IWriteablePlaceholderInner : IReadOnlyPlaceholderInner, IWriteableSingleInner
    {
    }

    /// <summary>
    /// Inner for a child node.
    /// </summary>
    public interface IWriteablePlaceholderInner<out IIndex> : IReadOnlyPlaceholderInner<IIndex>, IWriteableSingleInner<IIndex>
        where IIndex : IWriteableBrowsingPlaceholderNodeIndex
    {
    }

    /// <summary>
    /// Inner for a child node.
    /// </summary>
    public class WriteablePlaceholderInner<IIndex, TIndex> : ReadOnlyPlaceholderInner<IIndex, TIndex>, IWriteablePlaceholderInner<IIndex>, IWriteablePlaceholderInner
        where IIndex : IWriteableBrowsingPlaceholderNodeIndex
        where TIndex : WriteableBrowsingPlaceholderNodeIndex, IIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteablePlaceholderInner{IIndex, TIndex}"/> class.
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
        public new IWriteableNodeState ChildState { get { return (IWriteableNodeState)base.ChildState; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Replaces a node.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        /// <param name="nodeIndex">Index of the node to insert.</param>
        /// <param name="oldBrowsingIndex">Index of the replaced node upon return.</param>
        /// <param name="newBrowsingIndex">Index of the inserted node upon return.</param>
        /// <param name="childState">State of the inserted node upon return.</param>
        public virtual void Replace(IWriteableReplaceOperation operation, IWriteableInsertionChildIndex nodeIndex, out IWriteableBrowsingChildIndex oldBrowsingIndex, out IWriteableBrowsingChildIndex newBrowsingIndex, out IWriteableNodeState childState)
        {
            Debug.Assert(nodeIndex != null);

            if (nodeIndex is IWriteableInsertionPlaceholderNodeIndex AsPlaceholderIndex)
                Replace(operation, AsPlaceholderIndex, out oldBrowsingIndex, out newBrowsingIndex, out childState);
            else
                throw new ArgumentOutOfRangeException(nameof(nodeIndex));
        }

        protected virtual void Replace(IWriteableReplaceOperation operation, IWriteableInsertionPlaceholderNodeIndex placeholderIndex, out IWriteableBrowsingChildIndex oldBrowsingIndex, out IWriteableBrowsingChildIndex newBrowsingIndex, out IWriteableNodeState childState)
        {
            Debug.Assert(placeholderIndex != null);

            INode ParentNode = Owner.Node;

            oldBrowsingIndex = (WriteableBrowsingPlaceholderNodeIndex)ChildState.ParentIndex;
            NodeTreeHelperChild.SetChildNode(ParentNode, PropertyName, placeholderIndex.Node);

            WriteableBrowsingPlaceholderNodeIndex BrowsingIndex = (WriteableBrowsingPlaceholderNodeIndex)placeholderIndex.ToBrowsingIndex();
            newBrowsingIndex = BrowsingIndex;

            IWriteablePlaceholderNodeState NewChildState = (IWriteablePlaceholderNodeState)CreateNodeState(BrowsingIndex);
            SetChildState(NewChildState);

            childState = NewChildState;

            operation.Update(BrowsingIndex, NewChildState);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        protected override IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyBrowsingPlaceholderNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteablePlaceholderInner<IIndex, TIndex>));
            return new WriteablePlaceholderNodeState((IWriteableBrowsingPlaceholderNodeIndex)nodeIndex);
        }
        #endregion
    }
}
