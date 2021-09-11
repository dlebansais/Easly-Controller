namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FocusOperationList : FrameOperationList, ICollection<FocusOperation>, IEnumerable<FocusOperation>, IList<FocusOperation>, IReadOnlyCollection<FocusOperation>, IReadOnlyList<FocusOperation>
    {
        /// <inheritdoc/>
        public new FocusOperation this[int index] { get { return (FocusOperation)base[index]; } set { base[index] = value; } }

        #region FocusOperation
        void ICollection<FocusOperation>.Add(FocusOperation item) { Add(item); }
        bool ICollection<FocusOperation>.Contains(FocusOperation item) { return Contains(item); }
        void ICollection<FocusOperation>.CopyTo(FocusOperation[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<FocusOperation>.Remove(FocusOperation item) { return Remove(item); }
        bool ICollection<FocusOperation>.IsReadOnly { get { return ((ICollection<FrameOperation>)this).IsReadOnly; } }
        IEnumerator<FocusOperation> IEnumerable<FocusOperation>.GetEnumerator() { Enumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (FocusOperation)iterator.Current; } }
        FocusOperation IList<FocusOperation>.this[int index] { get { return (FocusOperation)this[index]; } set { this[index] = value; } }
        int IList<FocusOperation>.IndexOf(FocusOperation item) { return IndexOf(item); }
        void IList<FocusOperation>.Insert(int index, FocusOperation item) { Insert(index, item); }
        FocusOperation IReadOnlyList<FocusOperation>.this[int index] { get { return (FocusOperation)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override WriteableOperationReadOnlyList ToReadOnly()
        {
            return new FocusOperationReadOnlyList(this);
        }
    }
}
