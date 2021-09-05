namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class FocusBrowsingListNodeIndexList : FrameBrowsingListNodeIndexList, ICollection<IFocusBrowsingListNodeIndex>, IEnumerable<IFocusBrowsingListNodeIndex>, IList<IFocusBrowsingListNodeIndex>, IReadOnlyCollection<IFocusBrowsingListNodeIndex>, IReadOnlyList<IFocusBrowsingListNodeIndex>
    {
        #region IFocusBrowsingListNodeIndex
        void ICollection<IFocusBrowsingListNodeIndex>.Add(IFocusBrowsingListNodeIndex item) { Add(item); }
        bool ICollection<IFocusBrowsingListNodeIndex>.Contains(IFocusBrowsingListNodeIndex item) { return Contains(item); }
        void ICollection<IFocusBrowsingListNodeIndex>.CopyTo(IFocusBrowsingListNodeIndex[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<IFocusBrowsingListNodeIndex>.Remove(IFocusBrowsingListNodeIndex item) { return Remove(item); }
        bool ICollection<IFocusBrowsingListNodeIndex>.IsReadOnly { get { return ((ICollection<IFrameBrowsingListNodeIndex>)this).IsReadOnly; } }
        IEnumerator<IFocusBrowsingListNodeIndex> IEnumerable<IFocusBrowsingListNodeIndex>.GetEnumerator() { return ((IList<IFocusBrowsingListNodeIndex>)this).GetEnumerator(); }
        IFocusBrowsingListNodeIndex IList<IFocusBrowsingListNodeIndex>.this[int index] { get { return (IFocusBrowsingListNodeIndex)this[index]; } set { this[index] = value; } }
        int IList<IFocusBrowsingListNodeIndex>.IndexOf(IFocusBrowsingListNodeIndex item) { return IndexOf(item); }
        void IList<IFocusBrowsingListNodeIndex>.Insert(int index, IFocusBrowsingListNodeIndex item) { Insert(index, item); }
        IFocusBrowsingListNodeIndex IReadOnlyList<IFocusBrowsingListNodeIndex>.this[int index] { get { return (IFocusBrowsingListNodeIndex)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyBrowsingListNodeIndexReadOnlyList ToReadOnly()
        {
            return new FocusBrowsingListNodeIndexReadOnlyList(this);
        }
    }
}
