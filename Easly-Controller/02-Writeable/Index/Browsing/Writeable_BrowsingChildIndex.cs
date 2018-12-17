using EaslyController.ReadOnly;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Index for a child node.
    /// </summary>
    public interface IWriteableBrowsingChildIndex : IReadOnlyBrowsingChildIndex, IWriteableChildIndex
    {
    }
}
