#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// List of IxxxOperation
    /// </summary>
    public class WriteableOperationList : List<WriteableOperation>
    {
        public virtual WriteableOperationReadOnlyList ToReadOnly()
        {
            return new WriteableOperationReadOnlyList(this);
        }
    }
}
