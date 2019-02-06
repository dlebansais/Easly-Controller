#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;

    /// <summary>
    /// List of IxxxIndexCollection
    /// </summary>
    internal interface IWriteableIndexCollectionList : IReadOnlyIndexCollectionList, IList<IWriteableIndexCollection>, IReadOnlyList<IWriteableIndexCollection>
    {
        new IWriteableIndexCollection this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<IWriteableIndexCollection> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxIndexCollection
    /// </summary>
    internal class WriteableIndexCollectionList : Collection<IWriteableIndexCollection>, IWriteableIndexCollectionList
    {
        #region ReadOnly
        IReadOnlyIndexCollection IReadOnlyIndexCollectionList.this[int index] { get { return this[index]; } set { this[index] = (IWriteableIndexCollection)value; } }
        IReadOnlyIndexCollection IList<IReadOnlyIndexCollection>.this[int index] { get { return this[index]; } set { this[index] = (IWriteableIndexCollection)value; } }
        int IList<IReadOnlyIndexCollection>.IndexOf(IReadOnlyIndexCollection value) { return IndexOf((IWriteableIndexCollection)value); }
        void IList<IReadOnlyIndexCollection>.Insert(int index, IReadOnlyIndexCollection item) { Insert(index, (IWriteableIndexCollection)item); }
        void ICollection<IReadOnlyIndexCollection>.Add(IReadOnlyIndexCollection item) { Add((IWriteableIndexCollection)item); }
        bool ICollection<IReadOnlyIndexCollection>.Contains(IReadOnlyIndexCollection value) { return Contains((IWriteableIndexCollection)value); }
        void ICollection<IReadOnlyIndexCollection>.CopyTo(IReadOnlyIndexCollection[] array, int index) { CopyTo((IWriteableIndexCollection[])array, index); }
        bool ICollection<IReadOnlyIndexCollection>.IsReadOnly { get { return ((ICollection<IWriteableIndexCollection>)this).IsReadOnly; } }
        bool ICollection<IReadOnlyIndexCollection>.Remove(IReadOnlyIndexCollection item) { return Remove((IWriteableIndexCollection)item); }
        IEnumerator<IReadOnlyIndexCollection> IEnumerable<IReadOnlyIndexCollection>.GetEnumerator() { return GetEnumerator(); }
        IReadOnlyIndexCollection IReadOnlyList<IReadOnlyIndexCollection>.this[int index] { get { return this[index]; } }
        #endregion

        public virtual IReadOnlyIndexCollectionReadOnlyList ToReadOnly()
        {
            return new WriteableIndexCollectionReadOnlyList(this);
        }
    }
}
