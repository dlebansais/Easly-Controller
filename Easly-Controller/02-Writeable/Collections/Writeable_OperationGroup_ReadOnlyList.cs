#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Read-only list of IxxxOperationGroup
    /// </summary>
    public class WriteableOperationGroupReadOnlyList : ReadOnlyCollection<WriteableOperationGroup>
    {
        public WriteableOperationGroupReadOnlyList(WriteableOperationGroupList list)
            : base(list)
        {
        }
    }
}
