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
        new IWriteableNodeState this[int index] { get; }
        new int Count { get; }
        bool Contains(IWriteableNodeState value);
        new IEnumerator<IWriteableNodeState> GetEnumerator();
        int IndexOf(IWriteableNodeState value);
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
        bool IReadOnlyNodeStateReadOnlyList.Contains(IReadOnlyNodeState value) { return Contains((IWriteableNodeState)value); }
        int IReadOnlyNodeStateReadOnlyList.IndexOf(IReadOnlyNodeState value) { return IndexOf((IWriteableNodeState)value); }
        IEnumerator<IReadOnlyNodeState> IEnumerable<IReadOnlyNodeState>.GetEnumerator() { return GetEnumerator(); }
        IReadOnlyNodeState IReadOnlyList<IReadOnlyNodeState>.this[int index] { get { return this[index]; } }
        #endregion
    }
}
