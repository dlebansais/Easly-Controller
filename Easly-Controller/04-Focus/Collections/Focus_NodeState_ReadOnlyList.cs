#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Read-only list of IxxxNodeState
    /// </summary>
    public interface IFocusNodeStateReadOnlyList : IFrameNodeStateReadOnlyList, IReadOnlyList<IFocusNodeState>
    {
        new IFocusNodeState this[int index] { get; }
        new int Count { get; }
        bool Contains(IFocusNodeState value);
        new IEnumerator<IFocusNodeState> GetEnumerator();
        int IndexOf(IFocusNodeState value);
    }

    /// <summary>
    /// Read-only list of IxxxNodeState
    /// </summary>
    internal class FocusNodeStateReadOnlyList : ReadOnlyCollection<IFocusNodeState>, IFocusNodeStateReadOnlyList
    {
        public FocusNodeStateReadOnlyList(IFocusNodeStateList list)
            : base(list)
        {
        }

        #region ReadOnly
        bool IReadOnlyNodeStateReadOnlyList.Contains(IReadOnlyNodeState value) { return Contains((IFocusNodeState)value); }
        int IReadOnlyNodeStateReadOnlyList.IndexOf(IReadOnlyNodeState value) { return IndexOf((IFocusNodeState)value); }
        IEnumerator<IReadOnlyNodeState> IEnumerable<IReadOnlyNodeState>.GetEnumerator() { return GetEnumerator(); }
        IReadOnlyNodeState IReadOnlyList<IReadOnlyNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Writeable
        IWriteableNodeState IWriteableNodeStateReadOnlyList.this[int index] { get { return this[index]; } }
        bool IWriteableNodeStateReadOnlyList.Contains(IWriteableNodeState value) { return Contains((IFocusNodeState)value); }
        IEnumerator<IWriteableNodeState> IWriteableNodeStateReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        int IWriteableNodeStateReadOnlyList.IndexOf(IWriteableNodeState value) { return IndexOf((IFocusNodeState)value); }
        IEnumerator<IWriteableNodeState> IEnumerable<IWriteableNodeState>.GetEnumerator() { return GetEnumerator(); }
        IWriteableNodeState IReadOnlyList<IWriteableNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Frame
        IFrameNodeState IFrameNodeStateReadOnlyList.this[int index] { get { return this[index]; } }
        bool IFrameNodeStateReadOnlyList.Contains(IFrameNodeState value) { return Contains((IFocusNodeState)value); }
        IEnumerator<IFrameNodeState> IFrameNodeStateReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        int IFrameNodeStateReadOnlyList.IndexOf(IFrameNodeState value) { return IndexOf((IFocusNodeState)value); }
        IEnumerator<IFrameNodeState> IEnumerable<IFrameNodeState>.GetEnumerator() { return GetEnumerator(); }
        IFrameNodeState IReadOnlyList<IFrameNodeState>.this[int index] { get { return this[index]; } }
        #endregion
    }
}
