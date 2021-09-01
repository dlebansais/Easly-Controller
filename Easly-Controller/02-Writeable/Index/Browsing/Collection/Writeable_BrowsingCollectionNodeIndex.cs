namespace EaslyController.Writeable
{
    using BaseNode;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Base for list and block list index classes.
    /// </summary>
    public interface IWriteableBrowsingCollectionNodeIndex : IReadOnlyBrowsingCollectionNodeIndex, IWriteableBrowsingChildIndex, IWriteableNodeIndex
    {
    }

    /// <summary>
    /// Base for list and block list index classes.
    /// </summary>
    public abstract class WriteableBrowsingCollectionNodeIndex : ReadOnlyBrowsingCollectionNodeIndex, IWriteableBrowsingCollectionNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableBrowsingCollectionNodeIndex"/> class.
        /// </summary>
        /// <param name="node">The indexed node.</param>
        /// <param name="propertyName">The property for the index.</param>
        public WriteableBrowsingCollectionNodeIndex(Node node, string propertyName)
            : base(node, propertyName)
        {
        }
        #endregion
    }
}
