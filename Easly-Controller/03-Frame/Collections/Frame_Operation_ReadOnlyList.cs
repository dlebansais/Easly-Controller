#pragma warning disable 1591

namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Writeable;

    /// <summary>
    /// Read-only list of IxxxOperation
    /// </summary>
    public interface IFrameOperationReadOnlyList : IWriteableOperationReadOnlyList, IReadOnlyList<IFrameOperation>
    {
        new int Count { get; }
        new IFrameOperation this[int index] { get; }
        bool Contains(IFrameOperation value);
        int IndexOf(IFrameOperation value);
        new IEnumerator<IFrameOperation> GetEnumerator();
    }

    /// <summary>
    /// Read-only list of IxxxOperation
    /// </summary>
    internal class FrameOperationReadOnlyList : ReadOnlyCollection<IFrameOperation>, IFrameOperationReadOnlyList
    {
        public FrameOperationReadOnlyList(IFrameOperationList list)
            : base(list)
        {
        }

        #region Writeable
        IWriteableOperation IReadOnlyList<IWriteableOperation>.this[int index] { get { return this[index]; } }
        bool IWriteableOperationReadOnlyList.Contains(IWriteableOperation value) { return Contains((IFrameOperation)value); }
        int IWriteableOperationReadOnlyList.IndexOf(IWriteableOperation value) { return IndexOf((IFrameOperation)value); }
        IEnumerator<IWriteableOperation> IEnumerable<IWriteableOperation>.GetEnumerator() { return GetEnumerator(); }
        #endregion
    }
}
