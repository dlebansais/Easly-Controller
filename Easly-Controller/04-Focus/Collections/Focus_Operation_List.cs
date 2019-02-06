#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Frame;
    using EaslyController.Writeable;

    /// <summary>
    /// List of IxxxOperation
    /// </summary>
    public interface IFocusOperationList : IFrameOperationList, IList<IFocusOperation>, IReadOnlyList<IFocusOperation>
    {
        new IFocusOperation this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<IFocusOperation> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxOperation
    /// </summary>
    internal class FocusOperationList : Collection<IFocusOperation>, IFocusOperationList
    {
        #region Writeable
        IWriteableOperation IWriteableOperationList.this[int index] { get { return this[index]; } set { this[index] = (IFocusOperation)value; } }
        IEnumerator<IWriteableOperation> IWriteableOperationList.GetEnumerator() { return GetEnumerator(); }
        IWriteableOperation IList<IWriteableOperation>.this[int index] { get { return this[index]; } set { this[index] = (IFocusOperation)value; } }
        int IList<IWriteableOperation>.IndexOf(IWriteableOperation value) { return IndexOf((IFocusOperation)value); }
        void IList<IWriteableOperation>.Insert(int index, IWriteableOperation item) { Insert(index, (IFocusOperation)item); }
        void ICollection<IWriteableOperation>.Add(IWriteableOperation item) { Add((IFocusOperation)item); }
        bool ICollection<IWriteableOperation>.Contains(IWriteableOperation value) { return Contains((IFocusOperation)value); }
        void ICollection<IWriteableOperation>.CopyTo(IWriteableOperation[] array, int index) { CopyTo((IFocusOperation[])array, index); }
        bool ICollection<IWriteableOperation>.IsReadOnly { get { return ((ICollection<IFocusOperation>)this).IsReadOnly; } }
        bool ICollection<IWriteableOperation>.Remove(IWriteableOperation item) { return Remove((IFocusOperation)item); }
        IEnumerator<IWriteableOperation> IEnumerable<IWriteableOperation>.GetEnumerator() { return GetEnumerator(); }
        IWriteableOperation IReadOnlyList<IWriteableOperation>.this[int index] { get { return this[index]; } }
        #endregion

        #region Frame
        IFrameOperation IFrameOperationList.this[int index] { get { return this[index]; } set { this[index] = (IFocusOperation)value; } }
        IEnumerator<IFrameOperation> IFrameOperationList.GetEnumerator() { return GetEnumerator(); }
        IFrameOperation IList<IFrameOperation>.this[int index] { get { return this[index]; } set { this[index] = (IFocusOperation)value; } }
        int IList<IFrameOperation>.IndexOf(IFrameOperation value) { return IndexOf((IFocusOperation)value); }
        void IList<IFrameOperation>.Insert(int index, IFrameOperation item) { Insert(index, (IFocusOperation)item); }
        void ICollection<IFrameOperation>.Add(IFrameOperation item) { Add((IFocusOperation)item); }
        bool ICollection<IFrameOperation>.Contains(IFrameOperation value) { return Contains((IFocusOperation)value); }
        void ICollection<IFrameOperation>.CopyTo(IFrameOperation[] array, int index) { CopyTo((IFocusOperation[])array, index); }
        bool ICollection<IFrameOperation>.IsReadOnly { get { return ((ICollection<IFocusOperation>)this).IsReadOnly; } }
        bool ICollection<IFrameOperation>.Remove(IFrameOperation item) { return Remove((IFocusOperation)item); }
        IEnumerator<IFrameOperation> IEnumerable<IFrameOperation>.GetEnumerator() { return GetEnumerator(); }
        IFrameOperation IReadOnlyList<IFrameOperation>.this[int index] { get { return this[index]; } }
        #endregion

        public virtual IWriteableOperationReadOnlyList ToReadOnly()
        {
            return new FocusOperationReadOnlyList(this);
        }
    }
}
