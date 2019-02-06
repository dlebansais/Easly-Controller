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
        void ICollection<IReadOnlyBrowsingBlockNodeIndex>.Add(IReadOnlyBrowsingBlockNodeIndex item) { Add((IWriteableBrowsingBlockNodeIndex)item); }
        void IList<IReadOnlyBrowsingBlockNodeIndex>.Insert(int index, IReadOnlyBrowsingBlockNodeIndex item) { Insert(index, (IWriteableBrowsingBlockNodeIndex)item); }
        bool ICollection<IReadOnlyBrowsingBlockNodeIndex>.Remove(IReadOnlyBrowsingBlockNodeIndex item) { return Remove((IWriteableBrowsingBlockNodeIndex)item); }
        void ICollection<IReadOnlyBrowsingBlockNodeIndex>.CopyTo(IReadOnlyBrowsingBlockNodeIndex[] array, int index) { CopyTo((IWriteableBrowsingBlockNodeIndex[])array, index); }
        bool ICollection<IReadOnlyBrowsingBlockNodeIndex>.IsReadOnly { get { return ((ICollection<IWriteableBrowsingBlockNodeIndex>)this).IsReadOnly; } }
        bool ICollection<IReadOnlyBrowsingBlockNodeIndex>.Contains(IReadOnlyBrowsingBlockNodeIndex value) { return Contains((IWriteableBrowsingBlockNodeIndex)value); }
        int IList<IReadOnlyBrowsingBlockNodeIndex>.IndexOf(IReadOnlyBrowsingBlockNodeIndex value) { return IndexOf((IWriteableBrowsingBlockNodeIndex)value); }
        IEnumerator<IReadOnlyBrowsingBlockNodeIndex> IEnumerable<IReadOnlyBrowsingBlockNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        #endregion
    }
}
