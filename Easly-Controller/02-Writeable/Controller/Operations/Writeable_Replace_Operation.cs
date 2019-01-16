﻿using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Operation details for replacing a node.
    /// </summary>
    public interface IWriteableReplaceOperation : IWriteableOperation
    {
        /// <summary>
        /// Inner where the replacement is taking place.
        /// </summary>
        IWriteableInner<IWriteableBrowsingChildIndex> Inner { get; }

        /// <summary>
        /// Position where the node is replaced.
        /// </summary>
        IWriteableInsertionChildIndex ReplacementIndex { get; }

        /// <summary>
        /// Index of the state before it's replaced.
        /// </summary>
        IWriteableBrowsingChildIndex OldBrowsingIndex { get; }

        /// <summary>
        /// Index of the state after it's replaced.
        /// </summary>
        IWriteableBrowsingChildIndex NewBrowsingIndex { get; }

        /// <summary>
        /// The new state.
        /// </summary>
        IWriteableNodeState ChildState { get; }

        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="oldBrowsingIndex">Index of the state before it's replaced.</param>
        /// <param name="newBrowsingIndex">Index of the state after it's replaced.</param>
        /// <param name="childState">The new state.</param>
        void Update(IWriteableBrowsingChildIndex oldBrowsingIndex, IWriteableBrowsingChildIndex newBrowsingIndex, IWriteableNodeState childState);
    }

    /// <summary>
    /// Operation details for replacing a node in a list or block list.
    /// </summary>
    public class WriteableReplaceOperation : WriteableOperation, IWriteableReplaceOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="WriteableReplaceOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the replacement is taking place.</param>
        /// <param name="replacementIndex">Position where the node is replaced.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableReplaceOperation(IWriteableInner<IWriteableBrowsingChildIndex> inner, IWriteableInsertionChildIndex replacementIndex, bool isNested)
            : base(isNested)
        {
            Inner = inner;
            ReplacementIndex = replacementIndex;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where the replacement is taking place.
        /// </summary>
        public IWriteableInner<IWriteableBrowsingChildIndex> Inner { get; }

        /// <summary>
        /// Position where the node is replaced.
        /// </summary>
        public IWriteableInsertionChildIndex ReplacementIndex { get; }

        /// <summary>
        /// Index of the state before it's replaced.
        /// </summary>
        public IWriteableBrowsingChildIndex OldBrowsingIndex { get; private set; }

        /// <summary>
        /// Index of the state after it's replaced.
        /// </summary>
        public IWriteableBrowsingChildIndex NewBrowsingIndex { get; private set; }

        /// <summary>
        /// The new state.
        /// </summary>
        public IWriteableNodeState ChildState { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="oldBrowsingIndex">Index of the state before it's replaced.</param>
        /// <param name="newBrowsingIndex">Index of the state after it's replaced.</param>
        /// <param name="childState">The new state.</param>
        public virtual void Update(IWriteableBrowsingChildIndex oldBrowsingIndex, IWriteableBrowsingChildIndex newBrowsingIndex, IWriteableNodeState childState)
        {
            Debug.Assert(oldBrowsingIndex != null);
            Debug.Assert(newBrowsingIndex != null);
            Debug.Assert(childState != null);

            OldBrowsingIndex = oldBrowsingIndex;
            NewBrowsingIndex = newBrowsingIndex;
            ChildState = childState;
        }
        #endregion
    }
}
