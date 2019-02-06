#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;

    /// <summary>
    /// List of IxxxBrowsingListNodeIndex
    /// </summary>
    public interface IWriteableBrowsingListNodeIndexList : IReadOnlyBrowsingListNodeIndexList, IList<IWriteableBrowsingListNodeIndex>, IReadOnlyList<IWriteableBrowsingListNodeIndex>
    {
        new IWriteableBrowsingListNodeIndex this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<IWriteableBrowsingListNodeIndex> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxBrowsingListNodeIndex
    /// </summary>
    internal class WriteableBrowsingListNodeIndexList : Collection<IWriteableBrowsingListNodeIndex>, IWriteableBrowsingListNodeIndexList
    {
        #region ReadOnly
        IReadOnlyBrowsingListNodeIndex IReadOnlyBrowsingListNodeIndexList.this[int index] { get { return this[index]; } set { this[index] = (IWriteableBrowsingListNodeIndex)value; } }
        IReadOnlyBrowsingListNodeIndex IList<IReadOnlyBrowsingListNodeIndex>.this[int index] { get { return this[index]; } set { this[index] = (IWriteableBrowsingListNodeIndex)value; } }
        int IList<IReadOnlyBrowsingListNodeIndex>.IndexOf(IReadOnlyBrowsingListNodeIndex value) { return IndexOf((IWriteableBrowsingListNodeIndex)value); }
        void IList<IReadOnlyBrowsingListNodeIndex>.Insert(int index, IReadOnlyBrowsingListNodeIndex item) { Insert(index, (IWriteableBrowsingListNodeIndex)item); }
        void ICollection<IReadOnlyBrowsingListNodeIndex>.Add(IReadOnlyBrowsingListNodeIndex item) { Add((IWriteableBrowsingListNodeIndex)item); }
        bool ICollection<IReadOnlyBrowsingListNodeIndex>.Contains(IReadOnlyBrowsingListNodeIndex value) { return Contains((IWriteableBrowsingListNodeIndex)value); }
        void ICollection<IReadOnlyBrowsingListNodeIndex>.CopyTo(IReadOnlyBrowsingListNodeIndex[] array, int index) { CopyTo((IWriteableBrowsingListNodeIndex[])array, index); }
        bool ICollection<IReadOnlyBrowsingListNodeIndex>.IsReadOnly { get { return ((ICollection<IWriteableBrowsingListNodeIndex>)this).IsReadOnly; } }
        bool ICollection<IReadOnlyBrowsingListNodeIndex>.Remove(IReadOnlyBrowsingListNodeIndex item) { return Remove((IWriteableBrowsingListNodeIndex)item); }
        IEnumerator<IReadOnlyBrowsingListNodeIndex> IEnumerable<IReadOnlyBrowsingListNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        IReadOnlyBrowsingListNodeIndex IReadOnlyList<IReadOnlyBrowsingListNodeIndex>.this[int index] { get { return this[index]; } }
        #endregion
    }
}
