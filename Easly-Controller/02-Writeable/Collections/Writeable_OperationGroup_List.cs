namespace EaslyController.Writeable
{
    using System.Collections.Generic;

    /// <inheritdoc/>
    public class WriteableOperationGroupList : List<WriteableOperationGroup>
    {
        /// <inheritdoc/>
        public virtual WriteableOperationGroupReadOnlyList ToReadOnly()
        {
            return new WriteableOperationGroupReadOnlyList(this);
        }
    }
}
