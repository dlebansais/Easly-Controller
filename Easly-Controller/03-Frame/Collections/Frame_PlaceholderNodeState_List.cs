using EaslyController.ReadOnly;
using EaslyController.Writeable;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#pragma warning disable 1591

namespace EaslyController.Frame
{
    /// <summary>
    /// List of IxxxPlaceholderNodeState
    /// </summary>
    public interface IFramePlaceholderNodeStateList : IWriteablePlaceholderNodeStateList, IList<IFramePlaceholderNodeState>, IReadOnlyList<IFramePlaceholderNodeState>
    {
        new int Count { get; }
        new IFramePlaceholderNodeState this[int index] { get; set; }
    }

    /// <summary>
    /// List of IxxxPlaceholderNodeState
    /// </summary>
    public class FramePlaceholderNodeStateList : Collection<IFramePlaceholderNodeState>, IFramePlaceholderNodeStateList
    {
        #region ReadOnly
        public new IReadOnlyPlaceholderNodeState this[int index] { get { return base[index]; } set { base[index] = (IFramePlaceholderNodeState)value; } }
        public void Add(IReadOnlyPlaceholderNodeState item) { base.Add((IFramePlaceholderNodeState)item); }
        public void Insert(int index, IReadOnlyPlaceholderNodeState item) { base.Insert(index, (IFramePlaceholderNodeState)item); }
        public bool Remove(IReadOnlyPlaceholderNodeState item) { return base.Remove((IFramePlaceholderNodeState)item); }
        public void CopyTo(IReadOnlyPlaceholderNodeState[] array, int index) { base.CopyTo((IFramePlaceholderNodeState[])array, index); }
        bool ICollection<IReadOnlyPlaceholderNodeState>.IsReadOnly { get { return ((ICollection<IFramePlaceholderNodeState>)this).IsReadOnly; } }
        public bool Contains(IReadOnlyPlaceholderNodeState value) { return base.Contains((IFramePlaceholderNodeState)value); }
        public int IndexOf(IReadOnlyPlaceholderNodeState value) { return base.IndexOf((IFramePlaceholderNodeState)value); }
        public new IEnumerator<IReadOnlyPlaceholderNodeState> GetEnumerator() { return base.GetEnumerator(); }
        #endregion

        #region Writeable
        IWriteablePlaceholderNodeState IWriteablePlaceholderNodeStateList.this[int index] { get { return base[index]; } set { base[index] = (IFramePlaceholderNodeState)value; } }
        IWriteablePlaceholderNodeState IList<IWriteablePlaceholderNodeState>.this[int index] { get { return base[index]; } set { base[index] = (IFramePlaceholderNodeState)value; } }
        IWriteablePlaceholderNodeState IReadOnlyList<IWriteablePlaceholderNodeState>.this[int index] { get { return base[index]; } }
        public void Add(IWriteablePlaceholderNodeState item) { base.Add((IFramePlaceholderNodeState)item); }
        public void Insert(int index, IWriteablePlaceholderNodeState item) { base.Insert(index, (IFramePlaceholderNodeState)item); }
        public bool Remove(IWriteablePlaceholderNodeState item) { return base.Remove((IFramePlaceholderNodeState)item); }
        public void CopyTo(IWriteablePlaceholderNodeState[] array, int index) { base.CopyTo((IFramePlaceholderNodeState[])array, index); }
        bool ICollection<IWriteablePlaceholderNodeState>.IsReadOnly { get { return ((ICollection<IFramePlaceholderNodeState>)this).IsReadOnly; } }
        public bool Contains(IWriteablePlaceholderNodeState value) { return base.Contains((IFramePlaceholderNodeState)value); }
        public int IndexOf(IWriteablePlaceholderNodeState value) { return base.IndexOf((IFramePlaceholderNodeState)value); }
        IEnumerator<IWriteablePlaceholderNodeState> IEnumerable<IWriteablePlaceholderNodeState>.GetEnumerator() { return base.GetEnumerator(); }
        #endregion
    }
}
