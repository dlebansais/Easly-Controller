#pragma warning disable 1591

namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// List of IxxxNodeState
    /// </summary>
    public interface IFrameNodeStateList : IWriteableNodeStateList, IList<IFrameNodeState>, IReadOnlyList<IFrameNodeState>
    {
        new IFrameNodeState this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<IFrameNodeState> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxNodeState
    /// </summary>
    internal class FrameNodeStateList : Collection<IFrameNodeState>, IFrameNodeStateList
    {
        #region ReadOnly
        IReadOnlyNodeState IReadOnlyNodeStateList.this[int index] { get { return this[index]; } set { this[index] = (IFrameNodeState)value; } }
        IReadOnlyNodeState IList<IReadOnlyNodeState>.this[int index] { get { return this[index]; } set { this[index] = (IFrameNodeState)value; } }
        int IList<IReadOnlyNodeState>.IndexOf(IReadOnlyNodeState value) { return IndexOf((IFrameNodeState)value); }
        void IList<IReadOnlyNodeState>.Insert(int index, IReadOnlyNodeState item) { Insert(index, (IFrameNodeState)item); }
        void ICollection<IReadOnlyNodeState>.Add(IReadOnlyNodeState item) { Add((IFrameNodeState)item); }
        bool ICollection<IReadOnlyNodeState>.Contains(IReadOnlyNodeState value) { return Contains((IFrameNodeState)value); }
        void ICollection<IReadOnlyNodeState>.CopyTo(IReadOnlyNodeState[] array, int index) { CopyTo((IFrameNodeState[])array, index); }
        bool ICollection<IReadOnlyNodeState>.IsReadOnly { get { return ((ICollection<IFrameNodeState>)this).IsReadOnly; } }
        bool ICollection<IReadOnlyNodeState>.Remove(IReadOnlyNodeState item) { return Remove((IFrameNodeState)item); }
        IEnumerator<IReadOnlyNodeState> IEnumerable<IReadOnlyNodeState>.GetEnumerator() { return GetEnumerator(); }
        IReadOnlyNodeState IReadOnlyList<IReadOnlyNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Writeable
        IWriteableNodeState IWriteableNodeStateList.this[int index] { get { return this[index]; } set { this[index] = (IFrameNodeState)value; } }
        IEnumerator<IWriteableNodeState> IWriteableNodeStateList.GetEnumerator() { return GetEnumerator(); }
        IWriteableNodeState IList<IWriteableNodeState>.this[int index] { get { return this[index]; } set { this[index] = (IFrameNodeState)value; } }
        int IList<IWriteableNodeState>.IndexOf(IWriteableNodeState value) { return IndexOf((IFrameNodeState)value); }
        void IList<IWriteableNodeState>.Insert(int index, IWriteableNodeState item) { Insert(index, (IFrameNodeState)item); }
        void ICollection<IWriteableNodeState>.Add(IWriteableNodeState item) { Add((IFrameNodeState)item); }
        bool ICollection<IWriteableNodeState>.Contains(IWriteableNodeState value) { return Contains((IFrameNodeState)value); }
        void ICollection<IWriteableNodeState>.CopyTo(IWriteableNodeState[] array, int index) { CopyTo((IFrameNodeState[])array, index); }
        bool ICollection<IWriteableNodeState>.IsReadOnly { get { return ((ICollection<IFrameNodeState>)this).IsReadOnly; } }
        bool ICollection<IWriteableNodeState>.Remove(IWriteableNodeState item) { return Remove((IFrameNodeState)item); }
        IEnumerator<IWriteableNodeState> IEnumerable<IWriteableNodeState>.GetEnumerator() { return GetEnumerator(); }
        IWriteableNodeState IReadOnlyList<IWriteableNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        public virtual IReadOnlyNodeStateReadOnlyList ToReadOnly()
        {
            return new FrameNodeStateReadOnlyList(this);
        }
    }
}
