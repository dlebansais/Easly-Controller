using EaslyController.Frame;

namespace EaslyController.Focus
{
    /// <summary>
    /// Operation details for inserting a node in a list or block list.
    /// </summary>
    public interface IFocusInsertNodeOperation : IFrameInsertNodeOperation, IFocusInsertOperation
    {
        /// <summary>
        /// Inner where the insertion is taking place.
        /// </summary>
        new IFocusCollectionInner<IFocusBrowsingCollectionNodeIndex> Inner { get; }

        /// <summary>
        /// Position where the node is inserted.
        /// </summary>
        new IFocusInsertionCollectionNodeIndex InsertionIndex { get; }

        /// <summary>
        /// Index of the state after it's inserted.
        /// </summary>
        new IFocusBrowsingCollectionNodeIndex BrowsingIndex { get; }

        /// <summary>
        /// State inserted.
        /// </summary>
        new IFocusPlaceholderNodeState ChildState { get; }
    }

    /// <summary>
    /// Operation details for inserting a node in a list or block list.
    /// </summary>
    public class FocusInsertNodeOperation : FrameInsertNodeOperation, IFocusInsertNodeOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FocusInsertNodeOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the insertion is taking place.</param>
        /// <param name="insertionIndex">Position where the node is inserted.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FocusInsertNodeOperation(IFocusCollectionInner<IFocusBrowsingCollectionNodeIndex> inner, IFocusInsertionCollectionNodeIndex insertionIndex, bool isNested)
            : base(inner, insertionIndex, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where the insertion is taking place.
        /// </summary>
        public new IFocusCollectionInner<IFocusBrowsingCollectionNodeIndex> Inner { get { return (IFocusCollectionInner<IFocusBrowsingCollectionNodeIndex>)base.Inner; } }

        /// <summary>
        /// Position where the node is inserted.
        /// </summary>
        public new IFocusInsertionCollectionNodeIndex InsertionIndex { get { return (IFocusInsertionCollectionNodeIndex)base.InsertionIndex; } }

        /// <summary>
        /// Index of the state after it's inserted.
        /// </summary>
        public new IFocusBrowsingCollectionNodeIndex BrowsingIndex { get { return (IFocusBrowsingCollectionNodeIndex)base.BrowsingIndex; } }

        /// <summary>
        /// State inserted.
        /// </summary>
        public new IFocusPlaceholderNodeState ChildState { get { return (IFocusPlaceholderNodeState)base.ChildState; } }
        #endregion
    }
}
