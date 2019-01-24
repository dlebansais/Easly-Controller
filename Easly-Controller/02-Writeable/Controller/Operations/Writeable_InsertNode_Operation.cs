using System;
using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Operation details for inserting a node in a list or block list.
    /// </summary>
    public interface IWriteableInsertNodeOperation : IWriteableInsertOperation
    {
        /// <summary>
        /// Inner where the insertion is taking place.
        /// </summary>
        IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> Inner { get; }

        /// <summary>
        /// Position where the node is inserted.
        /// </summary>
        IWriteableInsertionCollectionNodeIndex InsertionIndex { get; }

        /// <summary>
        /// Index of the state after it's inserted.
        /// </summary>
        IWriteableBrowsingCollectionNodeIndex BrowsingIndex { get; }

        /// <summary>
        /// State inserted.
        /// </summary>
        IWriteablePlaceholderNodeState ChildState { get; }

        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="browsingIndex">Index of the state after it's inserted.</param>
        /// <param name="childState">State inserted.</param>
        void Update(IWriteableBrowsingCollectionNodeIndex browsingIndex, IWriteablePlaceholderNodeState childState);

        /// <summary>
        /// Creates an operation to undo the insert operation.
        /// </summary>
        IWriteableRemoveNodeOperation ToRemoveNodeOperation();
    }

    /// <summary>
    /// Operation details for inserting a node in a list or block list.
    /// </summary>
    public class WriteableInsertNodeOperation : WriteableInsertOperation, IWriteableInsertNodeOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="WriteableInsertNodeOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the insertion is taking place.</param>
        /// <param name="insertionIndex">Position where the node is inserted.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableInsertNodeOperation(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableInsertionCollectionNodeIndex insertionIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
            Inner = inner;
            InsertionIndex = insertionIndex;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where the insertion is taking place.
        /// </summary>
        public IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> Inner { get; }

        /// <summary>
        /// Position where the node is inserted.
        /// </summary>
        public IWriteableInsertionCollectionNodeIndex InsertionIndex { get; }

        /// <summary>
        /// Index of the state after it's inserted.
        /// </summary>
        public IWriteableBrowsingCollectionNodeIndex BrowsingIndex { get; private set; }

        /// <summary>
        /// State inserted.
        /// </summary>
        public IWriteablePlaceholderNodeState ChildState { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="browsingIndex">Index of the state after it's inserted.</param>
        /// <param name="childState">State inserted.</param>
        public virtual void Update(IWriteableBrowsingCollectionNodeIndex browsingIndex, IWriteablePlaceholderNodeState childState)
        {
            Debug.Assert(browsingIndex != null);
            Debug.Assert(childState != null);

            BrowsingIndex = browsingIndex;
            ChildState = childState;
        }

        /// <summary>
        /// Creates an operation to undo the insert operation.
        /// </summary>
        public IWriteableRemoveNodeOperation ToRemoveNodeOperation()
        {
            return CreateRemoveNodeOperation(Inner, BrowsingIndex, HandlerUndo, HandlerRedo, IsNested);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxRemoveNodeOperation object.
        /// </summary>
        protected virtual IWriteableRemoveNodeOperation CreateRemoveNodeOperation(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableBrowsingCollectionNodeIndex nodeIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableInsertNodeOperation));
            return new WriteableRemoveNodeOperation(inner, nodeIndex, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
