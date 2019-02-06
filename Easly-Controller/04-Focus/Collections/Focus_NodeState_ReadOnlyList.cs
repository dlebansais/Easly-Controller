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
        new int Count { get; }
        new IFocusNodeState this[int index] { get; }
        bool Contains(IFocusNodeState value);
        int IndexOf(IFocusNodeState value);
        new IEnumerator<IFocusNodeState> GetEnumerator();
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
        IReadOnlyNodeState IReadOnlyList<IReadOnlyNodeState>.this[int index] { get { return this[index]; } }
        bool IReadOnlyNodeStateReadOnlyList.Contains(IReadOnlyNodeState value) { return Contains((IFocusNodeState)value); }
        int IReadOnlyNodeStateReadOnlyList.IndexOf(IReadOnlyNodeState value) { return IndexOf((IFocusNodeState)value); }
        IEnumerator<IReadOnlyNodeState> IEnumerable<IReadOnlyNodeState>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Writeable
        IWriteableNodeState IWriteableNodeStateReadOnlyList.this[int index] { get { return this[index]; } }
        IWriteableNodeState IReadOnlyList<IWriteableNodeState>.this[int index] { get { return this[index]; } }
        bool IWriteableNodeStateReadOnlyList.Contains(IWriteableNodeState value) { return Contains((IFocusNodeState)value); }
        int IWriteableNodeStateReadOnlyList.IndexOf(IWriteableNodeState value) { return IndexOf((IFocusNodeState)value); }
        IEnumerator<IWriteableNodeState> IWriteableNodeStateReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IWriteableNodeState> IEnumerable<IWriteableNodeState>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Frame
        IFrameNodeState IFrameNodeStateReadOnlyList.this[int index] { get { return this[index]; } }
        IFrameNodeState IReadOnlyList<IFrameNodeState>.this[int index] { get { return this[index]; } }
        bool IFrameNodeStateReadOnlyList.Contains(IFrameNodeState value) { return Contains((IFocusNodeState)value); }
        int IFrameNodeStateReadOnlyList.IndexOf(IFrameNodeState value) { return IndexOf((IFocusNodeState)value); }
        IEnumerator<IFrameNodeState> IFrameNodeStateReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IFrameNodeState> IEnumerable<IFrameNodeState>.GetEnumerator() { return GetEnumerator(); }
        #endregion
    }
}
