namespace EaslyController.Writeable
{
    /// <summary>
    /// Details for removal operations.
    /// </summary>
    public interface IWriteableRemoveOperation : IWriteableOperation
    {
    }

    /// <summary>
    /// Details for removal operations.
    /// </summary>
    public abstract class WriteableRemoveOperation : WriteableOperation, IWriteableRemoveOperation
    {
    }
}
