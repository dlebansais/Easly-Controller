namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FocusOperationList : FrameOperationList, ICollection<IFocusOperation>, IEnumerable<IFocusOperation>, IList<IFocusOperation>, IReadOnlyCollection<IFocusOperation>, IReadOnlyList<IFocusOperation>
    {
        /// <inheritdoc/>
        public new IFocusOperation this[int index] { get { return (IFocusOperation)base[index]; } set { base[index] = value; } }

        #region IFocusOperation
        void ICollection<IFocusOperation>.Add(IFocusOperation item) { Add(item); }
        bool ICollection<IFocusOperation>.Contains(IFocusOperation item) { return Contains(item); }
        void ICollection<IFocusOperation>.CopyTo(IFocusOperation[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<IFocusOperation>.Remove(IFocusOperation item) { return Remove(item); }
        bool ICollection<IFocusOperation>.IsReadOnly { get { return ((ICollection<IFrameOperation>)this).IsReadOnly; } }
        IEnumerator<IFocusOperation> IEnumerable<IFocusOperation>.GetEnumerator() { var iterator = ((List<IWriteableOperation>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IFocusOperation)iterator.Current; } }
        IFocusOperation IList<IFocusOperation>.this[int index] { get { return (IFocusOperation)this[index]; } set { this[index] = value; } }
        int IList<IFocusOperation>.IndexOf(IFocusOperation item) { return IndexOf(item); }
        void IList<IFocusOperation>.Insert(int index, IFocusOperation item) { Insert(index, item); }
        IFocusOperation IReadOnlyList<IFocusOperation>.this[int index] { get { return (IFocusOperation)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override WriteableOperationReadOnlyList ToReadOnly()
        {
            return new FocusOperationReadOnlyList(this);
        }
    }
}
