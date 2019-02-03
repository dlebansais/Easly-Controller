namespace EaslyController.Writeable
{
    using EaslyController.ReadOnly;

    /// <summary>
    /// Index for a child node.
    /// </summary>
    public interface IWriteableBrowsingChildIndex : IReadOnlyBrowsingChildIndex, IWriteableChildIndex
    {
    }
}
