namespace EaslyController.Frame
{
    using BaseNode;
    using EaslyController.Writeable;

    /// <summary>
    /// Base for block list index classes.
    /// </summary>
    public interface IFrameBrowsingBlockNodeIndex : IWriteableBrowsingBlockNodeIndex, IFrameBrowsingCollectionNodeIndex
    {
    }

    /// <summary>
    /// Base for block list index classes.
    /// </summary>
    internal abstract class FrameBrowsingBlockNodeIndex : WriteableBrowsingBlockNodeIndex, IFrameBrowsingBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameBrowsingBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="node">The indexed node.</param>
        /// <param name="propertyName">The property for the index.</param>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        public FrameBrowsingBlockNodeIndex(INode node, string propertyName, int blockIndex)
            : base(node, propertyName, blockIndex)
        {
        }
        #endregion
    }
}
