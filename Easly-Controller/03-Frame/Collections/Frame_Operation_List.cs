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
        void ICollection<IWriteableOperation>.Add(IWriteableOperation item) { Add((IFrameOperation)item); }
        void IList<IWriteableOperation>.Insert(int index, IWriteableOperation item) { Insert(index, (IFrameOperation)item); }
        bool ICollection<IWriteableOperation>.Remove(IWriteableOperation item) { return Remove((IFrameOperation)item); }
        void ICollection<IWriteableOperation>.CopyTo(IWriteableOperation[] array, int index) { CopyTo((IFrameOperation[])array, index); }
        bool ICollection<IWriteableOperation>.Contains(IWriteableOperation value) { return Contains((IFrameOperation)value); }
        int IList<IWriteableOperation>.IndexOf(IWriteableOperation value) { return IndexOf((IFrameOperation)value); }
        IEnumerator<IWriteableOperation> IWriteableOperationList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IWriteableOperation> IEnumerable<IWriteableOperation>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        public virtual IWriteableOperationReadOnlyList ToReadOnly()
        {
            return new FrameOperationReadOnlyList(this);
        }
    }
}
