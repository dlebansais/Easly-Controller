#pragma warning disable 1591

namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// List of IxxxPlaceholderNodeState
    /// </summary>
    public interface IFramePlaceholderNodeStateList : IWriteablePlaceholderNodeStateList, IList<IFramePlaceholderNodeState>, IReadOnlyList<IFramePlaceholderNodeState>
    {
        new int Count { get; }
        new IFramePlaceholderNodeState this[int index] { get; set; }
        new IEnumerator<IFramePlaceholderNodeState> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxPlaceholderNodeState
    /// </summary>
    internal class FramePlaceholderNodeStateList : Collection<IFramePlaceholderNodeState>, IFramePlaceholderNodeStateList
    {
        #region ReadOnly
        IReadOnlyPlaceholderNodeState IReadOnlyPlaceholderNodeStateList.this[int index] { get { return this[index]; } set { this[index] = (IFramePlaceholderNodeState)value; } }
        IReadOnlyPlaceholderNodeState IList<IReadOnlyPlaceholderNodeState>.this[int index] { get { return this[index]; } set { this[index] = (IFramePlaceholderNodeState)value; } }
        IReadOnlyPlaceholderNodeState IReadOnlyList<IReadOnlyPlaceholderNodeState>.this[int index] { get { return this[index]; } }
        public void Add(IReadOnlyPlaceholderNodeState item) { base.Add((IFramePlaceholderNodeState)item); }
        public void Insert(int index, IReadOnlyPlaceholderNodeState item) { base.Insert(index, (IFramePlaceholderNodeState)item); }
        public bool Remove(IReadOnlyPlaceholderNodeState item) { return base.Remove((IFramePlaceholderNodeState)item); }
        public void CopyTo(IReadOnlyPlaceholderNodeState[] array, int index) { base.CopyTo((IFramePlaceholderNodeState[])array, index); }
        bool ICollection<IReadOnlyPlaceholderNodeState>.IsReadOnly { get { return ((ICollection<IFramePlaceholderNodeState>)this).IsReadOnly; } }
        public bool Contains(IReadOnlyPlaceholderNodeState value) { return base.Contains((IFramePlaceholderNodeState)value); }
        public int IndexOf(IReadOnlyPlaceholderNodeState value) { return base.IndexOf((IFramePlaceholderNodeState)value); }
        IEnumerator<IReadOnlyPlaceholderNodeState> IEnumerable<IReadOnlyPlaceholderNodeState>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Writeable
        IWriteablePlaceholderNodeState IWriteablePlaceholderNodeStateList.this[int index] { get { return this[index]; } set { this[index] = (IFramePlaceholderNodeState)value; } }
        IWriteablePlaceholderNodeState IList<IWriteablePlaceholderNodeState>.this[int index] { get { return this[index]; } set { this[index] = (IFramePlaceholderNodeState)value; } }
        IWriteablePlaceholderNodeState IReadOnlyList<IWriteablePlaceholderNodeState>.this[int index] { get { return this[index]; } }
        public void Add(IWriteablePlaceholderNodeState item) { base.Add((IFramePlaceholderNodeState)item); }
        public void Insert(int index, IWriteablePlaceholderNodeState item) { base.Insert(index, (IFramePlaceholderNodeState)item); }
        public bool Remove(IWriteablePlaceholderNodeState item) { return base.Remove((IFramePlaceholderNodeState)item); }
        public void CopyTo(IWriteablePlaceholderNodeState[] array, int index) { base.CopyTo((IFramePlaceholderNodeState[])array, index); }
        bool ICollection<IWriteablePlaceholderNodeState>.IsReadOnly { get { return ((ICollection<IFramePlaceholderNodeState>)this).IsReadOnly; } }
        public bool Contains(IWriteablePlaceholderNodeState value) { return base.Contains((IFramePlaceholderNodeState)value); }
        public int IndexOf(IWriteablePlaceholderNodeState value) { return base.IndexOf((IFramePlaceholderNodeState)value); }
        IEnumerator<IWriteablePlaceholderNodeState> IWriteablePlaceholderNodeStateList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IWriteablePlaceholderNodeState> IEnumerable<IWriteablePlaceholderNodeState>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        public virtual IReadOnlyPlaceholderNodeStateReadOnlyList ToReadOnly()
        {
            return new FramePlaceholderNodeStateReadOnlyList(this);
        }
    }
}
