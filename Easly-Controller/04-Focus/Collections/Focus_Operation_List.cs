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
    public class FocusOperationList : Collection<IFocusOperation>, IFocusOperationList
    {
        #region Writeable
        bool ICollection<IWriteableOperation>.IsReadOnly { get { return ((ICollection<IFocusOperation>)this).IsReadOnly; } }
        public void Add(IWriteableOperation item) { base.Add((IFocusOperation)item); }
        public void Insert(int index, IWriteableOperation item) { base.Insert(index, (IFocusOperation)item); }
        IWriteableOperation IWriteableOperationList.this[int index] { get { return base[index]; } set { base[index] = (IFocusOperation)value; } }
        IWriteableOperation IList<IWriteableOperation>.this[int index] { get { return base[index]; } set { base[index] = (IFocusOperation)value; } }
        IWriteableOperation IReadOnlyList<IWriteableOperation>.this[int index] { get { return base[index]; } }
        public bool Remove(IWriteableOperation item) { return base.Remove((IFocusOperation)item); }
        public void CopyTo(IWriteableOperation[] array, int index) { base.CopyTo((IFocusOperation[])array, index); }
        public bool Contains(IWriteableOperation value) { return base.Contains((IFocusOperation)value); }
        public int IndexOf(IWriteableOperation value) { return base.IndexOf((IFocusOperation)value); }
        IEnumerator<IWriteableOperation> IEnumerable<IWriteableOperation>.GetEnumerator() { return base.GetEnumerator(); }
        IEnumerator<IWriteableOperation> IWriteableOperationList.GetEnumerator() { return base.GetEnumerator(); }
        #endregion

        #region Frame
        bool ICollection<IFrameOperation>.IsReadOnly { get { return ((ICollection<IFocusOperation>)this).IsReadOnly; } }
        public void Add(IFrameOperation item) { base.Add((IFocusOperation)item); }
        public void Insert(int index, IFrameOperation item) { base.Insert(index, (IFocusOperation)item); }
        public new IFrameOperation this[int index] { get { return base[index]; } set { base[index] = (IFocusOperation)value; } }
        public bool Remove(IFrameOperation item) { return base.Remove((IFocusOperation)item); }
        public void CopyTo(IFrameOperation[] array, int index) { base.CopyTo((IFocusOperation[])array, index); }
        public bool Contains(IFrameOperation value) { return base.Contains((IFocusOperation)value); }
        public int IndexOf(IFrameOperation value) { return base.IndexOf((IFocusOperation)value); }
        public new IEnumerator<IFrameOperation> GetEnumerator() { return base.GetEnumerator(); }
        #endregion
    }
}
