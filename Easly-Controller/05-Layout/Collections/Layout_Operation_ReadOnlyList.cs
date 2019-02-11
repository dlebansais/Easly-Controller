#pragma warning disable 1591

namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Focus;
    using EaslyController.Frame;
    using EaslyController.Writeable;

    /// <summary>
    /// Read-only list of IxxxOperation
    /// </summary>
    public interface ILayoutOperationReadOnlyList : IFocusOperationReadOnlyList, IReadOnlyList<ILayoutOperation>
    {
        new ILayoutOperation this[int index] { get; }
        new int Count { get; }
        bool Contains(ILayoutOperation value);
        new IEnumerator<ILayoutOperation> GetEnumerator();
        int IndexOf(ILayoutOperation value);
    }

    /// <summary>
    /// Read-only list of IxxxOperation
    /// </summary>
    internal class LayoutOperationReadOnlyList : ReadOnlyCollection<ILayoutOperation>, ILayoutOperationReadOnlyList
    {
        public LayoutOperationReadOnlyList(ILayoutOperationList list)
            : base(list)
        {
        }

        #region Writeable
        bool IWriteableOperationReadOnlyList.Contains(IWriteableOperation value) { return Contains((ILayoutOperation)value); }
        int IWriteableOperationReadOnlyList.IndexOf(IWriteableOperation value) { return IndexOf((ILayoutOperation)value); }
        IEnumerator<IWriteableOperation> IEnumerable<IWriteableOperation>.GetEnumerator() { return GetEnumerator(); }
        IWriteableOperation IReadOnlyList<IWriteableOperation>.this[int index] { get { return this[index]; } }
        #endregion

        #region Frame
        IFrameOperation IFrameOperationReadOnlyList.this[int index] { get { return this[index]; } }
        IEnumerator<IFrameOperation> IFrameOperationReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        bool IFrameOperationReadOnlyList.Contains(IFrameOperation value) { return Contains((ILayoutOperation)value); }
        int IFrameOperationReadOnlyList.IndexOf(IFrameOperation value) { return IndexOf((ILayoutOperation)value); }
        IEnumerator<IFrameOperation> IEnumerable<IFrameOperation>.GetEnumerator() { return GetEnumerator(); }
        IFrameOperation IReadOnlyList<IFrameOperation>.this[int index] { get { return this[index]; } }
        #endregion

        #region Focus
        IFocusOperation IFocusOperationReadOnlyList.this[int index] { get { return this[index]; } }
        IEnumerator<IFocusOperation> IFocusOperationReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        bool IFocusOperationReadOnlyList.Contains(IFocusOperation value) { return Contains((ILayoutOperation)value); }
        int IFocusOperationReadOnlyList.IndexOf(IFocusOperation value) { return IndexOf((ILayoutOperation)value); }
        IEnumerator<IFocusOperation> IEnumerable<IFocusOperation>.GetEnumerator() { return GetEnumerator(); }
        IFocusOperation IReadOnlyList<IFocusOperation>.this[int index] { get { return this[index]; } }
        #endregion
    }
}
