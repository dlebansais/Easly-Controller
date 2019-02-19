#pragma warning disable 1591

namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Focus;
    using EaslyController.Frame;
    using EaslyController.Writeable;

    /// <summary>
    /// List of IxxxOperation
    /// </summary>
    public interface ILayoutOperationList : IFocusOperationList, IList<ILayoutOperation>, IReadOnlyList<ILayoutOperation>
    {
        new ILayoutOperation this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<ILayoutOperation> GetEnumerator();
        new void Clear();
    }

    /// <summary>
    /// List of IxxxOperation
    /// </summary>
    internal class LayoutOperationList : Collection<ILayoutOperation>, ILayoutOperationList
    {
        #region Writeable
        IWriteableOperation IWriteableOperationList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutOperation)value; } }
        IWriteableOperation IList<IWriteableOperation>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutOperation)value; } }
        int IList<IWriteableOperation>.IndexOf(IWriteableOperation value) { return IndexOf((ILayoutOperation)value); }
        void IList<IWriteableOperation>.Insert(int index, IWriteableOperation item) { Insert(index, (ILayoutOperation)item); }
        void ICollection<IWriteableOperation>.Add(IWriteableOperation item) { Add((ILayoutOperation)item); }
        bool ICollection<IWriteableOperation>.Contains(IWriteableOperation value) { return Contains((ILayoutOperation)value); }
        void ICollection<IWriteableOperation>.CopyTo(IWriteableOperation[] array, int index) { CopyTo((ILayoutOperation[])array, index); }
        bool ICollection<IWriteableOperation>.IsReadOnly { get { return ((ICollection<ILayoutOperation>)this).IsReadOnly; } }
        bool ICollection<IWriteableOperation>.Remove(IWriteableOperation item) { return Remove((ILayoutOperation)item); }
        IEnumerator<IWriteableOperation> IEnumerable<IWriteableOperation>.GetEnumerator() { return GetEnumerator(); }
        IWriteableOperation IReadOnlyList<IWriteableOperation>.this[int index] { get { return this[index]; } }
        #endregion

        #region Frame
        IFrameOperation IFrameOperationList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutOperation)value; } }
        IEnumerator<IFrameOperation> IFrameOperationList.GetEnumerator() { return GetEnumerator(); }
        IFrameOperation IList<IFrameOperation>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutOperation)value; } }
        int IList<IFrameOperation>.IndexOf(IFrameOperation value) { return IndexOf((ILayoutOperation)value); }
        void IList<IFrameOperation>.Insert(int index, IFrameOperation item) { Insert(index, (ILayoutOperation)item); }
        void ICollection<IFrameOperation>.Add(IFrameOperation item) { Add((ILayoutOperation)item); }
        bool ICollection<IFrameOperation>.Contains(IFrameOperation value) { return Contains((ILayoutOperation)value); }
        void ICollection<IFrameOperation>.CopyTo(IFrameOperation[] array, int index) { CopyTo((ILayoutOperation[])array, index); }
        bool ICollection<IFrameOperation>.IsReadOnly { get { return ((ICollection<ILayoutOperation>)this).IsReadOnly; } }
        bool ICollection<IFrameOperation>.Remove(IFrameOperation item) { return Remove((ILayoutOperation)item); }
        IEnumerator<IFrameOperation> IEnumerable<IFrameOperation>.GetEnumerator() { return GetEnumerator(); }
        IFrameOperation IReadOnlyList<IFrameOperation>.this[int index] { get { return this[index]; } }
        #endregion

        #region Focus
        IFocusOperation IFocusOperationList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutOperation)value; } }
        IEnumerator<IFocusOperation> IFocusOperationList.GetEnumerator() { return GetEnumerator(); }
        IFocusOperation IList<IFocusOperation>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutOperation)value; } }
        int IList<IFocusOperation>.IndexOf(IFocusOperation value) { return IndexOf((ILayoutOperation)value); }
        void IList<IFocusOperation>.Insert(int index, IFocusOperation item) { Insert(index, (ILayoutOperation)item); }
        void ICollection<IFocusOperation>.Add(IFocusOperation item) { Add((ILayoutOperation)item); }
        bool ICollection<IFocusOperation>.Contains(IFocusOperation value) { return Contains((ILayoutOperation)value); }
        void ICollection<IFocusOperation>.CopyTo(IFocusOperation[] array, int index) { CopyTo((ILayoutOperation[])array, index); }
        bool ICollection<IFocusOperation>.IsReadOnly { get { return ((ICollection<ILayoutOperation>)this).IsReadOnly; } }
        bool ICollection<IFocusOperation>.Remove(IFocusOperation item) { return Remove((ILayoutOperation)item); }
        IEnumerator<IFocusOperation> IEnumerable<IFocusOperation>.GetEnumerator() { return GetEnumerator(); }
        IFocusOperation IReadOnlyList<IFocusOperation>.this[int index] { get { return this[index]; } }
        #endregion

        public virtual IWriteableOperationReadOnlyList ToReadOnly()
        {
            return new LayoutOperationReadOnlyList(this);
        }
    }
}
