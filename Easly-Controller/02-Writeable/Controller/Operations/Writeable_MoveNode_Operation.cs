using System;
using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Operation details for moving a node in a list or block list.
    /// </summary>
    public interface IWriteableMoveNodeOperation : IWriteableOperation
    {
        /// <summary>
        /// Inner where the move is taking place.
        /// </summary>
        IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> Inner { get; }

        /// <summary>
        /// Index of the moved node.
        /// </summary>
        IWriteableBrowsingCollectionNodeIndex NodeIndex { get; }

        /// <summary>
        /// The current position before move.
        /// </summary>
        int Index { get; }

        /// <summary>
        /// The change in position, relative to the current position.
        /// </summary>
        int Direction { get; }

        /// <summary>
        /// State moved.
        /// </summary>
        IWriteablePlaceholderNodeState State { get; }

        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="childState">State moved.</param>
        void Update(IWriteablePlaceholderNodeState childState);
    }

    /// <summary>
    /// Operation details for moving a node in a list or block list.
    /// </summary>
    public class WriteableMoveNodeOperation : WriteableOperation, IWriteableMoveNodeOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="WriteableMoveNodeOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the move is taking place.</param>
        /// <param name="nodeIndex">Position where the node is moved.</param>
        /// <param name="direction">The change in position, relative to the current position.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableMoveNodeOperation(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableBrowsingCollectionNodeIndex nodeIndex, int direction, Action<IWriteableOperation> handlerRedo, bool isNested)
            : base(handlerRedo, isNested)
        {
            Inner = inner;
            NodeIndex = nodeIndex;

            if (nodeIndex is IWriteableBrowsingExistingBlockNodeIndex AsBlockIndex)
                Index = AsBlockIndex.Index;
            else if (nodeIndex is IWriteableBrowsingListNodeIndex AsListIndex)
                Index = AsListIndex.Index;
            else
                throw new ArgumentOutOfRangeException(nameof(nodeIndex));

            Direction = direction;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where the move is taking place.
        /// </summary>
        public IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> Inner { get; }

        /// <summary>
        /// Index of the moved node.
        /// </summary>
        public IWriteableBrowsingCollectionNodeIndex NodeIndex { get; }

        /// <summary>
        /// The current position before move.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// The change in position, relative to the current position.
        /// </summary>
        public int Direction { get; }

        /// <summary>
        /// State moved.
        /// </summary>
        public IWriteablePlaceholderNodeState State { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="state">State moved.</param>
        public virtual void Update(IWriteablePlaceholderNodeState state)
        {
            Debug.Assert(state != null);

            State = state;
        }
        #endregion
    }
}
