using BaseNode;
using EaslyController.Writeable;

namespace EaslyController.Frame
{
    /// <summary>
    /// Base for block list insertion index classes.
    /// </summary>
    public interface IFrameInsertionBlockNodeIndex : IWriteableInsertionBlockNodeIndex, IFrameInsertionCollectionNodeIndex
    {
    }

    /// <summary>
    /// Base for block list insertion index classes.
    /// </summary>
    public abstract class FrameInsertionBlockNodeIndex : WriteableInsertionBlockNodeIndex, IFrameInsertionBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameInsertionBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">The node in which the insertion operation is taking place.</param>
        /// <param name="propertyName">The property for the index.</param>
        /// <param name="node">The inserted node.</param>
        public FrameInsertionBlockNodeIndex(INode parentNode, string propertyName, INode node)
            : base(parentNode, propertyName, node)
        {
        }
        #endregion
    }
}
