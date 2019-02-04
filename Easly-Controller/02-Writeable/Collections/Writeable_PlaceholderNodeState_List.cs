#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;

    /// <summary>
    /// List of IxxxPlaceholderNodeState
    /// </summary>
    public interface IWriteablePlaceholderNodeStateList : IReadOnlyPlaceholderNodeStateList, IList<IWriteablePlaceholderNodeState>, IReadOnlyList<IWriteablePlaceholderNodeState>
    {
        new int Count { get; }
        new IWriteablePlaceholderNodeState this[int index] { get; set; }
        new IEnumerator<IWriteablePlaceholderNodeState> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxPlaceholderNodeState
    /// </summary>
    internal class WriteablePlaceholderNodeStateList : Collection<IWriteablePlaceholderNodeState>, IWriteablePlaceholderNodeStateList
    {
        #region ReadOnly
        IReadOnlyPlaceholderNodeState IReadOnlyPlaceholderNodeStateList.this[int index] { get { return this[index]; } set { this[index] = (IWriteablePlaceholderNodeState)value; } }
        IReadOnlyPlaceholderNodeState IList<IReadOnlyPlaceholderNodeState>.this[int index] { get { return this[index]; } set { this[index] = (IWriteablePlaceholderNodeState)value; } }
        IReadOnlyPlaceholderNodeState IReadOnlyList<IReadOnlyPlaceholderNodeState>.this[int index] { get { return this[index]; } }
        public void Add(IReadOnlyPlaceholderNodeState item) { base.Add((IWriteablePlaceholderNodeState)item); }
        public void Insert(int index, IReadOnlyPlaceholderNodeState item) { base.Insert(index, (IWriteablePlaceholderNodeState)item); }
        public bool Remove(IReadOnlyPlaceholderNodeState item) { return base.Remove((IWriteablePlaceholderNodeState)item); }
        public void CopyTo(IReadOnlyPlaceholderNodeState[] array, int index) { base.CopyTo((IWriteablePlaceholderNodeState[])array, index); }
        bool ICollection<IReadOnlyPlaceholderNodeState>.IsReadOnly { get { return ((ICollection<IWriteablePlaceholderNodeState>)this).IsReadOnly; } }
        public bool Contains(IReadOnlyPlaceholderNodeState value) { return base.Contains((IWriteablePlaceholderNodeState)value); }
        public int IndexOf(IReadOnlyPlaceholderNodeState value) { return base.IndexOf((IWriteablePlaceholderNodeState)value); }
        IEnumerator<IReadOnlyPlaceholderNodeState> IEnumerable<IReadOnlyPlaceholderNodeState>.GetEnumerator() { return GetEnumerator(); }
        #endregion
    }
}
