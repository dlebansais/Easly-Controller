﻿using EaslyController.ReadOnly;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.Writeable
{
    /// <summary>
    /// List of IxxxPlaceholderNodeState
    /// </summary>
    public interface IWriteablePlaceholderNodeStateList : IReadOnlyPlaceholderNodeStateList, IList<IWriteablePlaceholderNodeState>, IReadOnlyList<IWriteablePlaceholderNodeState>
    {
        new int Count { get; }
        new IWriteablePlaceholderNodeState this[int index] { get; set; }
    }

    /// <summary>
    /// List of IxxxPlaceholderNodeState
    /// </summary>
    public class WriteablePlaceholderNodeStateList : Collection<IWriteablePlaceholderNodeState>, IWriteablePlaceholderNodeStateList
    {
        public new IReadOnlyPlaceholderNodeState this[int index] { get { return base[index]; } set { base[index] = (IWriteablePlaceholderNodeState)value; } }
        public void Add(IReadOnlyPlaceholderNodeState item) { base.Add((IWriteablePlaceholderNodeState)item); }
        public void Insert(int index, IReadOnlyPlaceholderNodeState item) { base.Insert(index, (IWriteablePlaceholderNodeState)item); }
        public bool Remove(IReadOnlyPlaceholderNodeState item) { return base.Remove((IWriteablePlaceholderNodeState)item); }
        public void CopyTo(IReadOnlyPlaceholderNodeState[] array, int index) { base.CopyTo((IWriteablePlaceholderNodeState[])array, index); }
        bool ICollection<IReadOnlyPlaceholderNodeState>.IsReadOnly { get { return ((ICollection<IWriteablePlaceholderNodeState>)this).IsReadOnly; } }
        public bool Contains(IReadOnlyPlaceholderNodeState value) { return base.Contains((IWriteablePlaceholderNodeState)value); }
        public int IndexOf(IReadOnlyPlaceholderNodeState value) { return base.IndexOf((IWriteablePlaceholderNodeState)value); }
        public new IEnumerator<IReadOnlyPlaceholderNodeState> GetEnumerator() { return base.GetEnumerator(); }
    }
}