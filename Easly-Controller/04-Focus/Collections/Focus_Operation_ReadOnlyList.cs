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
        new IFocusOperation this[int index] { get; }
        new int Count { get; }
        bool Contains(IFocusOperation value);
        new IEnumerator<IFocusOperation> GetEnumerator();
        int IndexOf(IFocusOperation value);
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
        bool IWriteableOperationReadOnlyList.Contains(IWriteableOperation value) { return Contains((IFocusOperation)value); }
        int IWriteableOperationReadOnlyList.IndexOf(IWriteableOperation value) { return IndexOf((IFocusOperation)value); }
        IEnumerator<IWriteableOperation> IEnumerable<IWriteableOperation>.GetEnumerator() { return GetEnumerator(); }
        IWriteableOperation IReadOnlyList<IWriteableOperation>.this[int index] { get { return this[index]; } }
        #endregion

        #region Frame
        IFrameOperation IFrameOperationReadOnlyList.this[int index] { get { return this[index]; } }
        IEnumerator<IFrameOperation> IFrameOperationReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        bool IFrameOperationReadOnlyList.Contains(IFrameOperation value) { return Contains((IFocusOperation)value); }
        int IFrameOperationReadOnlyList.IndexOf(IFrameOperation value) { return IndexOf((IFocusOperation)value); }
        IEnumerator<IFrameOperation> IEnumerable<IFrameOperation>.GetEnumerator() { return GetEnumerator(); }
        IFrameOperation IReadOnlyList<IFrameOperation>.this[int index] { get { return this[index]; } }
        #endregion
    }
}
