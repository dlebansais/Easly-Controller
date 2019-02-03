namespace EaslyController.Writeable
{
    using BaseNode;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Base for block list index classes.
    /// </summary>
    public interface IWriteableBrowsingBlockNodeIndex : IReadOnlyBrowsingBlockNodeIndex, IWriteableBrowsingCollectionNodeIndex
    {
    }

    /// <summary>
    /// Base for block list index classes.
    /// </summary>
    internal abstract class WriteableBrowsingBlockNodeIndex : ReadOnlyBrowsingBlockNodeIndex, IWriteableBrowsingBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableBrowsingBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="node">The indexed node.</param>
        /// <param name="propertyName">The property for the index.</param>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        public WriteableBrowsingBlockNodeIndex(INode node, string propertyName, int blockIndex)
            : base(node, propertyName, blockIndex)
        {
        }
        #endregion
    }
}
