namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class FocusIndexCollectionList : FrameIndexCollectionList, ICollection<IFocusIndexCollection>, IEnumerable<IFocusIndexCollection>, IList<IFocusIndexCollection>, IReadOnlyCollection<IFocusIndexCollection>, IReadOnlyList<IFocusIndexCollection>
    {
        /// <inheritdoc/>
        public new IFocusIndexCollection this[int index] { get { return (IFocusIndexCollection)base[index]; } set { base[index] = value; } }

        #region IFocusIndexCollection
        void ICollection<IFocusIndexCollection>.Add(IFocusIndexCollection item) { Add(item); }
        bool ICollection<IFocusIndexCollection>.Contains(IFocusIndexCollection item) { return Contains(item); }
        void ICollection<IFocusIndexCollection>.CopyTo(IFocusIndexCollection[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<IFocusIndexCollection>.Remove(IFocusIndexCollection item) { return Remove(item); }
        bool ICollection<IFocusIndexCollection>.IsReadOnly { get { return ((ICollection<IFrameIndexCollection>)this).IsReadOnly; } }
        IEnumerator<IFocusIndexCollection> IEnumerable<IFocusIndexCollection>.GetEnumerator() { return ((IList<IFocusIndexCollection>)this).GetEnumerator(); }
        IFocusIndexCollection IList<IFocusIndexCollection>.this[int index] { get { return (IFocusIndexCollection)this[index]; } set { this[index] = value; } }
        int IList<IFocusIndexCollection>.IndexOf(IFocusIndexCollection item) { return IndexOf(item); }
        void IList<IFocusIndexCollection>.Insert(int index, IFocusIndexCollection item) { Insert(index, item); }
        IFocusIndexCollection IReadOnlyList<IFocusIndexCollection>.this[int index] { get { return (IFocusIndexCollection)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyIndexCollectionReadOnlyList ToReadOnly()
        {
            return new FocusIndexCollectionReadOnlyList(this);
        }
    }
}
