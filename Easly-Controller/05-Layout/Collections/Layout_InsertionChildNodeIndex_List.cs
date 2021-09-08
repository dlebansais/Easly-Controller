namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;

    /// <inheritdoc/>
    public class LayoutInsertionChildNodeIndexList : FocusInsertionChildNodeIndexList, ICollection<ILayoutInsertionChildNodeIndex>, IEnumerable<ILayoutInsertionChildNodeIndex>, IList<ILayoutInsertionChildNodeIndex>, IReadOnlyCollection<ILayoutInsertionChildNodeIndex>, IReadOnlyList<ILayoutInsertionChildNodeIndex>
    {
        /// <inheritdoc/>
        public new ILayoutInsertionChildNodeIndex this[int index] { get { return (ILayoutInsertionChildNodeIndex)base[index]; } set { base[index] = value; } }

        #region ILayoutInsertionChildNodeIndex
        void ICollection<ILayoutInsertionChildNodeIndex>.Add(ILayoutInsertionChildNodeIndex item) { Add(item); }
        bool ICollection<ILayoutInsertionChildNodeIndex>.Contains(ILayoutInsertionChildNodeIndex item) { return Contains(item); }
        void ICollection<ILayoutInsertionChildNodeIndex>.CopyTo(ILayoutInsertionChildNodeIndex[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<ILayoutInsertionChildNodeIndex>.Remove(ILayoutInsertionChildNodeIndex item) { return Remove(item); }
        bool ICollection<ILayoutInsertionChildNodeIndex>.IsReadOnly { get { return ((ICollection<IFocusInsertionChildNodeIndex>)this).IsReadOnly; } }
        IEnumerator<ILayoutInsertionChildNodeIndex> IEnumerable<ILayoutInsertionChildNodeIndex>.GetEnumerator() { return ((IList<ILayoutInsertionChildNodeIndex>)this).GetEnumerator(); }
        ILayoutInsertionChildNodeIndex IList<ILayoutInsertionChildNodeIndex>.this[int index] { get { return (ILayoutInsertionChildNodeIndex)this[index]; } set { this[index] = value; } }
        int IList<ILayoutInsertionChildNodeIndex>.IndexOf(ILayoutInsertionChildNodeIndex item) { return IndexOf(item); }
        void IList<ILayoutInsertionChildNodeIndex>.Insert(int index, ILayoutInsertionChildNodeIndex item) { Insert(index, item); }
        ILayoutInsertionChildNodeIndex IReadOnlyList<ILayoutInsertionChildNodeIndex>.this[int index] { get { return (ILayoutInsertionChildNodeIndex)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override FocusInsertionChildNodeIndexReadOnlyList ToReadOnly()
        {
            return new LayoutInsertionChildNodeIndexReadOnlyList(this);
        }
    }
}
