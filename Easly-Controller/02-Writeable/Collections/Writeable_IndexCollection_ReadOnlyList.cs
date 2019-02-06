#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Read-only list of IxxxIndexCollection
    /// </summary>
    internal interface IWriteableIndexCollectionReadOnlyList : IReadOnlyIndexCollectionReadOnlyList, IReadOnlyList<IWriteableIndexCollection>
    {
        new int Count { get; }
        new IWriteableIndexCollection this[int index] { get; }
        bool Contains(IWriteableIndexCollection value);
        int IndexOf(IWriteableIndexCollection value);
        new IEnumerator<IWriteableIndexCollection> GetEnumerator();
    }

    /// <summary>
    /// Read-only list of IxxxIndexCollection
    /// </summary>
    internal class WriteableIndexCollectionReadOnlyList : ReadOnlyCollection<IWriteableIndexCollection>, IWriteableIndexCollectionReadOnlyList
    {
        public WriteableIndexCollectionReadOnlyList(IWriteableIndexCollectionList list)
            : base(list)
        {
        }

        #region ReadOnly
        IReadOnlyIndexCollection IReadOnlyList<IReadOnlyIndexCollection>.this[int index] { get { return this[index]; } }
        bool IReadOnlyIndexCollectionReadOnlyList.Contains(IReadOnlyIndexCollection value) { return Contains((IWriteableIndexCollection)value); }
        int IReadOnlyIndexCollectionReadOnlyList.IndexOf(IReadOnlyIndexCollection value) { return IndexOf((IWriteableIndexCollection)value); }
        IEnumerator<IReadOnlyIndexCollection> IEnumerable<IReadOnlyIndexCollection>.GetEnumerator() { return GetEnumerator(); }
        #endregion
    }
}
