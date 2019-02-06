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
        new IWriteableIndexCollection this[int index] { get; }
        new int Count { get; }
        bool Contains(IWriteableIndexCollection value);
        new IEnumerator<IWriteableIndexCollection> GetEnumerator();
        int IndexOf(IWriteableIndexCollection value);
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
        bool IReadOnlyIndexCollectionReadOnlyList.Contains(IReadOnlyIndexCollection value) { return Contains((IWriteableIndexCollection)value); }
        int IReadOnlyIndexCollectionReadOnlyList.IndexOf(IReadOnlyIndexCollection value) { return IndexOf((IWriteableIndexCollection)value); }
        IEnumerator<IReadOnlyIndexCollection> IEnumerable<IReadOnlyIndexCollection>.GetEnumerator() { return GetEnumerator(); }
        IReadOnlyIndexCollection IReadOnlyList<IReadOnlyIndexCollection>.this[int index] { get { return this[index]; } }
        #endregion
    }
}
