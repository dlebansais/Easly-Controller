using System;
using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Operation details for removing a node in a list or block list.
    /// </summary>
    public interface IWriteableRemoveNodeOperation : IWriteableRemoveOperation
    {
        /// <summary>
        /// Inner where the removal is taking place.
        /// </summary>
        IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> Inner { get; }

        /// <summary>
        /// Index of the removed node.
        /// </summary>
        IWriteableBrowsingCollectionNodeIndex NodeIndex { get; }

        /// <summary>
        /// State removed.
        /// </summary>
        IWriteablePlaceholderNodeState ChildState { get; }

        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="childState">State removed.</param>
        void Update(IWriteablePlaceholderNodeState childState);

        /// <summary>
        /// Creates an operation to undo the remove operation.
        /// </summary>
        IWriteableInsertNodeOperation ToInsertNodeOperation();
    }

    /// <summary>
    /// Operation details for removing a node in a list or block list.
    /// </summary>
    public class WriteableRemoveNodeOperation : WriteableRemoveOperation, IWriteableRemoveNodeOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="WriteableRemoveNodeOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the removal is taking place.</param>
        /// <param name="nodeIndex">Index of the removed node.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableRemoveNodeOperation(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableBrowsingCollectionNodeIndex nodeIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
            Inner = inner;
            NodeIndex = nodeIndex;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where the removal is taking place.
        /// </summary>
        public IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> Inner { get; }

        /// <summary>
        /// Index of the removed node.
        /// </summary>
        public IWriteableBrowsingCollectionNodeIndex NodeIndex { get; private set; }

        /// <summary>
        /// State removed.
        /// </summary>
        public IWriteablePlaceholderNodeState ChildState { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="childState">State removed.</param>
        public virtual void Update(IWriteablePlaceholderNodeState childState)
        {
            Debug.Assert(childState != null);

            ChildState = childState;
        }

        /// <summary>
        /// Creates an operation to undo the remove operation.
        /// </summary>
        public IWriteableInsertNodeOperation ToInsertNodeOperation()
        {
            IWriteableInsertionCollectionNodeIndex InsertionIndex = NodeIndex.ToInsertionIndex(Inner.Owner.Node, ChildState.Node) as IWriteableInsertionCollectionNodeIndex;
            Debug.Assert(InsertionIndex != null);

            return CreateInsertNodeOperation(Inner, InsertionIndex, HandlerUndo, HandlerRedo, IsNested);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxInsertNodeOperation object.
        /// </summary>
        protected virtual IWriteableInsertNodeOperation CreateInsertNodeOperation(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableInsertionCollectionNodeIndex insertionIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableRemoveNodeOperation));
            return new WriteableInsertNodeOperation(inner, insertionIndex, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
