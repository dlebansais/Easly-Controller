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
    }

    /// <summary>
    /// Inner for an optional node.
    /// </summary>
    public interface IWriteableOptionalInner<out IIndex> : IReadOnlyOptionalInner<IIndex>, IWriteableSingleInner<IIndex>
        where IIndex : IWriteableBrowsingOptionalNodeIndex
    {
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
        public new IWriteableNodeState ChildState { get { return (IWriteableNodeState)base.ChildState; } }
        #endregion

        #region Client Interface
        public virtual void Replace(IWriteableInsertionChildIndex nodeIndex, out IWriteableBrowsingChildIndex oldBrowsingIndex, out IWriteableBrowsingChildIndex newBrowsingIndex, out IWriteableNodeState childState)
        {
            Debug.Assert(nodeIndex != null);

            if (nodeIndex is IWriteableInsertionOptionalNodeIndex AsOptionalIndex)
                Replace(AsOptionalIndex, out oldBrowsingIndex, out newBrowsingIndex, out childState);
            else
                throw new ArgumentOutOfRangeException(nameof(nodeIndex));
        }

        protected virtual void Replace(IWriteableInsertionOptionalNodeIndex optionalIndex, out IWriteableBrowsingChildIndex oldBrowsingIndex, out IWriteableBrowsingChildIndex newBrowsingIndex, out IWriteableNodeState childState)
        {
            Debug.Assert(optionalIndex != null);
            Debug.Assert(optionalIndex.Optional != null);

            INode ParentNode = Owner.Node;

            oldBrowsingIndex = (WriteableBrowsingOptionalNodeIndex)ChildState.ParentIndex;
            NodeTreeHelperOptional.SetOptionalReference(ParentNode, PropertyName, optionalIndex.Optional);

            WriteableBrowsingOptionalNodeIndex BrowsingIndex = (WriteableBrowsingOptionalNodeIndex)optionalIndex.ToBrowsingIndex();
            newBrowsingIndex = BrowsingIndex;

            IWriteableOptionalNodeState NewChildState = (IWriteableOptionalNodeState)CreateNodeState(BrowsingIndex);
            SetChildState(NewChildState);

            childState = NewChildState;
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
        #endregion
    }
}
