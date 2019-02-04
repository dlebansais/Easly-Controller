#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Read-only list of IxxxNodeState
    /// </summary>
    public interface IWriteableNodeStateReadOnlyList : IReadOnlyNodeStateReadOnlyList, IReadOnlyList<IWriteableNodeState>
    {
        new int Count { get; }
        new IWriteableNodeState this[int index] { get; }
        bool Contains(IWriteableNodeState value);
        int IndexOf(IWriteableNodeState value);
        new IEnumerator<IWriteableNodeState> GetEnumerator();
    }

    /// <summary>
    /// Read-only list of IxxxNodeState
    /// </summary>
    internal class WriteableNodeStateReadOnlyList : ReadOnlyCollection<IWriteableNodeState>, IWriteableNodeStateReadOnlyList
    {
        public WriteableNodeStateReadOnlyList(IWriteableNodeStateList list)
            : base(list)
        {
        }

        #region ReadOnly
        IReadOnlyNodeState IReadOnlyList<IReadOnlyNodeState>.this[int index] { get { return this[index]; } }
        public bool Contains(IReadOnlyNodeState value) { return base.Contains((IWriteableNodeState)value); }
        public int IndexOf(IReadOnlyNodeState value) { return base.IndexOf((IWriteableNodeState)value); }
        IEnumerator<IReadOnlyNodeState> IEnumerable<IReadOnlyNodeState>.GetEnumerator() { return GetEnumerator(); }
        #endregion
    }
}
