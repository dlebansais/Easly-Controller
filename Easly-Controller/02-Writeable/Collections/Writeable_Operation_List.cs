#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// List of IxxxOperation
    /// </summary>
    public interface IWriteableOperationList : IList<IWriteableOperation>, IReadOnlyList<IWriteableOperation>
    {
        new IWriteableOperation this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<IWriteableOperation> GetEnumerator();
        IWriteableOperationReadOnlyList ToReadOnly();
    }

    /// <summary>
    /// List of IxxxOperation
    /// </summary>
    internal class WriteableOperationList : Collection<IWriteableOperation>, IWriteableOperationList
    {
        public virtual IWriteableOperationReadOnlyList ToReadOnly()
        {
            return new WriteableOperationReadOnlyList(this);
        }
    }
}
