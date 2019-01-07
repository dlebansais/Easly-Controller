using EaslyController.Writeable;

namespace EaslyController.Frame
{
    /// <summary>
    /// Operation details for replacing a node.
    /// </summary>
    public interface IFrameReplaceOperation : IWriteableReplaceOperation, IFrameOperation
    {
        /// <summary>
        /// Inner where the replacement is taking place.
        /// </summary>
        new IFrameInner<IFrameBrowsingChildIndex> Inner { get; }

        /// <summary>
        /// Position where the node is replaced.
        /// </summary>
        new IFrameInsertionChildIndex ReplacementIndex { get; }

        /// <summary>
        /// Index of the state after it's replaced.
        /// </summary>
        new IFrameBrowsingChildIndex BrowsingIndex { get; }

        /// <summary>
        /// The new state.
        /// </summary>
        new IFrameNodeState ChildState { get; }
    }

    /// <summary>
    /// Operation details for replacing a node in a list or block list.
    /// </summary>
    public class FrameReplaceOperation : WriteableReplaceOperation, IFrameReplaceOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FrameReplaceOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the replacement is taking place.</param>
        /// <param name="replacementIndex">Position where the node is replaced.</param>
        public FrameReplaceOperation(IFrameInner<IFrameBrowsingChildIndex> inner, IFrameInsertionChildIndex replacementIndex)
            : base(inner, replacementIndex)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where the replacement is taking place.
        /// </summary>
        public new IFrameInner<IFrameBrowsingChildIndex> Inner { get { return (IFrameInner<IFrameBrowsingChildIndex>)base.Inner; } }

        /// <summary>
        /// Position where the node is replaced.
        /// </summary>
        public new IFrameInsertionChildIndex ReplacementIndex { get { return (IFrameInsertionChildIndex)base.ReplacementIndex; } }

        /// <summary>
        /// Index of the state after it's replaced.
        /// </summary>
        public new IFrameBrowsingChildIndex BrowsingIndex { get { return (IFrameBrowsingChildIndex)base.BrowsingIndex; } }

        /// <summary>
        /// The new state.
        /// </summary>
        public new IFrameNodeState ChildState { get { return (IFrameNodeState)base.ChildState; } }
        #endregion
    }
}
