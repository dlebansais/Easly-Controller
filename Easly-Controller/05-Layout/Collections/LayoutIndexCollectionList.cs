namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class LayoutIndexCollectionList : FocusIndexCollectionList, ICollection<ILayoutIndexCollection>, IEnumerable<ILayoutIndexCollection>, IList<ILayoutIndexCollection>, IReadOnlyCollection<ILayoutIndexCollection>, IReadOnlyList<ILayoutIndexCollection>
    {
        /// <inheritdoc/>
        public new ILayoutIndexCollection this[int index] { get { return (ILayoutIndexCollection)base[index]; } set { base[index] = value; } }

        #region ILayoutIndexCollection
        void ICollection<ILayoutIndexCollection>.Add(ILayoutIndexCollection item) { Add(item); }
        bool ICollection<ILayoutIndexCollection>.Contains(ILayoutIndexCollection item) { return Contains(item); }
        void ICollection<ILayoutIndexCollection>.CopyTo(ILayoutIndexCollection[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<ILayoutIndexCollection>.Remove(ILayoutIndexCollection item) { return Remove(item); }
        bool ICollection<ILayoutIndexCollection>.IsReadOnly { get { return ((ICollection<IFocusIndexCollection>)this).IsReadOnly; } }
        IEnumerator<ILayoutIndexCollection> IEnumerable<ILayoutIndexCollection>.GetEnumerator() { var iterator = ((List<IReadOnlyIndexCollection>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutIndexCollection)iterator.Current; } }
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
