#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Read-only list of IxxxOperationGroup
    /// </summary>
    public interface IWriteableOperationGroupReadOnlyList : IReadOnlyList<IWriteableOperationGroup>
    {
        bool Contains(IWriteableOperationGroup value);
        int IndexOf(IWriteableOperationGroup value);
    }

    /// <summary>
    /// Read-only list of IxxxOperationGroup
    /// </summary>
    public class WriteableOperationGroupReadOnlyList : ReadOnlyCollection<IWriteableOperationGroup>, IWriteableOperationGroupReadOnlyList
    {
        public WriteableOperationGroupReadOnlyList(IWriteableOperationGroupList list)
            : base(list)
        {
        }
    }
}
