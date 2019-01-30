namespace EaslyController.Writeable
{
    using EaslyController.ReadOnly;

    /// <summary>
    /// Base interface for any index representing a node.
    /// </summary>
    public interface IWriteableNodeIndex : IReadOnlyNodeIndex, IWriteableIndex
    {
    }
}
