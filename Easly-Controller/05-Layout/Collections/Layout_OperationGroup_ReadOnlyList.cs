#pragma warning disable 1591

namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Focus;
    using EaslyController.Frame;
    using EaslyController.Writeable;

    /// <summary>
    /// Read-only list of IxxxOperationGroup
    /// </summary>
    public interface ILayoutOperationGroupReadOnlyList : IFocusOperationGroupReadOnlyList, IReadOnlyList<ILayoutOperationGroup>
    {
        new ILayoutOperationGroup this[int index] { get; }
        new int Count { get; }
        bool Contains(ILayoutOperationGroup value);
        new IEnumerator<ILayoutOperationGroup> GetEnumerator();
        int IndexOf(ILayoutOperationGroup value);
    }

    /// <summary>
    /// Read-only list of IxxxOperationGroup
    /// </summary>
    internal class LayoutOperationGroupReadOnlyList : ReadOnlyCollection<ILayoutOperationGroup>, ILayoutOperationGroupReadOnlyList
    {
        public LayoutOperationGroupReadOnlyList(ILayoutOperationGroupList list)
            : base(list)
        {
        }

        #region Writeable
        bool IWriteableOperationGroupReadOnlyList.Contains(IWriteableOperationGroup value) { return Contains((ILayoutOperationGroup)value); }
        int IWriteableOperationGroupReadOnlyList.IndexOf(IWriteableOperationGroup value) { return IndexOf((ILayoutOperationGroup)value); }
        IEnumerator<IWriteableOperationGroup> IEnumerable<IWriteableOperationGroup>.GetEnumerator() { return GetEnumerator(); }
        IWriteableOperationGroup IReadOnlyList<IWriteableOperationGroup>.this[int index] { get { return this[index]; } }
        #endregion

        #region Frame
        IFrameOperationGroup IFrameOperationGroupReadOnlyList.this[int index] { get { return this[index]; } }
        IEnumerator<IFrameOperationGroup> IFrameOperationGroupReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        bool IFrameOperationGroupReadOnlyList.Contains(IFrameOperationGroup value) { return Contains((ILayoutOperationGroup)value); }
        int IFrameOperationGroupReadOnlyList.IndexOf(IFrameOperationGroup value) { return IndexOf((ILayoutOperationGroup)value); }
        IEnumerator<IFrameOperationGroup> IEnumerable<IFrameOperationGroup>.GetEnumerator() { return GetEnumerator(); }
        IFrameOperationGroup IReadOnlyList<IFrameOperationGroup>.this[int index] { get { return this[index]; } }
        #endregion

        #region Focus
        IFocusOperationGroup IFocusOperationGroupReadOnlyList.this[int index] { get { return this[index]; } }
        IEnumerator<IFocusOperationGroup> IFocusOperationGroupReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        bool IFocusOperationGroupReadOnlyList.Contains(IFocusOperationGroup value) { return Contains((ILayoutOperationGroup)value); }
        int IFocusOperationGroupReadOnlyList.IndexOf(IFocusOperationGroup value) { return IndexOf((ILayoutOperationGroup)value); }
        IEnumerator<IFocusOperationGroup> IEnumerable<IFocusOperationGroup>.GetEnumerator() { return GetEnumerator(); }
        IFocusOperationGroup IReadOnlyList<IFocusOperationGroup>.this[int index] { get { return this[index]; } }
        #endregion
    }
}
