namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class LayoutOperationList : FocusOperationList, ICollection<LayoutOperation>, IEnumerable<LayoutOperation>, IList<LayoutOperation>, IReadOnlyCollection<LayoutOperation>, IReadOnlyList<LayoutOperation>
    {
        /// <inheritdoc/>
        public new LayoutOperation this[int index] { get { return (LayoutOperation)base[index]; } set { base[index] = value; } }

        #region LayoutOperation
        void ICollection<LayoutOperation>.Add(LayoutOperation item) { Add(item); }
        bool ICollection<LayoutOperation>.Contains(LayoutOperation item) { return Contains(item); }
        void ICollection<LayoutOperation>.CopyTo(LayoutOperation[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<LayoutOperation>.Remove(LayoutOperation item) { return Remove(item); }
        bool ICollection<LayoutOperation>.IsReadOnly { get { return ((ICollection<FocusOperation>)this).IsReadOnly; } }
        IEnumerator<LayoutOperation> IEnumerable<LayoutOperation>.GetEnumerator() { return ((IList<LayoutOperation>)this).GetEnumerator(); }
        LayoutOperation IList<LayoutOperation>.this[int index] { get { return (LayoutOperation)this[index]; } set { this[index] = value; } }
        int IList<LayoutOperation>.IndexOf(LayoutOperation item) { return IndexOf(item); }
        void IList<LayoutOperation>.Insert(int index, LayoutOperation item) { Insert(index, item); }
        LayoutOperation IReadOnlyList<LayoutOperation>.this[int index] { get { return (LayoutOperation)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override WriteableOperationReadOnlyList ToReadOnly()
        {
            return new LayoutOperationReadOnlyList(this);
        }
    }
}
