namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class WriteableBrowsingBlockNodeIndexList : ReadOnlyBrowsingBlockNodeIndexList, ICollection<IWriteableBrowsingBlockNodeIndex>, IEnumerable<IWriteableBrowsingBlockNodeIndex>, IList<IWriteableBrowsingBlockNodeIndex>, IReadOnlyCollection<IWriteableBrowsingBlockNodeIndex>, IReadOnlyList<IWriteableBrowsingBlockNodeIndex>
    {
        /// <inheritdoc/>
        public new IWriteableBrowsingBlockNodeIndex this[int index] { get { return (IWriteableBrowsingBlockNodeIndex)base[index]; } set { base[index] = value; } }

        #region IWriteableBrowsingBlockNodeIndex
        void ICollection<IWriteableBrowsingBlockNodeIndex>.Add(IWriteableBrowsingBlockNodeIndex item) { Add(item); }
        bool ICollection<IWriteableBrowsingBlockNodeIndex>.Contains(IWriteableBrowsingBlockNodeIndex item) { return Contains(item); }
        void ICollection<IWriteableBrowsingBlockNodeIndex>.CopyTo(IWriteableBrowsingBlockNodeIndex[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<IWriteableBrowsingBlockNodeIndex>.Remove(IWriteableBrowsingBlockNodeIndex item) { return Remove(item); }
        bool ICollection<IWriteableBrowsingBlockNodeIndex>.IsReadOnly { get { return ((ICollection < IReadOnlyBrowsingBlockNodeIndex>)this).IsReadOnly; } }
        IEnumerator<IWriteableBrowsingBlockNodeIndex> IEnumerable<IWriteableBrowsingBlockNodeIndex>.GetEnumerator() { return ((IList < IWriteableBrowsingBlockNodeIndex>)this).GetEnumerator(); }
        IWriteableBrowsingBlockNodeIndex IList<IWriteableBrowsingBlockNodeIndex>.this[int index] { get { return (IWriteableBrowsingBlockNodeIndex)this[index]; } set { this[index] = value; } }
        int IList<IWriteableBrowsingBlockNodeIndex>.IndexOf(IWriteableBrowsingBlockNodeIndex item) { return IndexOf(item); }
        void IList<IWriteableBrowsingBlockNodeIndex>.Insert(int index, IWriteableBrowsingBlockNodeIndex item) { Insert(index, item); }
        IWriteableBrowsingBlockNodeIndex IReadOnlyList<IWriteableBrowsingBlockNodeIndex>.this[int index] { get { return (IWriteableBrowsingBlockNodeIndex)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyBrowsingBlockNodeIndexReadOnlyList ToReadOnly()
        {
            return new WriteableBrowsingBlockNodeIndexReadOnlyList(this);
        }
    }
}
