using System.Collections.Generic;
using System.Collections.ObjectModel;

#pragma warning disable 1591

namespace EaslyController.Writeable
{
    /// <summary>
    /// Read-only list of IxxxOperation
    /// </summary>
    public interface IWriteableOperationReadOnlyList : IReadOnlyList<IWriteableOperation>
    {
        bool Contains(IWriteableOperation value);
        int IndexOf(IWriteableOperation value);
    }

    /// <summary>
    /// Read-only list of IxxxOperation
    /// </summary>
    public class WriteableOperationReadOnlyList : ReadOnlyCollection<IWriteableOperation>, IWriteableOperationReadOnlyList
    {
        public WriteableOperationReadOnlyList(IWriteableOperationList list)
            : base(list)
        {
        }
    }
}
