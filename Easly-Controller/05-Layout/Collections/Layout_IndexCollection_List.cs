namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;
    using EaslyController.Focus;

    /// <inheritdoc/>
    public class LayoutIndexCollectionList : FocusIndexCollectionList, ICollection<ILayoutIndexCollection>, IEnumerable<ILayoutIndexCollection>, IList<ILayoutIndexCollection>, IReadOnlyCollection<ILayoutIndexCollection>, IReadOnlyList<ILayoutIndexCollection>
    {
        #region ILayoutIndexCollection
        void ICollection<ILayoutIndexCollection>.Add(ILayoutIndexCollection item) { Add(item); }
        bool ICollection<ILayoutIndexCollection>.Contains(ILayoutIndexCollection item) { return Contains(item); }
        void ICollection<ILayoutIndexCollection>.CopyTo(ILayoutIndexCollection[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<ILayoutIndexCollection>.Remove(ILayoutIndexCollection item) { return Remove(item); }
        bool ICollection<ILayoutIndexCollection>.IsReadOnly { get { return ((ICollection<IFocusIndexCollection>)this).IsReadOnly; } }
        IEnumerator<ILayoutIndexCollection> IEnumerable<ILayoutIndexCollection>.GetEnumerator() { return ((IList<ILayoutIndexCollection>)this).GetEnumerator(); }
        ILayoutIndexCollection IList<ILayoutIndexCollection>.this[int index] { get { return (ILayoutIndexCollection)this[index]; } set { this[index] = value; } }
        int IList<ILayoutIndexCollection>.IndexOf(ILayoutIndexCollection item) { return IndexOf(item); }
        void IList<ILayoutIndexCollection>.Insert(int index, ILayoutIndexCollection item) { Insert(index, item); }
        ILayoutIndexCollection IReadOnlyList<ILayoutIndexCollection>.this[int index] { get { return (ILayoutIndexCollection)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyIndexCollectionReadOnlyList ToReadOnly()
        {
            return new LayoutIndexCollectionReadOnlyList(this);
        }
    }
}
