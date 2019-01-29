using System.Collections.Generic;
using System.Collections.ObjectModel;

#pragma warning disable 1591

namespace EaslyController.Writeable
{
    /// <summary>
    /// List of IxxxOperationGroup
    /// </summary>
    public interface IWriteableOperationGroupList : IList<IWriteableOperationGroup>, IReadOnlyList<IWriteableOperationGroup>
    {
        new int Count { get; }
        new IWriteableOperationGroup this[int index] { get; set; }
        new IEnumerator<IWriteableOperationGroup> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxOperationGroup
    /// </summary>
    public class WriteableOperationGroupList : Collection<IWriteableOperationGroup>, IWriteableOperationGroupList
    {
    }
}
