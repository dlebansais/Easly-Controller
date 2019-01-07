using EaslyController.Writeable;

namespace EaslyController.Frame
{
    /// <summary>
    /// Operation details for inserting a node in a list or block list.
    /// </summary>
    public interface IFrameInsertNodeOperation : IWriteableInsertNodeOperation, IFrameInsertOperation
    {
        /// <summary>
        /// Inner where the insertion is taking place.
        /// </summary>
        new IFrameCollectionInner<IFrameBrowsingCollectionNodeIndex> Inner { get; }

        /// <summary>
        /// Position where the node is inserted.
        /// </summary>
        new IFrameInsertionCollectionNodeIndex InsertionIndex { get; }

        /// <summary>
        /// Index of the state after it's inserted.
        /// </summary>
        new IFrameBrowsingCollectionNodeIndex BrowsingIndex { get; }

        /// <summary>
        /// State inserted.
        /// </summary>
        new IFramePlaceholderNodeState ChildState { get; }
    }

    /// <summary>
    /// Operation details for inserting a node in a list or block list.
    /// </summary>
    public class FrameInsertNodeOperation : WriteableInsertNodeOperation, IFrameInsertNodeOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FrameInsertNodeOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the insertion is taking place.</param>
        /// <param name="insertionIndex">Position where the node is inserted.</param>
        public FrameInsertNodeOperation(IFrameCollectionInner<IFrameBrowsingCollectionNodeIndex> inner, IFrameInsertionCollectionNodeIndex insertionIndex)
            : base(inner, insertionIndex)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where the insertion is taking place.
        /// </summary>
        public new IFrameCollectionInner<IFrameBrowsingCollectionNodeIndex> Inner { get { return (IFrameCollectionInner<IFrameBrowsingCollectionNodeIndex>)base.Inner; } }

        /// <summary>
        /// Position where the node is inserted.
        /// </summary>
        public new IFrameInsertionCollectionNodeIndex InsertionIndex { get { return (IFrameInsertionCollectionNodeIndex)base.InsertionIndex; } }

        /// <summary>
        /// Index of the state after it's inserted.
        /// </summary>
        public new IFrameBrowsingCollectionNodeIndex BrowsingIndex { get { return (IFrameBrowsingCollectionNodeIndex)base.BrowsingIndex; } }

        /// <summary>
        /// State inserted.
        /// </summary>
        public new IFramePlaceholderNodeState ChildState { get { return (IFramePlaceholderNodeState)base.ChildState; } }
        #endregion
    }
}
