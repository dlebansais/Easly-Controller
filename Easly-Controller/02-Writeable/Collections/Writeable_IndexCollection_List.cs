namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class WriteableIndexCollectionList : ReadOnlyIndexCollectionList, ICollection<IWriteableIndexCollection>, IEnumerable<IWriteableIndexCollection>, IList<IWriteableIndexCollection>, IReadOnlyCollection<IWriteableIndexCollection>, IReadOnlyList<IWriteableIndexCollection>
    {
        /// <inheritdoc/>
        public new IWriteableIndexCollection this[int index] { get { return (IWriteableIndexCollection)base[index]; } set { base[index] = value; } }

        #region IWriteableIndexCollection
        void ICollection<IWriteableIndexCollection>.Add(IWriteableIndexCollection item) { Add(item); }
        bool ICollection<IWriteableIndexCollection>.Contains(IWriteableIndexCollection item) { return Contains(item); }
        void ICollection<IWriteableIndexCollection>.CopyTo(IWriteableIndexCollection[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<IWriteableIndexCollection>.Remove(IWriteableIndexCollection item) { return Remove(item); }
        bool ICollection<IWriteableIndexCollection>.IsReadOnly { get { return ((ICollection<IReadOnlyIndexCollection>)this).IsReadOnly; } }
        IEnumerator<IWriteableIndexCollection> IEnumerable<IWriteableIndexCollection>.GetEnumerator() { return ((IList<IWriteableIndexCollection>)this).GetEnumerator(); }
        IWriteableIndexCollection IList<IWriteableIndexCollection>.this[int index] { get { return (IWriteableIndexCollection)this[index]; } set { this[index] = value; } }
        int IList<IWriteableIndexCollection>.IndexOf(IWriteableIndexCollection item) { return IndexOf(item); }
        void IList<IWriteableIndexCollection>.Insert(int index, IWriteableIndexCollection item) { Insert(index, item); }
        IWriteableIndexCollection IReadOnlyList<IWriteableIndexCollection>.this[int index] { get { return (IWriteableIndexCollection)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyIndexCollectionReadOnlyList ToReadOnly()
        {
            return new WriteableIndexCollectionReadOnlyList(this);
        }
    }
}
