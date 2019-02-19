#pragma warning disable 1591

namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Focus;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// List of IxxxNodeState
    /// </summary>
    public interface ILayoutNodeStateList : IFocusNodeStateList, IList<ILayoutNodeState>, IReadOnlyList<ILayoutNodeState>
    {
        new ILayoutNodeState this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<ILayoutNodeState> GetEnumerator();
        new void Clear();
    }

    /// <summary>
    /// List of IxxxNodeState
    /// </summary>
    internal class LayoutNodeStateList : Collection<ILayoutNodeState>, ILayoutNodeStateList
    {
        #region ReadOnly
        IReadOnlyNodeState IReadOnlyNodeStateList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutNodeState)value; } }
        IReadOnlyNodeState IList<IReadOnlyNodeState>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutNodeState)value; } }
        int IList<IReadOnlyNodeState>.IndexOf(IReadOnlyNodeState value) { return IndexOf((ILayoutNodeState)value); }
        void IList<IReadOnlyNodeState>.Insert(int index, IReadOnlyNodeState item) { Insert(index, (ILayoutNodeState)item); }
        void ICollection<IReadOnlyNodeState>.Add(IReadOnlyNodeState item) { Add((ILayoutNodeState)item); }
        bool ICollection<IReadOnlyNodeState>.Contains(IReadOnlyNodeState value) { return Contains((ILayoutNodeState)value); }
        void ICollection<IReadOnlyNodeState>.CopyTo(IReadOnlyNodeState[] array, int index) { CopyTo((ILayoutNodeState[])array, index); }
        bool ICollection<IReadOnlyNodeState>.IsReadOnly { get { return ((ICollection<ILayoutNodeState>)this).IsReadOnly; } }
        bool ICollection<IReadOnlyNodeState>.Remove(IReadOnlyNodeState item) { return Remove((ILayoutNodeState)item); }
        IEnumerator<IReadOnlyNodeState> IEnumerable<IReadOnlyNodeState>.GetEnumerator() { return GetEnumerator(); }
        IReadOnlyNodeState IReadOnlyList<IReadOnlyNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Writeable
        IWriteableNodeState IWriteableNodeStateList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutNodeState)value; } }
        IEnumerator<IWriteableNodeState> IWriteableNodeStateList.GetEnumerator() { return GetEnumerator(); }
        IWriteableNodeState IList<IWriteableNodeState>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutNodeState)value; } }
        int IList<IWriteableNodeState>.IndexOf(IWriteableNodeState value) { return IndexOf((ILayoutNodeState)value); }
        void IList<IWriteableNodeState>.Insert(int index, IWriteableNodeState item) { Insert(index, (ILayoutNodeState)item); }
        void ICollection<IWriteableNodeState>.Add(IWriteableNodeState item) { Add((ILayoutNodeState)item); }
        bool ICollection<IWriteableNodeState>.Contains(IWriteableNodeState value) { return Contains((ILayoutNodeState)value); }
        void ICollection<IWriteableNodeState>.CopyTo(IWriteableNodeState[] array, int index) { CopyTo((ILayoutNodeState[])array, index); }
        bool ICollection<IWriteableNodeState>.IsReadOnly { get { return ((ICollection<ILayoutNodeState>)this).IsReadOnly; } }
        bool ICollection<IWriteableNodeState>.Remove(IWriteableNodeState item) { return Remove((ILayoutNodeState)item); }
        IEnumerator<IWriteableNodeState> IEnumerable<IWriteableNodeState>.GetEnumerator() { return GetEnumerator(); }
        IWriteableNodeState IReadOnlyList<IWriteableNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Frame
        IFrameNodeState IFrameNodeStateList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutNodeState)value; } }
        IEnumerator<IFrameNodeState> IFrameNodeStateList.GetEnumerator() { return GetEnumerator(); }
        IFrameNodeState IList<IFrameNodeState>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutNodeState)value; } }
        int IList<IFrameNodeState>.IndexOf(IFrameNodeState value) { return IndexOf((ILayoutNodeState)value); }
        void IList<IFrameNodeState>.Insert(int index, IFrameNodeState item) { Insert(index, (ILayoutNodeState)item); }
        void ICollection<IFrameNodeState>.Add(IFrameNodeState item) { Add((ILayoutNodeState)item); }
        bool ICollection<IFrameNodeState>.Contains(IFrameNodeState value) { return Contains((ILayoutNodeState)value); }
        void ICollection<IFrameNodeState>.CopyTo(IFrameNodeState[] array, int index) { CopyTo((ILayoutNodeState[])array, index); }
        bool ICollection<IFrameNodeState>.IsReadOnly { get { return ((ICollection<ILayoutNodeState>)this).IsReadOnly; } }
        bool ICollection<IFrameNodeState>.Remove(IFrameNodeState item) { return Remove((ILayoutNodeState)item); }
        IEnumerator<IFrameNodeState> IEnumerable<IFrameNodeState>.GetEnumerator() { return GetEnumerator(); }
        IFrameNodeState IReadOnlyList<IFrameNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Focus
        IFocusNodeState IFocusNodeStateList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutNodeState)value; } }
        IEnumerator<IFocusNodeState> IFocusNodeStateList.GetEnumerator() { return GetEnumerator(); }
        IFocusNodeState IList<IFocusNodeState>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutNodeState)value; } }
        int IList<IFocusNodeState>.IndexOf(IFocusNodeState value) { return IndexOf((ILayoutNodeState)value); }
        void IList<IFocusNodeState>.Insert(int index, IFocusNodeState item) { Insert(index, (ILayoutNodeState)item); }
        void ICollection<IFocusNodeState>.Add(IFocusNodeState item) { Add((ILayoutNodeState)item); }
        bool ICollection<IFocusNodeState>.Contains(IFocusNodeState value) { return Contains((ILayoutNodeState)value); }
        void ICollection<IFocusNodeState>.CopyTo(IFocusNodeState[] array, int index) { CopyTo((ILayoutNodeState[])array, index); }
        bool ICollection<IFocusNodeState>.IsReadOnly { get { return ((ICollection<ILayoutNodeState>)this).IsReadOnly; } }
        bool ICollection<IFocusNodeState>.Remove(IFocusNodeState item) { return Remove((ILayoutNodeState)item); }
        IEnumerator<IFocusNodeState> IEnumerable<IFocusNodeState>.GetEnumerator() { return GetEnumerator(); }
        IFocusNodeState IReadOnlyList<IFocusNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        public virtual IReadOnlyNodeStateReadOnlyList ToReadOnly()
        {
            return new LayoutNodeStateReadOnlyList(this);
        }
    }
}
