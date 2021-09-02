namespace EaslyController.Writeable
{
    using System.Collections.ObjectModel;

    /// <inheritdoc/>
    public class WriteableOperationReadOnlyList : ReadOnlyCollection<WriteableOperation>
    {
        /// <inheritdoc/>
        public WriteableOperationReadOnlyList(WriteableOperationList list)
            : base(list)
        {
        }
    }
}
