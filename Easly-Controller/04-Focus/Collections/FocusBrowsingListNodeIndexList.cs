namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class FocusBrowsingListNodeIndexList : FrameBrowsingListNodeIndexList, ICollection<IFocusBrowsingListNodeIndex>, IEnumerable<IFocusBrowsingListNodeIndex>, IList<IFocusBrowsingListNodeIndex>, IReadOnlyCollection<IFocusBrowsingListNodeIndex>, IReadOnlyList<IFocusBrowsingListNodeIndex>
    {
        /// <inheritdoc/>
        public new IFocusBrowsingListNodeIndex this[int index] { get { return (IFocusBrowsingListNodeIndex)base[index]; } set { base[index] = value; } }

        #region IFocusBrowsingListNodeIndex
        void ICollection<IFocusBrowsingListNodeIndex>.Add(IFocusBrowsingListNodeIndex item) { Add(item); }
        bool ICollection<IFocusBrowsingListNodeIndex>.Contains(IFocusBrowsingListNodeIndex item) { return Contains(item); }
        void ICollection<IFocusBrowsingListNodeIndex>.CopyTo(IFocusBrowsingListNodeIndex[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<IFocusBrowsingListNodeIndex>.Remove(IFocusBrowsingListNodeIndex item) { return Remove(item); }
        bool ICollection<IFocusBrowsingListNodeIndex>.IsReadOnly { get { return ((ICollection<IFrameBrowsingListNodeIndex>)this).IsReadOnly; } }
        IEnumerator<IFocusBrowsingListNodeIndex> IEnumerable<IFocusBrowsingListNodeIndex>.GetEnumerator() { var iterator = ((List<IReadOnlyBrowsingListNodeIndex>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IFocusBrowsingListNodeIndex)iterator.Current; } }
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
