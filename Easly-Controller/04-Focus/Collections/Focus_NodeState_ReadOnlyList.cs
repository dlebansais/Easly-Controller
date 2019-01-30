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
        public new IReadOnlyNodeState this[int index] { get { return base[index]; } }
        public bool Contains(IReadOnlyNodeState value) { return base.Contains((IFocusNodeState)value); }
        public int IndexOf(IReadOnlyNodeState value) { return base.IndexOf((IFocusNodeState)value); }
        public new IEnumerator<IReadOnlyNodeState> GetEnumerator() { return base.GetEnumerator(); }
        #endregion

        #region Writeable
        IWriteableNodeState IWriteableNodeStateReadOnlyList.this[int index] { get { return base[index]; } }
        IWriteableNodeState IReadOnlyList<IWriteableNodeState>.this[int index] { get { return base[index]; } }
        public bool Contains(IWriteableNodeState value) { return base.Contains((IFocusNodeState)value); }
        public int IndexOf(IWriteableNodeState value) { return base.IndexOf((IFocusNodeState)value); }
        IEnumerator<IWriteableNodeState> IWriteableNodeStateReadOnlyList.GetEnumerator() { return base.GetEnumerator(); }
        IEnumerator<IWriteableNodeState> IEnumerable<IWriteableNodeState>.GetEnumerator() { return base.GetEnumerator(); }
        #endregion

        #region Frame
        IFrameNodeState IFrameNodeStateReadOnlyList.this[int index] { get { return base[index]; } }
        IFrameNodeState IReadOnlyList<IFrameNodeState>.this[int index] { get { return base[index]; } }
        public bool Contains(IFrameNodeState value) { return base.Contains((IFocusNodeState)value); }
        public int IndexOf(IFrameNodeState value) { return base.IndexOf((IFocusNodeState)value); }
        IEnumerator<IFrameNodeState> IFrameNodeStateReadOnlyList.GetEnumerator() { return base.GetEnumerator(); }
        IEnumerator<IFrameNodeState> IEnumerable<IFrameNodeState>.GetEnumerator() { return base.GetEnumerator(); }
        #endregion
    }
}
