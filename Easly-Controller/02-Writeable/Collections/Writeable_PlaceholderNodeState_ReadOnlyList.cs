using EaslyController.ReadOnly;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Read-only list of IxxxPlaceholderNodeState
    /// </summary>
    public interface IWriteablePlaceholderNodeStateReadOnlyList : IReadOnlyPlaceholderNodeStateReadOnlyList, IReadOnlyList<IWriteablePlaceholderNodeState>
    {
        new int Count { get; }
        new IWriteablePlaceholderNodeState this[int index] { get; }
        bool Contains(IWriteablePlaceholderNodeState value);
        int IndexOf(IWriteablePlaceholderNodeState value);
        new IEnumerator<IWriteablePlaceholderNodeState> GetEnumerator();
    }

    /// <summary>
    /// Read-only list of IxxxPlaceholderNodeState
    /// </summary>
    public class WriteablePlaceholderNodeStateReadOnlyList : ReadOnlyCollection<IWriteablePlaceholderNodeState>, IWriteablePlaceholderNodeStateReadOnlyList
    {
        public WriteablePlaceholderNodeStateReadOnlyList(IWriteablePlaceholderNodeStateList list)
            : base(list)
        {
        }

        public new IReadOnlyPlaceholderNodeState this[int index] { get { return base[index]; } }
        public bool Contains(IReadOnlyPlaceholderNodeState value) { return base.Contains((IWriteablePlaceholderNodeState)value); }
        public int IndexOf(IReadOnlyPlaceholderNodeState value) { return base.IndexOf((IWriteablePlaceholderNodeState)value); }
        public new IEnumerator<IReadOnlyPlaceholderNodeState> GetEnumerator() { return base.GetEnumerator(); }
    }
}
