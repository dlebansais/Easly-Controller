#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;

    /// <summary>
    /// List of IxxxBrowsingListNodeIndex
    /// </summary>
    internal class WriteableBrowsingListNodeIndexList : ReadOnlyBrowsingListNodeIndexList, ICollection<WriteableBrowsingListNodeIndex>, IEnumerable<WriteableBrowsingListNodeIndex>, IList<WriteableBrowsingListNodeIndex>, IReadOnlyCollection<WriteableBrowsingListNodeIndex>, IReadOnlyList<WriteableBrowsingListNodeIndex>
    {
        #region WriteableBrowsingListNodeIndex
        void ICollection<WriteableBrowsingListNodeIndex>.Add(WriteableBrowsingListNodeIndex item) { Add(item); }
        bool ICollection<WriteableBrowsingListNodeIndex>.Contains(WriteableBrowsingListNodeIndex item) { return Contains(item); }
        void ICollection<WriteableBrowsingListNodeIndex>.CopyTo(WriteableBrowsingListNodeIndex[] array, int arrayIndex) { for (int i = 0; i < Count; i++) array[arrayIndex + i] = (WriteableBrowsingListNodeIndex)this[i]; }
        bool ICollection<WriteableBrowsingListNodeIndex>.Remove(WriteableBrowsingListNodeIndex item) { return Remove(item); }
        bool ICollection<WriteableBrowsingListNodeIndex>.IsReadOnly { get { return false; } }
        IEnumerator<WriteableBrowsingListNodeIndex> IEnumerable<WriteableBrowsingListNodeIndex>.GetEnumerator() { return new List<WriteableBrowsingListNodeIndex>(this).GetEnumerator(); }
        WriteableBrowsingListNodeIndex IList<WriteableBrowsingListNodeIndex>.this[int index] { get { return (WriteableBrowsingListNodeIndex)this[index]; } set { this[index] = value; } }
        int IList<WriteableBrowsingListNodeIndex>.IndexOf(WriteableBrowsingListNodeIndex item) { return IndexOf(item); }
        void IList<WriteableBrowsingListNodeIndex>.Insert(int index, WriteableBrowsingListNodeIndex item) { Insert(index, item); }
        WriteableBrowsingListNodeIndex IReadOnlyList<WriteableBrowsingListNodeIndex>.this[int index] { get { return (WriteableBrowsingListNodeIndex)this[index]; } }
        #endregion
    }
}
