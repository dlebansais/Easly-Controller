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
        new int Count { get; }
        new IFrameNodeState this[int index] { get; }
        bool Contains(IFrameNodeState value);
        int IndexOf(IFrameNodeState value);
        new IEnumerator<IFrameNodeState> GetEnumerator();
    }

    /// <summary>
    /// Read-only list of IxxxNodeState
    /// </summary>
    public class FrameNodeStateReadOnlyList : ReadOnlyCollection<IFrameNodeState>, IFrameNodeStateReadOnlyList
    {
        public FrameNodeStateReadOnlyList(IFrameNodeStateList list)
            : base(list)
        {
        }

        #region ReadOnly
        public new IReadOnlyNodeState this[int index] { get { return base[index]; } }
        public bool Contains(IReadOnlyNodeState value) { return base.Contains((IFrameNodeState)value); }
        public int IndexOf(IReadOnlyNodeState value) { return base.IndexOf((IFrameNodeState)value); }
        public new IEnumerator<IReadOnlyNodeState> GetEnumerator() { return base.GetEnumerator(); }
        #endregion

        #region Writeable
        IWriteableNodeState IWriteableNodeStateReadOnlyList.this[int index] { get { return base[index]; } }
        IWriteableNodeState IReadOnlyList<IWriteableNodeState>.this[int index] { get { return base[index]; } }
        public bool Contains(IWriteableNodeState value) { return base.Contains((IFrameNodeState)value); }
        public int IndexOf(IWriteableNodeState value) { return base.IndexOf((IFrameNodeState)value); }
        IEnumerator<IWriteableNodeState> IWriteableNodeStateReadOnlyList.GetEnumerator() { return base.GetEnumerator(); }
        IEnumerator<IWriteableNodeState> IEnumerable<IWriteableNodeState>.GetEnumerator() { return base.GetEnumerator(); }
        #endregion
    }
}
