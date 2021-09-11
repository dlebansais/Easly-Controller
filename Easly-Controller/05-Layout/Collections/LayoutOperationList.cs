namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class LayoutOperationList : FocusOperationList, ICollection<ILayoutOperation>, IEnumerable<ILayoutOperation>, IList<ILayoutOperation>, IReadOnlyCollection<ILayoutOperation>, IReadOnlyList<ILayoutOperation>
    {
        /// <inheritdoc/>
        public new ILayoutOperation this[int index] { get { return (ILayoutOperation)base[index]; } set { base[index] = value; } }

        #region ILayoutOperation
        void ICollection<ILayoutOperation>.Add(ILayoutOperation item) { Add(item); }
        bool ICollection<ILayoutOperation>.Contains(ILayoutOperation item) { return Contains(item); }
        void ICollection<ILayoutOperation>.CopyTo(ILayoutOperation[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<ILayoutOperation>.Remove(ILayoutOperation item) { return Remove(item); }
        bool ICollection<ILayoutOperation>.IsReadOnly { get { return ((ICollection<IFocusOperation>)this).IsReadOnly; } }
        IEnumerator<ILayoutOperation> IEnumerable<ILayoutOperation>.GetEnumerator() { Enumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutOperation)iterator.Current; } }
        ILayoutOperation IList<ILayoutOperation>.this[int index] { get { return (ILayoutOperation)this[index]; } set { this[index] = value; } }
        int IList<ILayoutOperation>.IndexOf(ILayoutOperation item) { return IndexOf(item); }
        void IList<ILayoutOperation>.Insert(int index, ILayoutOperation item) { Insert(index, item); }
        ILayoutOperation IReadOnlyList<ILayoutOperation>.this[int index] { get { return (ILayoutOperation)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override WriteableOperationReadOnlyList ToReadOnly()
        {
            return new LayoutOperationReadOnlyList(this);
        }
    }
}
