namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;
    using EaslyController.Focus;

    /// <inheritdoc/>
    public class LayoutBrowsingBlockNodeIndexList : FocusBrowsingBlockNodeIndexList, ICollection<ILayoutBrowsingBlockNodeIndex>, IEnumerable<ILayoutBrowsingBlockNodeIndex>, IList<ILayoutBrowsingBlockNodeIndex>, IReadOnlyCollection<ILayoutBrowsingBlockNodeIndex>, IReadOnlyList<ILayoutBrowsingBlockNodeIndex>
    {
        #region ILayoutBrowsingBlockNodeIndex
        void ICollection<ILayoutBrowsingBlockNodeIndex>.Add(ILayoutBrowsingBlockNodeIndex item) { Add(item); }
        bool ICollection<ILayoutBrowsingBlockNodeIndex>.Contains(ILayoutBrowsingBlockNodeIndex item) { return Contains(item); }
        void ICollection<ILayoutBrowsingBlockNodeIndex>.CopyTo(ILayoutBrowsingBlockNodeIndex[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<ILayoutBrowsingBlockNodeIndex>.Remove(ILayoutBrowsingBlockNodeIndex item) { return Remove(item); }
        bool ICollection<ILayoutBrowsingBlockNodeIndex>.IsReadOnly { get { return ((ICollection<IFocusBrowsingBlockNodeIndex>)this).IsReadOnly; } }
        IEnumerator<ILayoutBrowsingBlockNodeIndex> IEnumerable<ILayoutBrowsingBlockNodeIndex>.GetEnumerator() { return ((IList<ILayoutBrowsingBlockNodeIndex>)this).GetEnumerator(); }
        ILayoutBrowsingBlockNodeIndex IList<ILayoutBrowsingBlockNodeIndex>.this[int index] { get { return (ILayoutBrowsingBlockNodeIndex)this[index]; } set { this[index] = value; } }
        int IList<ILayoutBrowsingBlockNodeIndex>.IndexOf(ILayoutBrowsingBlockNodeIndex item) { return IndexOf(item); }
        void IList<ILayoutBrowsingBlockNodeIndex>.Insert(int index, ILayoutBrowsingBlockNodeIndex item) { Insert(index, item); }
        ILayoutBrowsingBlockNodeIndex IReadOnlyList<ILayoutBrowsingBlockNodeIndex>.this[int index] { get { return (ILayoutBrowsingBlockNodeIndex)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyBrowsingBlockNodeIndexReadOnlyList ToReadOnly()
        {
            return new LayoutBrowsingBlockNodeIndexReadOnlyList(this);
        }
    }
}
