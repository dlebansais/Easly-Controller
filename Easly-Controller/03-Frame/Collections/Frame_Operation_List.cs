#pragma warning disable 1591

namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Writeable;

    /// <summary>
    /// List of IxxxOperation
    /// </summary>
    public interface IFrameOperationList : IWriteableOperationList, IList<IFrameOperation>, IReadOnlyList<IFrameOperation>
    {
        new int Count { get; }
        new IFrameOperation this[int index] { get; set; }
        new IEnumerator<IFrameOperation> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxOperation
    /// </summary>
    internal class FrameOperationList : Collection<IFrameOperation>, IFrameOperationList
    {
        #region Writeable
        IWriteableOperation IWriteableOperationList.this[int index] { get { return this[index]; } set { this[index] = (IFrameOperation)value; } }
        IWriteableOperation IList<IWriteableOperation>.this[int index] { get { return this[index]; } set { this[index] = (IFrameOperation)value; } }
        IWriteableOperation IReadOnlyList<IWriteableOperation>.this[int index] { get { return this[index]; } }
        bool ICollection<IWriteableOperation>.IsReadOnly { get { return ((ICollection<IFrameOperation>)this).IsReadOnly; } }
        public void Add(IWriteableOperation item) { base.Add((IFrameOperation)item); }
        public void Insert(int index, IWriteableOperation item) { base.Insert(index, (IFrameOperation)item); }
        public bool Remove(IWriteableOperation item) { return base.Remove((IFrameOperation)item); }
        public void CopyTo(IWriteableOperation[] array, int index) { base.CopyTo((IFrameOperation[])array, index); }
        public bool Contains(IWriteableOperation value) { return base.Contains((IFrameOperation)value); }
        public int IndexOf(IWriteableOperation value) { return base.IndexOf((IFrameOperation)value); }
        IEnumerator<IWriteableOperation> IWriteableOperationList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IWriteableOperation> IEnumerable<IWriteableOperation>.GetEnumerator() { return GetEnumerator(); }
        #endregion
    }
}
