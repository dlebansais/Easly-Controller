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
    public class FocusOperationReadOnlyList : ReadOnlyCollection<IFocusOperation>, IFocusOperationReadOnlyList
    {
        public FocusOperationReadOnlyList(IFocusOperationList list)
            : base(list)
        {
        }

        #region Writeable
        public new IWriteableOperation this[int index] { get { return base[index]; } }
        public bool Contains(IWriteableOperation value) { return base.Contains((IFocusOperation)value); }
        public int IndexOf(IWriteableOperation value) { return base.IndexOf((IFocusOperation)value); }
        public new IEnumerator<IWriteableOperation> GetEnumerator() { return base.GetEnumerator(); }
        #endregion

        #region Frame
        IFrameOperation IFrameOperationReadOnlyList.this[int index] { get { return base[index]; } }
        IFrameOperation IReadOnlyList<IFrameOperation>.this[int index] { get { return base[index]; } }
        public bool Contains(IFrameOperation value) { return base.Contains((IFocusOperation)value); }
        public int IndexOf(IFrameOperation value) { return base.IndexOf((IFocusOperation)value); }
        IEnumerator<IFrameOperation> IFrameOperationReadOnlyList.GetEnumerator() { return base.GetEnumerator(); }
        IEnumerator<IFrameOperation> IEnumerable<IFrameOperation>.GetEnumerator() { return base.GetEnumerator(); }
        #endregion
    }
}
