namespace EaslyController.ReadOnly
{
    using BaseNode;

    /// <summary>
    /// Base interface for any index representing a node.
    /// </summary>
    public interface IReadOnlyNodeIndex : IReadOnlyIndex
    {
        /// <summary>
        /// The indexed node.
        /// </summary>
        INode Node { get; }
    }
}
