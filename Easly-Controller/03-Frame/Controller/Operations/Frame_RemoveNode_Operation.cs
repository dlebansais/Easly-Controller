using EaslyController.Writeable;

namespace EaslyController.Frame
{
    /// <summary>
    /// Operation details for removing a node in a list or block list.
    /// </summary>
    public interface IFrameRemoveNodeOperation : IWriteableRemoveNodeOperation, IFrameRemoveOperation
    {
        /// <summary>
        /// Inner where the removal is taking place.
        /// </summary>
        new IFrameCollectionInner<IFrameBrowsingCollectionNodeIndex> Inner { get; }

        /// <summary>
        /// Index of the removed node.
        /// </summary>
        new IFrameBrowsingCollectionNodeIndex NodeIndex { get; }

        /// <summary>
        /// State removed.
        /// </summary>
        new IFramePlaceholderNodeState ChildState { get; }
    }

    /// <summary>
    /// Operation details for removing a node in a list or block list.
    /// </summary>
    public class FrameRemoveNodeOperation : WriteableRemoveNodeOperation, IFrameRemoveNodeOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FrameRemoveNodeOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the removal is taking place.</param>
        /// <param name="nodeIndex">Index of the removed node.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FrameRemoveNodeOperation(IFrameCollectionInner<IFrameBrowsingCollectionNodeIndex> inner, IFrameBrowsingCollectionNodeIndex nodeIndex, bool isNested)
            : base(inner, nodeIndex, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where the removal is taking place.
        /// </summary>
        public new IFrameCollectionInner<IFrameBrowsingCollectionNodeIndex> Inner { get { return (IFrameCollectionInner<IFrameBrowsingCollectionNodeIndex>)base.Inner; } }

        /// <summary>
        /// Index of the removed node.
        /// </summary>
        public new IFrameBrowsingCollectionNodeIndex NodeIndex { get { return (IFrameBrowsingCollectionNodeIndex)base.NodeIndex; } }

        /// <summary>
        /// State removed.
        /// </summary>
        public new IFramePlaceholderNodeState ChildState { get { return (IFramePlaceholderNodeState)base.ChildState; } }
        #endregion
    }
}
