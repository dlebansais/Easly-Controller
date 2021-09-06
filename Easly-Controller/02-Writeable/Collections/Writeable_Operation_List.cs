namespace EaslyController.Writeable
{
    using System.Collections.Generic;

    /// <inheritdoc/>
    public class WriteableOperationList : List<IWriteableOperation>
    {
        /// <inheritdoc/>
        public virtual WriteableOperationReadOnlyList ToReadOnly()
        {
            return new WriteableOperationReadOnlyList(this);
        }
    }
}
