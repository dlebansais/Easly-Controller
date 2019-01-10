namespace EaslyController.Writeable
{
    /// <summary>
    /// Details for insertion operations.
    /// </summary>
    public interface IWriteableInsertOperation : IWriteableOperation
    {
    }

    /// <summary>
    /// Details for insertion operations.
    /// </summary>
    public abstract class WriteableInsertOperation : WriteableOperation, IWriteableInsertOperation
    {
    }
}
