using EaslyController.Frame;

namespace EaslyController.Focus
{
    /// <summary>
    /// Operation details for removing a node in a list or block list.
    /// </summary>
    public interface IFocusRemoveNodeOperation : IFrameRemoveNodeOperation, IFocusRemoveOperation
    {
        /// <summary>
        /// Inner where the removal is taking place.
        /// </summary>
        new IFocusCollectionInner<IFocusBrowsingCollectionNodeIndex> Inner { get; }

        /// <summary>
        /// Index of the removed node.
        /// </summary>
        new IFocusBrowsingCollectionNodeIndex NodeIndex { get; }

        /// <summary>
        /// State removed.
        /// </summary>
        new IFocusPlaceholderNodeState ChildState { get; }
    }

    /// <summary>
    /// Operation details for removing a node in a list or block list.
    /// </summary>
    public class FocusRemoveNodeOperation : FrameRemoveNodeOperation, IFocusRemoveNodeOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FocusRemoveNodeOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the removal is taking place.</param>
        /// <param name="nodeIndex">Index of the removed node.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FocusRemoveNodeOperation(IFocusCollectionInner<IFocusBrowsingCollectionNodeIndex> inner, IFocusBrowsingCollectionNodeIndex nodeIndex, bool isNested)
            : base(inner, nodeIndex, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where the removal is taking place.
        /// </summary>
        public new IFocusCollectionInner<IFocusBrowsingCollectionNodeIndex> Inner { get { return (IFocusCollectionInner<IFocusBrowsingCollectionNodeIndex>)base.Inner; } }

        /// <summary>
        /// Index of the removed node.
        /// </summary>
        public new IFocusBrowsingCollectionNodeIndex NodeIndex { get { return (IFocusBrowsingCollectionNodeIndex)base.NodeIndex; } }

        /// <summary>
        /// State removed.
        /// </summary>
        public new IFocusPlaceholderNodeState ChildState { get { return (IFocusPlaceholderNodeState)base.ChildState; } }
        #endregion
    }
}
