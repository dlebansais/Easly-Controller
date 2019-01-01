using BaseNode;
using EaslyController.Writeable;

namespace EaslyController.Frame
{
    /// <summary>
    /// Base for list and block list index classes.
    /// </summary>
    public interface IFrameBrowsingCollectionNodeIndex : IWriteableBrowsingCollectionNodeIndex, IFrameBrowsingChildIndex, IFrameNodeIndex
    {
    }

    /// <summary>
    /// Base for list and block list index classes.
    /// </summary>
    public abstract class FrameBrowsingCollectionNodeIndex : WriteableBrowsingCollectionNodeIndex, IFrameBrowsingCollectionNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameBrowsingCollectionNodeIndex"/> class.
        /// </summary>
        /// <param name="node">The indexed node.</param>
        /// <param name="propertyName">The property for the index.</param>
        public FrameBrowsingCollectionNodeIndex(INode node, string propertyName)
            : base(node, propertyName)
        {
        }
        #endregion
    }
}
