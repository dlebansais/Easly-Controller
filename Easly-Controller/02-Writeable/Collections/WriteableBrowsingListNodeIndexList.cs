namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class WriteableBrowsingListNodeIndexList : ReadOnlyBrowsingListNodeIndexList, ICollection<IWriteableBrowsingListNodeIndex>, IEnumerable<IWriteableBrowsingListNodeIndex>, IList<IWriteableBrowsingListNodeIndex>, IReadOnlyCollection<IWriteableBrowsingListNodeIndex>, IReadOnlyList<IWriteableBrowsingListNodeIndex>
    {
        /// <inheritdoc/>
        public new IWriteableBrowsingListNodeIndex this[int index] { get { return (IWriteableBrowsingListNodeIndex)base[index]; } set { base[index] = value; } }

        #region IWriteableBrowsingListNodeIndex
        void ICollection<IWriteableBrowsingListNodeIndex>.Add(IWriteableBrowsingListNodeIndex item) { Add(item); }
        bool ICollection<IWriteableBrowsingListNodeIndex>.Contains(IWriteableBrowsingListNodeIndex item) { return Contains(item); }
        void ICollection<IWriteableBrowsingListNodeIndex>.CopyTo(IWriteableBrowsingListNodeIndex[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<IWriteableBrowsingListNodeIndex>.Remove(IWriteableBrowsingListNodeIndex item) { return Remove(item); }
        bool ICollection<IWriteableBrowsingListNodeIndex>.IsReadOnly { get { return ((ICollection<IReadOnlyBrowsingListNodeIndex>)this).IsReadOnly; } }
        IEnumerator<IWriteableBrowsingListNodeIndex> IEnumerable<IWriteableBrowsingListNodeIndex>.GetEnumerator() { var iterator = ((List<IReadOnlyBrowsingListNodeIndex>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IWriteableBrowsingListNodeIndex)iterator.Current; } }
        IWriteableBrowsingListNodeIndex IList<IWriteableBrowsingListNodeIndex>.this[int index] { get { return (IWriteableBrowsingListNodeIndex)this[index]; } set { this[index] = value; } }
        int IList<IWriteableBrowsingListNodeIndex>.IndexOf(IWriteableBrowsingListNodeIndex item) { return IndexOf(item); }
        void IList<IWriteableBrowsingListNodeIndex>.Insert(int index, IWriteableBrowsingListNodeIndex item) { Insert(index, item); }
        IWriteableBrowsingListNodeIndex IReadOnlyList<IWriteableBrowsingListNodeIndex>.this[int index] { get { return (IWriteableBrowsingListNodeIndex)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyBrowsingListNodeIndexReadOnlyList ToReadOnly()
        {
            return new WriteableBrowsingListNodeIndexReadOnlyList(this);
        }
    }
}
