#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;

    /// <summary>
    /// List of IxxxIndexCollection
    /// </summary>
    internal class WriteableIndexCollectionList : ReadOnlyIndexCollectionList, ICollection<IWriteableIndexCollection>, IEnumerable<IWriteableIndexCollection>, IList<IWriteableIndexCollection>, IReadOnlyCollection<IWriteableIndexCollection>, IReadOnlyList<IWriteableIndexCollection>
    {
        #region IWriteableIndexCollection
        void ICollection<IWriteableIndexCollection>.Add(IWriteableIndexCollection item) { Add(item); }
        bool ICollection<IWriteableIndexCollection>.Contains(IWriteableIndexCollection item) { return Contains(item); }
        void ICollection<IWriteableIndexCollection>.CopyTo(IWriteableIndexCollection[] array, int arrayIndex) { for (int i = 0; i < Count; i++) array[arrayIndex + i] = (IWriteableIndexCollection)this[i]; }
        bool ICollection<IWriteableIndexCollection>.Remove(IWriteableIndexCollection item) { return Remove(item); }
        bool ICollection<IWriteableIndexCollection>.IsReadOnly { get { return false; } }
        IEnumerator<IWriteableIndexCollection> IEnumerable<IWriteableIndexCollection>.GetEnumerator() { return new List<IWriteableIndexCollection>(this).GetEnumerator(); }
        IWriteableIndexCollection IList<IWriteableIndexCollection>.this[int index] { get { return (IWriteableIndexCollection)this[index]; } set { this[index] = value; } }
        int IList<IWriteableIndexCollection>.IndexOf(IWriteableIndexCollection item) { return IndexOf(item); }
        void IList<IWriteableIndexCollection>.Insert(int index, IWriteableIndexCollection item) { Insert(index, item); }
        IWriteableIndexCollection IReadOnlyList<IWriteableIndexCollection>.this[int index] { get { return (IWriteableIndexCollection)this[index]; } }
        #endregion

        public override ReadOnlyIndexCollectionReadOnlyList ToReadOnly()
        {
            return new WriteableIndexCollectionReadOnlyList(this);
        }
    }
}
