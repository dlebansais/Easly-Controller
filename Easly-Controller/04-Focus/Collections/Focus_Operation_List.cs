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
        new int Count { get; }
        new IFocusOperation this[int index] { get; set; }
        new IEnumerator<IFocusOperation> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxOperation
    /// </summary>
    internal class FocusOperationList : Collection<IFocusOperation>, IFocusOperationList
    {
        #region Writeable
        IWriteableOperation IWriteableOperationList.this[int index] { get { return this[index]; } set { this[index] = (IFocusOperation)value; } }
        IWriteableOperation IList<IWriteableOperation>.this[int index] { get { return this[index]; } set { this[index] = (IFocusOperation)value; } }
        IWriteableOperation IReadOnlyList<IWriteableOperation>.this[int index] { get { return this[index]; } }
        bool ICollection<IWriteableOperation>.IsReadOnly { get { return ((ICollection<IFocusOperation>)this).IsReadOnly; } }
        public void Add(IWriteableOperation item) { base.Add((IFocusOperation)item); }
        public void Insert(int index, IWriteableOperation item) { base.Insert(index, (IFocusOperation)item); }
        public bool Remove(IWriteableOperation item) { return base.Remove((IFocusOperation)item); }
        public void CopyTo(IWriteableOperation[] array, int index) { base.CopyTo((IFocusOperation[])array, index); }
        public bool Contains(IWriteableOperation value) { return base.Contains((IFocusOperation)value); }
        public int IndexOf(IWriteableOperation value) { return base.IndexOf((IFocusOperation)value); }
        IEnumerator<IWriteableOperation> IWriteableOperationList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IWriteableOperation> IEnumerable<IWriteableOperation>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Frame
        IFrameOperation IFrameOperationList.this[int index] { get { return this[index]; } set { this[index] = (IFocusOperation)value; } }
        IFrameOperation IList<IFrameOperation>.this[int index] { get { return this[index]; } set { this[index] = (IFocusOperation)value; } }
        IFrameOperation IReadOnlyList<IFrameOperation>.this[int index] { get { return this[index]; } }
        bool ICollection<IFrameOperation>.IsReadOnly { get { return ((ICollection<IFocusOperation>)this).IsReadOnly; } }
        public void Add(IFrameOperation item) { base.Add((IFocusOperation)item); }
        public void Insert(int index, IFrameOperation item) { base.Insert(index, (IFocusOperation)item); }
        public bool Remove(IFrameOperation item) { return base.Remove((IFocusOperation)item); }
        public void CopyTo(IFrameOperation[] array, int index) { base.CopyTo((IFocusOperation[])array, index); }
        public bool Contains(IFrameOperation value) { return base.Contains((IFocusOperation)value); }
        public int IndexOf(IFrameOperation value) { return base.IndexOf((IFocusOperation)value); }
        IEnumerator<IFrameOperation> IFrameOperationList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IFrameOperation> IEnumerable<IFrameOperation>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        public virtual IWriteableOperationReadOnlyList ToReadOnly()
        {
            return new FocusOperationReadOnlyList(this);
        }
    }
}
