namespace EaslyController.Writeable
{
    using System.Collections.Generic;

    /// <inheritdoc/>
    public class WriteableOperationList : List<WriteableOperation>
    {
        /// <inheritdoc/>
        public virtual WriteableOperationReadOnlyList ToReadOnly()
        {
            return new WriteableOperationReadOnlyList(this);
        }
    }
}
