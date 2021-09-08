namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FrameBrowsingListNodeIndexList : WriteableBrowsingListNodeIndexList, ICollection<IFrameBrowsingListNodeIndex>, IEnumerable<IFrameBrowsingListNodeIndex>, IList<IFrameBrowsingListNodeIndex>, IReadOnlyCollection<IFrameBrowsingListNodeIndex>, IReadOnlyList<IFrameBrowsingListNodeIndex>
    {
        /// <inheritdoc/>
        public new IFrameBrowsingListNodeIndex this[int index] { get { return (IFrameBrowsingListNodeIndex)base[index]; } set { base[index] = value; } }

        #region IFrameBrowsingListNodeIndex
        void ICollection<IFrameBrowsingListNodeIndex>.Add(IFrameBrowsingListNodeIndex item) { Add(item); }
        bool ICollection<IFrameBrowsingListNodeIndex>.Contains(IFrameBrowsingListNodeIndex item) { return Contains(item); }
        void ICollection<IFrameBrowsingListNodeIndex>.CopyTo(IFrameBrowsingListNodeIndex[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<IFrameBrowsingListNodeIndex>.Remove(IFrameBrowsingListNodeIndex item) { return Remove(item); }
        bool ICollection<IFrameBrowsingListNodeIndex>.IsReadOnly { get { return ((ICollection<IWriteableBrowsingListNodeIndex>)this).IsReadOnly; } }
        IEnumerator<IFrameBrowsingListNodeIndex> IEnumerable<IFrameBrowsingListNodeIndex>.GetEnumerator() { return ((IList<IFrameBrowsingListNodeIndex>)this).GetEnumerator(); }
        IFrameBrowsingListNodeIndex IList<IFrameBrowsingListNodeIndex>.this[int index] { get { return (IFrameBrowsingListNodeIndex)this[index]; } set { this[index] = value; } }
        int IList<IFrameBrowsingListNodeIndex>.IndexOf(IFrameBrowsingListNodeIndex item) { return IndexOf(item); }
        void IList<IFrameBrowsingListNodeIndex>.Insert(int index, IFrameBrowsingListNodeIndex item) { Insert(index, item); }
        IFrameBrowsingListNodeIndex IReadOnlyList<IFrameBrowsingListNodeIndex>.this[int index] { get { return (IFrameBrowsingListNodeIndex)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyBrowsingListNodeIndexReadOnlyList ToReadOnly()
        {
            return new FrameBrowsingListNodeIndexReadOnlyList(this);
        }
    }
}
