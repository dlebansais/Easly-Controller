#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

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
    internal class WriteableOperationReadOnlyList : ReadOnlyCollection<IWriteableOperation>, IWriteableOperationReadOnlyList
    {
        public WriteableOperationReadOnlyList(IWriteableOperationList list)
            : base(list)
        {
        }
    }
}
