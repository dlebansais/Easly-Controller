#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// List of IxxxPlaceholderNodeState
    /// </summary>
    public interface IFocusPlaceholderNodeStateList : IFramePlaceholderNodeStateList, IList<IFocusPlaceholderNodeState>, IReadOnlyList<IFocusPlaceholderNodeState>
    {
        new int Count { get; }
        new IFocusPlaceholderNodeState this[int index] { get; set; }
        new IEnumerator<IFocusPlaceholderNodeState> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxPlaceholderNodeState
    /// </summary>
    internal class FocusPlaceholderNodeStateList : Collection<IFocusPlaceholderNodeState>, IFocusPlaceholderNodeStateList
    {
        #region ReadOnly
        public new IReadOnlyPlaceholderNodeState this[int index] { get { return base[index]; } set { base[index] = (IFocusPlaceholderNodeState)value; } }
        public void Add(IReadOnlyPlaceholderNodeState item) { base.Add((IFocusPlaceholderNodeState)item); }
        public void Insert(int index, IReadOnlyPlaceholderNodeState item) { base.Insert(index, (IFocusPlaceholderNodeState)item); }
        public bool Remove(IReadOnlyPlaceholderNodeState item) { return base.Remove((IFocusPlaceholderNodeState)item); }
        public void CopyTo(IReadOnlyPlaceholderNodeState[] array, int index) { base.CopyTo((IFocusPlaceholderNodeState[])array, index); }
        bool ICollection<IReadOnlyPlaceholderNodeState>.IsReadOnly { get { return ((ICollection<IFocusPlaceholderNodeState>)this).IsReadOnly; } }
        public bool Contains(IReadOnlyPlaceholderNodeState value) { return base.Contains((IFocusPlaceholderNodeState)value); }
        public int IndexOf(IReadOnlyPlaceholderNodeState value) { return base.IndexOf((IFocusPlaceholderNodeState)value); }
        IEnumerator<IReadOnlyPlaceholderNodeState> IEnumerable<IReadOnlyPlaceholderNodeState>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Writeable
        IWriteablePlaceholderNodeState IWriteablePlaceholderNodeStateList.this[int index] { get { return base[index]; } set { base[index] = (IFocusPlaceholderNodeState)value; } }
        IWriteablePlaceholderNodeState IList<IWriteablePlaceholderNodeState>.this[int index] { get { return base[index]; } set { base[index] = (IFocusPlaceholderNodeState)value; } }
        IWriteablePlaceholderNodeState IReadOnlyList<IWriteablePlaceholderNodeState>.this[int index] { get { return base[index]; } }
        public void Add(IWriteablePlaceholderNodeState item) { base.Add((IFocusPlaceholderNodeState)item); }
        public void Insert(int index, IWriteablePlaceholderNodeState item) { base.Insert(index, (IFocusPlaceholderNodeState)item); }
        public bool Remove(IWriteablePlaceholderNodeState item) { return base.Remove((IFocusPlaceholderNodeState)item); }
        public void CopyTo(IWriteablePlaceholderNodeState[] array, int index) { base.CopyTo((IFocusPlaceholderNodeState[])array, index); }
        bool ICollection<IWriteablePlaceholderNodeState>.IsReadOnly { get { return ((ICollection<IFocusPlaceholderNodeState>)this).IsReadOnly; } }
        public bool Contains(IWriteablePlaceholderNodeState value) { return base.Contains((IFocusPlaceholderNodeState)value); }
        public int IndexOf(IWriteablePlaceholderNodeState value) { return base.IndexOf((IFocusPlaceholderNodeState)value); }
        IEnumerator<IWriteablePlaceholderNodeState> IWriteablePlaceholderNodeStateList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IWriteablePlaceholderNodeState> IEnumerable<IWriteablePlaceholderNodeState>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Frame
        IFramePlaceholderNodeState IFramePlaceholderNodeStateList.this[int index] { get { return base[index]; } set { base[index] = (IFocusPlaceholderNodeState)value; } }
        IFramePlaceholderNodeState IList<IFramePlaceholderNodeState>.this[int index] { get { return base[index]; } set { base[index] = (IFocusPlaceholderNodeState)value; } }
        IFramePlaceholderNodeState IReadOnlyList<IFramePlaceholderNodeState>.this[int index] { get { return base[index]; } }
        public void Add(IFramePlaceholderNodeState item) { base.Add((IFocusPlaceholderNodeState)item); }
        public void Insert(int index, IFramePlaceholderNodeState item) { base.Insert(index, (IFocusPlaceholderNodeState)item); }
        public bool Remove(IFramePlaceholderNodeState item) { return base.Remove((IFocusPlaceholderNodeState)item); }
        public void CopyTo(IFramePlaceholderNodeState[] array, int index) { base.CopyTo((IFocusPlaceholderNodeState[])array, index); }
        bool ICollection<IFramePlaceholderNodeState>.IsReadOnly { get { return ((ICollection<IFocusPlaceholderNodeState>)this).IsReadOnly; } }
        public bool Contains(IFramePlaceholderNodeState value) { return base.Contains((IFocusPlaceholderNodeState)value); }
        public int IndexOf(IFramePlaceholderNodeState value) { return base.IndexOf((IFocusPlaceholderNodeState)value); }
        IEnumerator<IFramePlaceholderNodeState> IFramePlaceholderNodeStateList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IFramePlaceholderNodeState> IEnumerable<IFramePlaceholderNodeState>.GetEnumerator() { return GetEnumerator(); }
        #endregion
    }
}
