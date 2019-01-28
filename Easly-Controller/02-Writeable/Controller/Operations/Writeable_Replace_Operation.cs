using System;
using System.Diagnostics;

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
        /// The old state.
        /// </summary>
        IWriteableNodeState OldChildState { get; }

        /// <summary>
        /// The new state.
        /// </summary>
        IWriteableNodeState NewChildState { get; }

        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="oldBrowsingIndex">Index of the state before it's replaced.</param>
        /// <param name="newBrowsingIndex">Index of the state after it's replaced.</param>
        /// <param name="oldChildState">The old state.</param>
        /// <param name="newChildState">The new state.</param>
        void Update(IWriteableBrowsingChildIndex oldBrowsingIndex, IWriteableBrowsingChildIndex newBrowsingIndex, IWriteableNodeState oldChildState, IWriteableNodeState newChildState);

        /// <summary>
        /// Creates an operation to undo the replace operation.
        /// </summary>
        IWriteableReplaceOperation ToInverseReplace();
    }

    /// <summary>
    /// Operation details for replacing a node.
    /// </summary>
    public class WriteableReplaceOperation : WriteableOperation, IWriteableReplaceOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="WriteableReplaceOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the replacement is taking place.</param>
        /// <param name="replacementIndex">Position where the node is replaced.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableReplaceOperation(IWriteableInner<IWriteableBrowsingChildIndex> inner, IWriteableInsertionChildIndex replacementIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
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
        /// The old state.
        /// </summary>
        public IWriteableNodeState OldChildState { get; private set; }

        /// <summary>
        /// The new state.
        /// </summary>
        public IWriteableNodeState NewChildState { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="oldBrowsingIndex">Index of the state before it's replaced.</param>
        /// <param name="newBrowsingIndex">Index of the state after it's replaced.</param>
        /// <param name="oldChildState">The old state.</param>
        /// <param name="newChildState">The new state.</param>
        public virtual void Update(IWriteableBrowsingChildIndex oldBrowsingIndex, IWriteableBrowsingChildIndex newBrowsingIndex, IWriteableNodeState oldChildState, IWriteableNodeState newChildState)
        {
            Debug.Assert(oldBrowsingIndex != null);
            Debug.Assert(newBrowsingIndex != null);
            Debug.Assert(oldChildState != null);
            Debug.Assert(newChildState != null);

            OldBrowsingIndex = oldBrowsingIndex;
            NewBrowsingIndex = newBrowsingIndex;
            OldChildState = oldChildState;
            NewChildState = newChildState;
        }

        /// <summary>
        /// Creates an operation to undo the replace operation.
        /// </summary>
        public IWriteableReplaceOperation ToInverseReplace()
        {
            IWriteableInsertionChildIndex ReplacementIndex = NewBrowsingIndex.ToInsertionIndex(Inner.Owner.Node, OldChildState.Node);
            return CreateReplaceOperation(Inner, ReplacementIndex, HandlerUndo, HandlerRedo, IsNested);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxReplaceOperation object.
        /// </summary>
        protected virtual IWriteableReplaceOperation CreateReplaceOperation(IWriteableInner<IWriteableBrowsingChildIndex> inner, IWriteableInsertionChildIndex replacementIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableReplaceOperation));
            return new WriteableReplaceOperation(inner, replacementIndex, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
