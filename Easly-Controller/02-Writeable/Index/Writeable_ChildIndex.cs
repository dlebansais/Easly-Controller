using EaslyController.ReadOnly;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Base interface for any index representing the child node of a parent node.
    /// </summary>
    public interface IWriteableChildIndex : IReadOnlyChildIndex, IWriteableIndex
    {
    }
}
