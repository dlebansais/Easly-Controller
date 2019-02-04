#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;

    /// <summary>
    /// List of IxxxBrowsingBlockNodeIndex
    /// </summary>
    public interface IWriteableBrowsingBlockNodeIndexList : IReadOnlyBrowsingBlockNodeIndexList, IList<IWriteableBrowsingBlockNodeIndex>, IReadOnlyList<IWriteableBrowsingBlockNodeIndex>
    {
        new int Count { get; }
        new IWriteableBrowsingBlockNodeIndex this[int index] { get; set; }
        new IEnumerator<IWriteableBrowsingBlockNodeIndex> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxBrowsingBlockNodeIndex
    /// </summary>
    internal class WriteableBrowsingBlockNodeIndexList : Collection<IWriteableBrowsingBlockNodeIndex>, IWriteableBrowsingBlockNodeIndexList
    {
        #region ReadOnly
        IReadOnlyBrowsingBlockNodeIndex IReadOnlyBrowsingBlockNodeIndexList.this[int index] { get { return this[index]; } set { this[index] = (IWriteableBrowsingBlockNodeIndex)value; } }
        IReadOnlyBrowsingBlockNodeIndex IList<IReadOnlyBrowsingBlockNodeIndex>.this[int index] { get { return this[index]; } set { this[index] = (IWriteableBrowsingBlockNodeIndex)value; } }
        IReadOnlyBrowsingBlockNodeIndex IReadOnlyList<IReadOnlyBrowsingBlockNodeIndex>.this[int index] { get { return this[index]; } }
        public void Add(IReadOnlyBrowsingBlockNodeIndex item) { base.Add((IWriteableBrowsingBlockNodeIndex)item); }
        public void Insert(int index, IReadOnlyBrowsingBlockNodeIndex item) { base.Insert(index, (IWriteableBrowsingBlockNodeIndex)item); }
        public bool Remove(IReadOnlyBrowsingBlockNodeIndex item) { return base.Remove((IWriteableBrowsingBlockNodeIndex)item); }
        public void CopyTo(IReadOnlyBrowsingBlockNodeIndex[] array, int index) { base.CopyTo((IWriteableBrowsingBlockNodeIndex[])array, index); }
        bool ICollection<IReadOnlyBrowsingBlockNodeIndex>.IsReadOnly { get { return ((ICollection<IWriteableBrowsingBlockNodeIndex>)this).IsReadOnly; } }
        public bool Contains(IReadOnlyBrowsingBlockNodeIndex value) { return base.Contains((IWriteableBrowsingBlockNodeIndex)value); }
        public int IndexOf(IReadOnlyBrowsingBlockNodeIndex value) { return base.IndexOf((IWriteableBrowsingBlockNodeIndex)value); }
        IEnumerator<IReadOnlyBrowsingBlockNodeIndex> IEnumerable<IReadOnlyBrowsingBlockNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        #endregion
    }
}