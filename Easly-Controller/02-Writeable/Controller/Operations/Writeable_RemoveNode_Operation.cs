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
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableRemoveNodeOperation(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableBrowsingCollectionNodeIndex nodeIndex, bool isNested)
            : base(isNested)
        {
            Inner = inner;
            NodeIndex = nodeIndex;
        }
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
    }
}
