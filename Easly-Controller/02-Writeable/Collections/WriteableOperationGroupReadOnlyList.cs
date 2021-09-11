namespace EaslyController.Writeable
{
    using System.Collections.ObjectModel;

    /// <inheritdoc/>
    public class WriteableOperationGroupReadOnlyList : ReadOnlyCollection<WriteableOperationGroup>
    {
        /// <inheritdoc/>
        public WriteableOperationGroupReadOnlyList(WriteableOperationGroupList list)
            : base(list)
        {
        }
    }
}
