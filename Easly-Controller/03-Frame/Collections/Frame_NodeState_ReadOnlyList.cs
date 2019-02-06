#pragma warning disable 1591

namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Read-only list of IxxxNodeState
    /// </summary>
    public interface IFrameNodeStateReadOnlyList : IWriteableNodeStateReadOnlyList, IReadOnlyList<IFrameNodeState>
    {
        new IFrameNodeState this[int index] { get; }
        new int Count { get; }
        bool Contains(IFrameNodeState value);
        new IEnumerator<IFrameNodeState> GetEnumerator();
        int IndexOf(IFrameNodeState value);
    }

    /// <summary>
    /// Read-only list of IxxxNodeState
    /// </summary>
    internal class FrameNodeStateReadOnlyList : ReadOnlyCollection<IFrameNodeState>, IFrameNodeStateReadOnlyList
    {
        public FrameNodeStateReadOnlyList(IFrameNodeStateList list)
            : base(list)
        {
        }

        #region ReadOnly
        bool IReadOnlyNodeStateReadOnlyList.Contains(IReadOnlyNodeState value) { return Contains((IFrameNodeState)value); }
        int IReadOnlyNodeStateReadOnlyList.IndexOf(IReadOnlyNodeState value) { return IndexOf((IFrameNodeState)value); }
        IEnumerator<IReadOnlyNodeState> IEnumerable<IReadOnlyNodeState>.GetEnumerator() { return GetEnumerator(); }
        IReadOnlyNodeState IReadOnlyList<IReadOnlyNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Writeable
        IWriteableNodeState IWriteableNodeStateReadOnlyList.this[int index] { get { return this[index]; } }
        bool IWriteableNodeStateReadOnlyList.Contains(IWriteableNodeState value) { return Contains((IFrameNodeState)value); }
        IEnumerator<IWriteableNodeState> IWriteableNodeStateReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        int IWriteableNodeStateReadOnlyList.IndexOf(IWriteableNodeState value) { return IndexOf((IFrameNodeState)value); }
        IEnumerator<IWriteableNodeState> IEnumerable<IWriteableNodeState>.GetEnumerator() { return GetEnumerator(); }
        IWriteableNodeState IReadOnlyList<IWriteableNodeState>.this[int index] { get { return this[index]; } }
        #endregion
    }
}
