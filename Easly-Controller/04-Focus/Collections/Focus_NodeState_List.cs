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
        new int Count { get; }
        new IFocusNodeState this[int index] { get; set; }
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
        IReadOnlyNodeState IReadOnlyList<IReadOnlyNodeState>.this[int index] { get { return this[index]; } }
        public void Add(IReadOnlyNodeState item) { base.Add((IFocusNodeState)item); }
        public void Insert(int index, IReadOnlyNodeState item) { base.Insert(index, (IFocusNodeState)item); }
        public bool Remove(IReadOnlyNodeState item) { return base.Remove((IFocusNodeState)item); }
        public void CopyTo(IReadOnlyNodeState[] array, int index) { base.CopyTo((IFocusNodeState[])array, index); }
        bool ICollection<IReadOnlyNodeState>.IsReadOnly { get { return ((ICollection<IFocusNodeState>)this).IsReadOnly; } }
        public bool Contains(IReadOnlyNodeState value) { return base.Contains((IFocusNodeState)value); }
        public int IndexOf(IReadOnlyNodeState value) { return base.IndexOf((IFocusNodeState)value); }
        IEnumerator<IReadOnlyNodeState> IEnumerable<IReadOnlyNodeState>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Writeable
        IWriteableNodeState IWriteableNodeStateList.this[int index] { get { return this[index]; } set { this[index] = (IFocusNodeState)value; } }
        IWriteableNodeState IList<IWriteableNodeState>.this[int index] { get { return this[index]; } set { this[index] = (IFocusNodeState)value; } }
        IWriteableNodeState IReadOnlyList<IWriteableNodeState>.this[int index] { get { return this[index]; } }
        public void Add(IWriteableNodeState item) { base.Add((IFocusNodeState)item); }
        public void Insert(int index, IWriteableNodeState item) { base.Insert(index, (IFocusNodeState)item); }
        public bool Remove(IWriteableNodeState item) { return base.Remove((IFocusNodeState)item); }
        public void CopyTo(IWriteableNodeState[] array, int index) { base.CopyTo((IFocusNodeState[])array, index); }
        bool ICollection<IWriteableNodeState>.IsReadOnly { get { return ((ICollection<IFocusNodeState>)this).IsReadOnly; } }
        public bool Contains(IWriteableNodeState value) { return base.Contains((IFocusNodeState)value); }
        public int IndexOf(IWriteableNodeState value) { return base.IndexOf((IFocusNodeState)value); }
        IEnumerator<IWriteableNodeState> IWriteableNodeStateList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IWriteableNodeState> IEnumerable<IWriteableNodeState>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Frame
        IFrameNodeState IFrameNodeStateList.this[int index] { get { return this[index]; } set { this[index] = (IFocusNodeState)value; } }
        IFrameNodeState IList<IFrameNodeState>.this[int index] { get { return this[index]; } set { this[index] = (IFocusNodeState)value; } }
        IFrameNodeState IReadOnlyList<IFrameNodeState>.this[int index] { get { return this[index]; } }
        public void Add(IFrameNodeState item) { base.Add((IFocusNodeState)item); }
        public void Insert(int index, IFrameNodeState item) { base.Insert(index, (IFocusNodeState)item); }
        public bool Remove(IFrameNodeState item) { return base.Remove((IFocusNodeState)item); }
        public void CopyTo(IFrameNodeState[] array, int index) { base.CopyTo((IFocusNodeState[])array, index); }
        bool ICollection<IFrameNodeState>.IsReadOnly { get { return ((ICollection<IFocusNodeState>)this).IsReadOnly; } }
        public bool Contains(IFrameNodeState value) { return base.Contains((IFocusNodeState)value); }
        public int IndexOf(IFrameNodeState value) { return base.IndexOf((IFocusNodeState)value); }
        IEnumerator<IFrameNodeState> IFrameNodeStateList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IFrameNodeState> IEnumerable<IFrameNodeState>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        public virtual IReadOnlyNodeStateReadOnlyList ToReadOnly()
        {
            return new FocusNodeStateReadOnlyList(this);
        }
    }
}
