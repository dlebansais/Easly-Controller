using EaslyController.ReadOnly;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.Writeable
{
    /// <summary>
    /// List of IxxxBrowsingListNodeIndex
    /// </summary>
    public interface IWriteableBrowsingListNodeIndexList : IReadOnlyBrowsingListNodeIndexList, IList<IWriteableBrowsingListNodeIndex>, IReadOnlyList<IWriteableBrowsingListNodeIndex>
    {
        new int Count { get; }
        new IWriteableBrowsingListNodeIndex this[int index] { get; set; }
    }

    /// <summary>
    /// List of IxxxBrowsingListNodeIndex
    /// </summary>
    public class WriteableBrowsingListNodeIndexList : Collection<IWriteableBrowsingListNodeIndex>, IWriteableBrowsingListNodeIndexList
    {
        public new IReadOnlyBrowsingListNodeIndex this[int index] { get { return base[index]; } set { base[index] = (IWriteableBrowsingListNodeIndex)value; } }
        public void Add(IReadOnlyBrowsingListNodeIndex item) { base.Add((IWriteableBrowsingListNodeIndex)item); }
        public void Insert(int index, IReadOnlyBrowsingListNodeIndex item) { base.Insert(index, (IWriteableBrowsingListNodeIndex)item); }
        public bool Remove(IReadOnlyBrowsingListNodeIndex item) { return base.Remove((IWriteableBrowsingListNodeIndex)item); }
        public void CopyTo(IReadOnlyBrowsingListNodeIndex[] array, int index) { base.CopyTo((IWriteableBrowsingListNodeIndex[])array, index); }
        bool ICollection<IReadOnlyBrowsingListNodeIndex>.IsReadOnly { get { return ((ICollection<IWriteableBrowsingListNodeIndex>)this).IsReadOnly; } }
        public bool Contains(IReadOnlyBrowsingListNodeIndex value) { return base.Contains((IWriteableBrowsingListNodeIndex)value); }
        public int IndexOf(IReadOnlyBrowsingListNodeIndex value) { return base.IndexOf((IWriteableBrowsingListNodeIndex)value); }
        public new IEnumerator<IReadOnlyBrowsingListNodeIndex> GetEnumerator() { return base.GetEnumerator(); }
    }
}
