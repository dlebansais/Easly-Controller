namespace EaslyController.Writeable
{
    using BaseNode;

    /// <summary>
    /// Index for a child node.
    /// </summary>
    public interface IWriteableBrowsingInsertableIndex
    {
        /// <summary>
        /// Creates an insertion index from this instance, that can be used to replace it.
        /// </summary>
        /// <param name="parentNode">The parent node where the index would be used to replace a node.</param>
        /// <param name="node">The node inserted.</param>
        IWriteableInsertionChildIndex ToInsertionIndex(Node parentNode, Node node);
    }
}
