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
        public new IReadOnlyIndexCollection this[int index] { get { return base[index]; } }
        public bool Contains(IReadOnlyIndexCollection value) { return base.Contains((IWriteableIndexCollection)value); }
        public int IndexOf(IReadOnlyIndexCollection value) { return base.IndexOf((IWriteableIndexCollection)value); }
        public new IEnumerator<IReadOnlyIndexCollection> GetEnumerator() { return base.GetEnumerator(); }
        #endregion
    }
}
