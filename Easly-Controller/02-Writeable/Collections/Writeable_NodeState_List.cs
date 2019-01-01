using EaslyController.ReadOnly;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.Writeable
{
    /// <summary>
    /// List of IxxxNodeState
    /// </summary>
    public interface IWriteableNodeStateList : IReadOnlyNodeStateList, IList<IWriteableNodeState>, IReadOnlyList<IWriteableNodeState>
    {
        new int Count { get; }
        new IWriteableNodeState this[int index] { get; set; }
    }

    /// <summary>
    /// List of IxxxNodeState
    /// </summary>
    public class WriteableNodeStateList : Collection<IWriteableNodeState>, IWriteableNodeStateList
    {
        #region ReadOnly
        public new IReadOnlyNodeState this[int index] { get { return base[index]; } set { base[index] = (IWriteableNodeState)value; } }
        public void Add(IReadOnlyNodeState item) { base.Add((IWriteableNodeState)item); }
        public void Insert(int index, IReadOnlyNodeState item) { base.Insert(index, (IWriteableNodeState)item); }
        public bool Remove(IReadOnlyNodeState item) { return base.Remove((IWriteableNodeState)item); }
        public void CopyTo(IReadOnlyNodeState[] array, int index) { base.CopyTo((IWriteableNodeState[])array, index); }
        bool ICollection<IReadOnlyNodeState>.IsReadOnly { get { return ((ICollection<IWriteableNodeState>)this).IsReadOnly; } }
        public bool Contains(IReadOnlyNodeState value) { return base.Contains((IWriteableNodeState)value); }
        public int IndexOf(IReadOnlyNodeState value) { return base.IndexOf((IWriteableNodeState)value); }
        public new IEnumerator<IReadOnlyNodeState> GetEnumerator() { return base.GetEnumerator(); }
        #endregion
    }
}
