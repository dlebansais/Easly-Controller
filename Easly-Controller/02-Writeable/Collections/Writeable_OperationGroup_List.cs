#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// List of IxxxOperationGroup
    /// </summary>
    public interface IWriteableOperationGroupList : IList<IWriteableOperationGroup>, IReadOnlyList<IWriteableOperationGroup>
    {
        new IWriteableOperationGroup this[int index] { get; set; }
        new int Count { get; }
        IWriteableOperationGroupReadOnlyList ToReadOnly();
    }

    /// <summary>
    /// List of IxxxOperationGroup
    /// </summary>
    internal class WriteableOperationGroupList : Collection<IWriteableOperationGroup>, IWriteableOperationGroupList
    {
        public virtual IWriteableOperationGroupReadOnlyList ToReadOnly()
        {
            return new WriteableOperationGroupReadOnlyList(this);
        }
    }
}
