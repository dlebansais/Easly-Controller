namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class FocusBrowsingBlockNodeIndexList : FrameBrowsingBlockNodeIndexList, ICollection<IFocusBrowsingBlockNodeIndex>, IEnumerable<IFocusBrowsingBlockNodeIndex>, IList<IFocusBrowsingBlockNodeIndex>, IReadOnlyCollection<IFocusBrowsingBlockNodeIndex>, IReadOnlyList<IFocusBrowsingBlockNodeIndex>
    {
        /// <inheritdoc/>
        public new IFocusBrowsingBlockNodeIndex this[int index] { get { return (IFocusBrowsingBlockNodeIndex)base[index]; } set { base[index] = value; } }

        #region IFocusBrowsingBlockNodeIndex
        void ICollection<IFocusBrowsingBlockNodeIndex>.Add(IFocusBrowsingBlockNodeIndex item) { Add(item); }
        bool ICollection<IFocusBrowsingBlockNodeIndex>.Contains(IFocusBrowsingBlockNodeIndex item) { return Contains(item); }
        void ICollection<IFocusBrowsingBlockNodeIndex>.CopyTo(IFocusBrowsingBlockNodeIndex[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<IFocusBrowsingBlockNodeIndex>.Remove(IFocusBrowsingBlockNodeIndex item) { return Remove(item); }
        bool ICollection<IFocusBrowsingBlockNodeIndex>.IsReadOnly { get { return ((ICollection<IFrameBrowsingBlockNodeIndex>)this).IsReadOnly; } }
        IEnumerator<IFocusBrowsingBlockNodeIndex> IEnumerable<IFocusBrowsingBlockNodeIndex>.GetEnumerator() { return ((IList<IFocusBrowsingBlockNodeIndex>)this).GetEnumerator(); }
        IFocusBrowsingBlockNodeIndex IList<IFocusBrowsingBlockNodeIndex>.this[int index] { get { return (IFocusBrowsingBlockNodeIndex)this[index]; } set { this[index] = value; } }
        int IList<IFocusBrowsingBlockNodeIndex>.IndexOf(IFocusBrowsingBlockNodeIndex item) { return IndexOf(item); }
        void IList<IFocusBrowsingBlockNodeIndex>.Insert(int index, IFocusBrowsingBlockNodeIndex item) { Insert(index, item); }
        IFocusBrowsingBlockNodeIndex IReadOnlyList<IFocusBrowsingBlockNodeIndex>.this[int index] { get { return (IFocusBrowsingBlockNodeIndex)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyBrowsingBlockNodeIndexReadOnlyList ToReadOnly()
        {
            return new FocusBrowsingBlockNodeIndexReadOnlyList(this);
        }
    }
}
