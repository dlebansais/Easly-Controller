using EaslyController.ReadOnly;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Base interface for any index representing a node.
    /// </summary>
    public interface IWriteableNodeIndex : IReadOnlyNodeIndex, IWriteableIndex
    {
    }
}
