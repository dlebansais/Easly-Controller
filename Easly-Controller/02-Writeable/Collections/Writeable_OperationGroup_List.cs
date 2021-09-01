#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// List of IxxxOperationGroup
    /// </summary>
    public class WriteableOperationGroupList : List<WriteableOperationGroup>
    {
        public virtual WriteableOperationGroupReadOnlyList ToReadOnly()
        {
            return new WriteableOperationGroupReadOnlyList(this);
        }
    }
}
