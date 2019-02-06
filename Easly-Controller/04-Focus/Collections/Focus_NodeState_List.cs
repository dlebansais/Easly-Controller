#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// List of IxxxNodeState
    /// </summary>
    public interface IFocusNodeStateList : IFrameNodeStateList, IList<IFocusNodeState>, IReadOnlyList<IFocusNodeState>
    {
        new IFocusNodeState this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<IFocusNodeState> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxNodeState
    /// </summary>
    internal class FocusNodeStateList : Collection<IFocusNodeState>, IFocusNodeStateList
    {
        #region ReadOnly
        IReadOnlyNodeState IReadOnlyNodeStateList.this[int index] { get { return this[index]; } set { this[index] = (IFocusNodeState)value; } }
        IReadOnlyNodeState IList<IReadOnlyNodeState>.this[int index] { get { return this[index]; } set { this[index] = (IFocusNodeState)value; } }
        int IList<IReadOnlyNodeState>.IndexOf(IReadOnlyNodeState value) { return IndexOf((IFocusNodeState)value); }
        void IList<IReadOnlyNodeState>.Insert(int index, IReadOnlyNodeState item) { Insert(index, (IFocusNodeState)item); }
        void ICollection<IReadOnlyNodeState>.Add(IReadOnlyNodeState item) { Add((IFocusNodeState)item); }
        bool ICollection<IReadOnlyNodeState>.Contains(IReadOnlyNodeState value) { return Contains((IFocusNodeState)value); }
        void ICollection<IReadOnlyNodeState>.CopyTo(IReadOnlyNodeState[] array, int index) { CopyTo((IFocusNodeState[])array, index); }
        bool ICollection<IReadOnlyNodeState>.IsReadOnly { get { return ((ICollection<IFocusNodeState>)this).IsReadOnly; } }
        bool ICollection<IReadOnlyNodeState>.Remove(IReadOnlyNodeState item) { return Remove((IFocusNodeState)item); }
        IEnumerator<IReadOnlyNodeState> IEnumerable<IReadOnlyNodeState>.GetEnumerator() { return GetEnumerator(); }
        IReadOnlyNodeState IReadOnlyList<IReadOnlyNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Writeable
        IWriteableNodeState IWriteableNodeStateList.this[int index] { get { return this[index]; } set { this[index] = (IFocusNodeState)value; } }
        IEnumerator<IWriteableNodeState> IWriteableNodeStateList.GetEnumerator() { return GetEnumerator(); }
        IWriteableNodeState IList<IWriteableNodeState>.this[int index] { get { return this[index]; } set { this[index] = (IFocusNodeState)value; } }
        int IList<IWriteableNodeState>.IndexOf(IWriteableNodeState value) { return IndexOf((IFocusNodeState)value); }
        void IList<IWriteableNodeState>.Insert(int index, IWriteableNodeState item) { Insert(index, (IFocusNodeState)item); }
        void ICollection<IWriteableNodeState>.Add(IWriteableNodeState item) { Add((IFocusNodeState)item); }
        bool ICollection<IWriteableNodeState>.Contains(IWriteableNodeState value) { return Contains((IFocusNodeState)value); }
        void ICollection<IWriteableNodeState>.CopyTo(IWriteableNodeState[] array, int index) { CopyTo((IFocusNodeState[])array, index); }
        bool ICollection<IWriteableNodeState>.IsReadOnly { get { return ((ICollection<IFocusNodeState>)this).IsReadOnly; } }
        bool ICollection<IWriteableNodeState>.Remove(IWriteableNodeState item) { return Remove((IFocusNodeState)item); }
        IEnumerator<IWriteableNodeState> IEnumerable<IWriteableNodeState>.GetEnumerator() { return GetEnumerator(); }
        IWriteableNodeState IReadOnlyList<IWriteableNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Frame
        IFrameNodeState IFrameNodeStateList.this[int index] { get { return this[index]; } set { this[index] = (IFocusNodeState)value; } }
        IEnumerator<IFrameNodeState> IFrameNodeStateList.GetEnumerator() { return GetEnumerator(); }
        IFrameNodeState IList<IFrameNodeState>.this[int index] { get { return this[index]; } set { this[index] = (IFocusNodeState)value; } }
        int IList<IFrameNodeState>.IndexOf(IFrameNodeState value) { return IndexOf((IFocusNodeState)value); }
        void IList<IFrameNodeState>.Insert(int index, IFrameNodeState item) { Insert(index, (IFocusNodeState)item); }
        void ICollection<IFrameNodeState>.Add(IFrameNodeState item) { Add((IFocusNodeState)item); }
        bool ICollection<IFrameNodeState>.Contains(IFrameNodeState value) { return Contains((IFocusNodeState)value); }
        void ICollection<IFrameNodeState>.CopyTo(IFrameNodeState[] array, int index) { CopyTo((IFocusNodeState[])array, index); }
        bool ICollection<IFrameNodeState>.IsReadOnly { get { return ((ICollection<IFocusNodeState>)this).IsReadOnly; } }
        bool ICollection<IFrameNodeState>.Remove(IFrameNodeState item) { return Remove((IFocusNodeState)item); }
        IEnumerator<IFrameNodeState> IEnumerable<IFrameNodeState>.GetEnumerator() { return GetEnumerator(); }
        IFrameNodeState IReadOnlyList<IFrameNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        public virtual IReadOnlyNodeStateReadOnlyList ToReadOnly()
        {
            return new FocusNodeStateReadOnlyList(this);
        }
    }
}
