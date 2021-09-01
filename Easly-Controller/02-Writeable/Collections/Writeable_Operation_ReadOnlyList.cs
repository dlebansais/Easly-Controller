#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Read-only list of IxxxOperation
    /// </summary>
    public class WriteableOperationReadOnlyList : ReadOnlyCollection<WriteableOperation>
    {
        public WriteableOperationReadOnlyList(WriteableOperationList list)
            : base(list)
        {
        }
    }
}
