namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FrameBrowsingBlockNodeIndexList : WriteableBrowsingBlockNodeIndexList, ICollection<IFrameBrowsingBlockNodeIndex>, IEnumerable<IFrameBrowsingBlockNodeIndex>, IList<IFrameBrowsingBlockNodeIndex>, IReadOnlyCollection<IFrameBrowsingBlockNodeIndex>, IReadOnlyList<IFrameBrowsingBlockNodeIndex>
    {
        /// <inheritdoc/>
        public new IFrameBrowsingBlockNodeIndex this[int index] { get { return (IFrameBrowsingBlockNodeIndex)base[index]; } set { base[index] = value; } }

        #region IFrameBrowsingBlockNodeIndex
        void ICollection<IFrameBrowsingBlockNodeIndex>.Add(IFrameBrowsingBlockNodeIndex item) { Add(item); }
        bool ICollection<IFrameBrowsingBlockNodeIndex>.Contains(IFrameBrowsingBlockNodeIndex item) { return Contains(item); }
        void ICollection<IFrameBrowsingBlockNodeIndex>.CopyTo(IFrameBrowsingBlockNodeIndex[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<IFrameBrowsingBlockNodeIndex>.Remove(IFrameBrowsingBlockNodeIndex item) { return Remove(item); }
        bool ICollection<IFrameBrowsingBlockNodeIndex>.IsReadOnly { get { return ((ICollection<IWriteableBrowsingBlockNodeIndex>)this).IsReadOnly; } }
        IEnumerator<IFrameBrowsingBlockNodeIndex> IEnumerable<IFrameBrowsingBlockNodeIndex>.GetEnumerator() { return ((IList<IFrameBrowsingBlockNodeIndex>)this).GetEnumerator(); }
        IFrameBrowsingBlockNodeIndex IList<IFrameBrowsingBlockNodeIndex>.this[int index] { get { return (IFrameBrowsingBlockNodeIndex)this[index]; } set { this[index] = value; } }
        int IList<IFrameBrowsingBlockNodeIndex>.IndexOf(IFrameBrowsingBlockNodeIndex item) { return IndexOf(item); }
        void IList<IFrameBrowsingBlockNodeIndex>.Insert(int index, IFrameBrowsingBlockNodeIndex item) { Insert(index, item); }
        IFrameBrowsingBlockNodeIndex IReadOnlyList<IFrameBrowsingBlockNodeIndex>.this[int index] { get { return (IFrameBrowsingBlockNodeIndex)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyBrowsingBlockNodeIndexReadOnlyList ToReadOnly()
        {
            return new FrameBrowsingBlockNodeIndexReadOnlyList(this);
        }
    }
}
