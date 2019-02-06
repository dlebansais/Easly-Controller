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
        new IFrameOperation this[int index] { get; set; }
        new int Count { get; }
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
        int IList<IWriteableOperation>.IndexOf(IWriteableOperation value) { return IndexOf((IFrameOperation)value); }
        void IList<IWriteableOperation>.Insert(int index, IWriteableOperation item) { Insert(index, (IFrameOperation)item); }
        void ICollection<IWriteableOperation>.Add(IWriteableOperation item) { Add((IFrameOperation)item); }
        bool ICollection<IWriteableOperation>.Contains(IWriteableOperation value) { return Contains((IFrameOperation)value); }
        void ICollection<IWriteableOperation>.CopyTo(IWriteableOperation[] array, int index) { CopyTo((IFrameOperation[])array, index); }
        bool ICollection<IWriteableOperation>.IsReadOnly { get { return ((ICollection<IFrameOperation>)this).IsReadOnly; } }
        bool ICollection<IWriteableOperation>.Remove(IWriteableOperation item) { return Remove((IFrameOperation)item); }
        IEnumerator<IWriteableOperation> IEnumerable<IWriteableOperation>.GetEnumerator() { return GetEnumerator(); }
        IWriteableOperation IReadOnlyList<IWriteableOperation>.this[int index] { get { return this[index]; } }
        #endregion

        public virtual IWriteableOperationReadOnlyList ToReadOnly()
        {
            return new FrameOperationReadOnlyList(this);
        }
    }
}
