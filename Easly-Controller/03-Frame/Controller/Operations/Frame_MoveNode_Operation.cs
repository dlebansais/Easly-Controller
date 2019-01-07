using EaslyController.Writeable;

namespace EaslyController.Frame
{
    /// <summary>
    /// Operation details for moving a node in a list or block list.
    /// </summary>
    public interface IFrameMoveNodeOperation : IWriteableMoveNodeOperation, IFrameOperation
    {
        /// <summary>
        /// Inner where the move is taking place.
        /// </summary>
        new IFrameCollectionInner<IFrameBrowsingCollectionNodeIndex> Inner { get; }

        /// <summary>
        /// Index of the moved node.
        /// </summary>
        new IFrameBrowsingCollectionNodeIndex NodeIndex { get; }

        /// <summary>
        /// State moved.
        /// </summary>
        new IFramePlaceholderNodeState State { get; }
    }

    /// <summary>
    /// Operation details for moving a node in a list or block list.
    /// </summary>
    public class FrameMoveNodeOperation : WriteableMoveNodeOperation, IFrameMoveNodeOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FrameMoveNodeOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the move is taking place.</param>
        /// <param name="moveIndex">Position where the node is moved.</param>
        /// <param name="direction">The change in position, relative to the current position.</param>
        public FrameMoveNodeOperation(IFrameCollectionInner<IFrameBrowsingCollectionNodeIndex> inner, IFrameBrowsingCollectionNodeIndex nodeIndex, int direction)
            : base(inner, nodeIndex, direction)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where the move is taking place.
        /// </summary>
        public new IFrameCollectionInner<IFrameBrowsingCollectionNodeIndex> Inner { get { return (IFrameCollectionInner<IFrameBrowsingCollectionNodeIndex>)base.Inner; } }

        /// <summary>
        /// Index of the moved node.
        /// </summary>
        public new IFrameBrowsingCollectionNodeIndex NodeIndex { get { return (IFrameBrowsingCollectionNodeIndex)base.NodeIndex; } }

        /// <summary>
        /// State moved.
        /// </summary>
        public new IFramePlaceholderNodeState State { get { return (IFramePlaceholderNodeState)base.State; } }
        #endregion
    }
}
