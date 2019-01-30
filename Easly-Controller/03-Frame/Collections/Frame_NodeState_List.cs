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
        new int Count { get; }
        new IFrameNodeState this[int index] { get; set; }
        new IEnumerator<IFrameNodeState> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxNodeState
    /// </summary>
    public class FrameNodeStateList : Collection<IFrameNodeState>, IFrameNodeStateList
    {
        #region ReadOnly
        public new IReadOnlyNodeState this[int index] { get { return base[index]; } set { base[index] = (IFrameNodeState)value; } }
        public void Add(IReadOnlyNodeState item) { base.Add((IFrameNodeState)item); }
        public void Insert(int index, IReadOnlyNodeState item) { base.Insert(index, (IFrameNodeState)item); }
        public bool Remove(IReadOnlyNodeState item) { return base.Remove((IFrameNodeState)item); }
        public void CopyTo(IReadOnlyNodeState[] array, int index) { base.CopyTo((IFrameNodeState[])array, index); }
        bool ICollection<IReadOnlyNodeState>.IsReadOnly { get { return ((ICollection<IFrameNodeState>)this).IsReadOnly; } }
        public bool Contains(IReadOnlyNodeState value) { return base.Contains((IFrameNodeState)value); }
        public int IndexOf(IReadOnlyNodeState value) { return base.IndexOf((IFrameNodeState)value); }
        public new IEnumerator<IReadOnlyNodeState> GetEnumerator() { return base.GetEnumerator(); }
        #endregion

        #region Writeable
        IWriteableNodeState IWriteableNodeStateList.this[int index] { get { return base[index]; } set { base[index] = (IFrameNodeState)value; } }
        IWriteableNodeState IList<IWriteableNodeState>.this[int index] { get { return base[index]; } set { base[index] = (IFrameNodeState)value; } }
        IWriteableNodeState IReadOnlyList<IWriteableNodeState>.this[int index] { get { return base[index]; } }
        public void Add(IWriteableNodeState item) { base.Add((IFrameNodeState)item); }
        public void Insert(int index, IWriteableNodeState item) { base.Insert(index, (IFrameNodeState)item); }
        public bool Remove(IWriteableNodeState item) { return base.Remove((IFrameNodeState)item); }
        public void CopyTo(IWriteableNodeState[] array, int index) { base.CopyTo((IFrameNodeState[])array, index); }
        bool ICollection<IWriteableNodeState>.IsReadOnly { get { return ((ICollection<IFrameNodeState>)this).IsReadOnly; } }
        public bool Contains(IWriteableNodeState value) { return base.Contains((IFrameNodeState)value); }
        public int IndexOf(IWriteableNodeState value) { return base.IndexOf((IFrameNodeState)value); }
        IEnumerator<IWriteableNodeState> IWriteableNodeStateList.GetEnumerator() { return base.GetEnumerator(); }
        IEnumerator<IWriteableNodeState> IEnumerable<IWriteableNodeState>.GetEnumerator() { return base.GetEnumerator(); }
        #endregion
    }
}
