namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class LayoutBrowsingListNodeIndexList : FocusBrowsingListNodeIndexList, ICollection<ILayoutBrowsingListNodeIndex>, IEnumerable<ILayoutBrowsingListNodeIndex>, IList<ILayoutBrowsingListNodeIndex>, IReadOnlyCollection<ILayoutBrowsingListNodeIndex>, IReadOnlyList<ILayoutBrowsingListNodeIndex>
    {
        /// <inheritdoc/>
        public new ILayoutBrowsingListNodeIndex this[int index] { get { return (ILayoutBrowsingListNodeIndex)base[index]; } set { base[index] = value; } }

        #region ILayoutBrowsingListNodeIndex
        void ICollection<ILayoutBrowsingListNodeIndex>.Add(ILayoutBrowsingListNodeIndex item) { Add(item); }
        bool ICollection<ILayoutBrowsingListNodeIndex>.Contains(ILayoutBrowsingListNodeIndex item) { return Contains(item); }
        void ICollection<ILayoutBrowsingListNodeIndex>.CopyTo(ILayoutBrowsingListNodeIndex[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<ILayoutBrowsingListNodeIndex>.Remove(ILayoutBrowsingListNodeIndex item) { return Remove(item); }
        bool ICollection<ILayoutBrowsingListNodeIndex>.IsReadOnly { get { return ((ICollection<IFocusBrowsingListNodeIndex>)this).IsReadOnly; } }
        IEnumerator<ILayoutBrowsingListNodeIndex> IEnumerable<ILayoutBrowsingListNodeIndex>.GetEnumerator() { var iterator = ((List<IReadOnlyBrowsingListNodeIndex>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutBrowsingListNodeIndex)iterator.Current; } }
        ILayoutBrowsingListNodeIndex IList<ILayoutBrowsingListNodeIndex>.this[int index] { get { return (ILayoutBrowsingListNodeIndex)this[index]; } set { this[index] = value; } }
        int IList<ILayoutBrowsingListNodeIndex>.IndexOf(ILayoutBrowsingListNodeIndex item) { return IndexOf(item); }
        void IList<ILayoutBrowsingListNodeIndex>.Insert(int index, ILayoutBrowsingListNodeIndex item) { Insert(index, item); }
        ILayoutBrowsingListNodeIndex IReadOnlyList<ILayoutBrowsingListNodeIndex>.this[int index] { get { return (ILayoutBrowsingListNodeIndex)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyBrowsingListNodeIndexReadOnlyList ToReadOnly()
        {
            return new LayoutBrowsingListNodeIndexReadOnlyList(this);
        }
    }
}
