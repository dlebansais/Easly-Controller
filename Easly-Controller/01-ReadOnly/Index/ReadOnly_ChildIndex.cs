namespace EaslyController.ReadOnly
{
    /// <summary>
    /// Base interface for any index representing the child node of a parent node.
    /// </summary>
    public interface IReadOnlyChildIndex : IReadOnlyIndex
    {
        /// <summary>
        /// The property in the parent for the indexed node.
        /// </summary>
        string PropertyName { get; }
    }
}
