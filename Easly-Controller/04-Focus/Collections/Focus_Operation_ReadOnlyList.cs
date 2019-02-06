#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Frame;
    using EaslyController.Writeable;

    /// <summary>
    /// Read-only list of IxxxOperation
    /// </summary>
    public interface IFocusOperationReadOnlyList : IFrameOperationReadOnlyList, IReadOnlyList<IFocusOperation>
    {
        new int Count { get; }
        new IFocusOperation this[int index] { get; }
        bool Contains(IFocusOperation value);
        int IndexOf(IFocusOperation value);
        new IEnumerator<IFocusOperation> GetEnumerator();
    }

    /// <summary>
    /// Read-only list of IxxxOperation
    /// </summary>
    internal class FocusOperationReadOnlyList : ReadOnlyCollection<IFocusOperation>, IFocusOperationReadOnlyList
    {
        public FocusOperationReadOnlyList(IFocusOperationList list)
            : base(list)
        {
        }

        #region Writeable
        IWriteableOperation IReadOnlyList<IWriteableOperation>.this[int index] { get { return this[index]; } }
        bool IWriteableOperationReadOnlyList.Contains(IWriteableOperation value) { return Contains((IFocusOperation)value); }
        int IWriteableOperationReadOnlyList.IndexOf(IWriteableOperation value) { return IndexOf((IFocusOperation)value); }
        IEnumerator<IWriteableOperation> IEnumerable<IWriteableOperation>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Frame
        IFrameOperation IFrameOperationReadOnlyList.this[int index] { get { return this[index]; } }
        IFrameOperation IReadOnlyList<IFrameOperation>.this[int index] { get { return this[index]; } }
        bool IFrameOperationReadOnlyList.Contains(IFrameOperation value) { return Contains((IFocusOperation)value); }
        int IFrameOperationReadOnlyList.IndexOf(IFrameOperation value) { return IndexOf((IFocusOperation)value); }
        IEnumerator<IFrameOperation> IFrameOperationReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IFrameOperation> IEnumerable<IFrameOperation>.GetEnumerator() { return GetEnumerator(); }
        #endregion
    }
}
