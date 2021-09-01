#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;

    /// <summary>
    /// List of IxxxBrowsingBlockNodeIndex
    /// </summary>
    internal class WriteableBrowsingBlockNodeIndexList : ReadOnlyBrowsingBlockNodeIndexList, ICollection<WriteableBrowsingBlockNodeIndex>, IEnumerable<WriteableBrowsingBlockNodeIndex>, IList<WriteableBrowsingBlockNodeIndex>, IReadOnlyCollection<WriteableBrowsingBlockNodeIndex>, IReadOnlyList<WriteableBrowsingBlockNodeIndex>
    {
        #region WriteableBrowsingBlockNodeIndex
        void ICollection<WriteableBrowsingBlockNodeIndex>.Add(WriteableBrowsingBlockNodeIndex item) { Add(item); }
        bool ICollection<WriteableBrowsingBlockNodeIndex>.Contains(WriteableBrowsingBlockNodeIndex item) { return Contains(item); }
        void ICollection<WriteableBrowsingBlockNodeIndex>.CopyTo(WriteableBrowsingBlockNodeIndex[] array, int arrayIndex) { for (int i = 0; i < Count; i++) array[arrayIndex + i] = (WriteableBrowsingBlockNodeIndex)this[i]; }
        bool ICollection<WriteableBrowsingBlockNodeIndex>.Remove(WriteableBrowsingBlockNodeIndex item) { return Remove(item); }
        bool ICollection<WriteableBrowsingBlockNodeIndex>.IsReadOnly { get { return false; } }
        IEnumerator<WriteableBrowsingBlockNodeIndex> IEnumerable<WriteableBrowsingBlockNodeIndex>.GetEnumerator() { return new List<WriteableBrowsingBlockNodeIndex>(this).GetEnumerator(); }
        WriteableBrowsingBlockNodeIndex IList<WriteableBrowsingBlockNodeIndex>.this[int index] { get { return (WriteableBrowsingBlockNodeIndex)this[index]; } set { this[index] = value; } }
        int IList<WriteableBrowsingBlockNodeIndex>.IndexOf(WriteableBrowsingBlockNodeIndex item) { return IndexOf(item); }
        void IList<WriteableBrowsingBlockNodeIndex>.Insert(int index, WriteableBrowsingBlockNodeIndex item) { Insert(index, item); }
        WriteableBrowsingBlockNodeIndex IReadOnlyList<WriteableBrowsingBlockNodeIndex>.this[int index] { get { return (WriteableBrowsingBlockNodeIndex)this[index]; } }
        #endregion
    }
}
