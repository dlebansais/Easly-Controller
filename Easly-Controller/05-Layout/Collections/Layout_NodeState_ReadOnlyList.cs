#pragma warning disable 1591

namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Focus;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Read-only list of IxxxNodeState
    /// </summary>
    public interface ILayoutNodeStateReadOnlyList : IFocusNodeStateReadOnlyList, IReadOnlyList<ILayoutNodeState>
    {
        new ILayoutNodeState this[int index] { get; }
        new int Count { get; }
        bool Contains(ILayoutNodeState value);
        new IEnumerator<ILayoutNodeState> GetEnumerator();
        int IndexOf(ILayoutNodeState value);
    }

    /// <summary>
    /// Read-only list of IxxxNodeState
    /// </summary>
    internal class LayoutNodeStateReadOnlyList : ReadOnlyCollection<ILayoutNodeState>, ILayoutNodeStateReadOnlyList
    {
        public LayoutNodeStateReadOnlyList(ILayoutNodeStateList list)
            : base(list)
        {
        }

        #region ReadOnly
        bool IReadOnlyNodeStateReadOnlyList.Contains(IReadOnlyNodeState value) { return Contains((ILayoutNodeState)value); }
        int IReadOnlyNodeStateReadOnlyList.IndexOf(IReadOnlyNodeState value) { return IndexOf((ILayoutNodeState)value); }
        IEnumerator<IReadOnlyNodeState> IEnumerable<IReadOnlyNodeState>.GetEnumerator() { return GetEnumerator(); }
        IReadOnlyNodeState IReadOnlyList<IReadOnlyNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Writeable
        IWriteableNodeState IWriteableNodeStateReadOnlyList.this[int index] { get { return this[index]; } }
        bool IWriteableNodeStateReadOnlyList.Contains(IWriteableNodeState value) { return Contains((ILayoutNodeState)value); }
        IEnumerator<IWriteableNodeState> IWriteableNodeStateReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        int IWriteableNodeStateReadOnlyList.IndexOf(IWriteableNodeState value) { return IndexOf((ILayoutNodeState)value); }
        IEnumerator<IWriteableNodeState> IEnumerable<IWriteableNodeState>.GetEnumerator() { return GetEnumerator(); }
        IWriteableNodeState IReadOnlyList<IWriteableNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Frame
        IFrameNodeState IFrameNodeStateReadOnlyList.this[int index] { get { return this[index]; } }
        bool IFrameNodeStateReadOnlyList.Contains(IFrameNodeState value) { return Contains((ILayoutNodeState)value); }
        IEnumerator<IFrameNodeState> IFrameNodeStateReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        int IFrameNodeStateReadOnlyList.IndexOf(IFrameNodeState value) { return IndexOf((ILayoutNodeState)value); }
        IEnumerator<IFrameNodeState> IEnumerable<IFrameNodeState>.GetEnumerator() { return GetEnumerator(); }
        IFrameNodeState IReadOnlyList<IFrameNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Focus
        IFocusNodeState IFocusNodeStateReadOnlyList.this[int index] { get { return this[index]; } }
        bool IFocusNodeStateReadOnlyList.Contains(IFocusNodeState value) { return Contains((ILayoutNodeState)value); }
        IEnumerator<IFocusNodeState> IFocusNodeStateReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        int IFocusNodeStateReadOnlyList.IndexOf(IFocusNodeState value) { return IndexOf((ILayoutNodeState)value); }
        IEnumerator<IFocusNodeState> IEnumerable<IFocusNodeState>.GetEnumerator() { return GetEnumerator(); }
        IFocusNodeState IReadOnlyList<IFocusNodeState>.this[int index] { get { return this[index]; } }
        #endregion
    }
}
