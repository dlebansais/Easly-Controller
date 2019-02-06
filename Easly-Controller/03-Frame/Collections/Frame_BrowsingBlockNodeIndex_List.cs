#pragma warning disable 1591

namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// List of IxxxBrowsingBlockNodeIndex
    /// </summary>
    public interface IFrameBrowsingBlockNodeIndexList : IWriteableBrowsingBlockNodeIndexList, IList<IFrameBrowsingBlockNodeIndex>, IReadOnlyList<IFrameBrowsingBlockNodeIndex>
    {
        new int Count { get; }
        new IFrameBrowsingBlockNodeIndex this[int index] { get; set; }
        new IEnumerator<IFrameBrowsingBlockNodeIndex> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxBrowsingBlockNodeIndex
    /// </summary>
    internal class FrameBrowsingBlockNodeIndexList : Collection<IFrameBrowsingBlockNodeIndex>, IFrameBrowsingBlockNodeIndexList
    {
        #region ReadOnly
        IReadOnlyBrowsingBlockNodeIndex IReadOnlyBrowsingBlockNodeIndexList.this[int index] { get { return this[index]; } set { this[index] = (IFrameBrowsingBlockNodeIndex)value; } }
        IReadOnlyBrowsingBlockNodeIndex IList<IReadOnlyBrowsingBlockNodeIndex>.this[int index] { get { return this[index]; } set { this[index] = (IFrameBrowsingBlockNodeIndex)value; } }
        IReadOnlyBrowsingBlockNodeIndex IReadOnlyList<IReadOnlyBrowsingBlockNodeIndex>.this[int index] { get { return this[index]; } }
        void ICollection<IReadOnlyBrowsingBlockNodeIndex>.Add(IReadOnlyBrowsingBlockNodeIndex item) { Add((IFrameBrowsingBlockNodeIndex)item); }
        void IList<IReadOnlyBrowsingBlockNodeIndex>.Insert(int index, IReadOnlyBrowsingBlockNodeIndex item) { Insert(index, (IFrameBrowsingBlockNodeIndex)item); }
        bool ICollection<IReadOnlyBrowsingBlockNodeIndex>.Remove(IReadOnlyBrowsingBlockNodeIndex item) { return Remove((IFrameBrowsingBlockNodeIndex)item); }
        void ICollection<IReadOnlyBrowsingBlockNodeIndex>.CopyTo(IReadOnlyBrowsingBlockNodeIndex[] array, int index) { CopyTo((IFrameBrowsingBlockNodeIndex[])array, index); }
        bool ICollection<IReadOnlyBrowsingBlockNodeIndex>.IsReadOnly { get { return ((ICollection<IFrameBrowsingBlockNodeIndex>)this).IsReadOnly; } }
        bool ICollection<IReadOnlyBrowsingBlockNodeIndex>.Contains(IReadOnlyBrowsingBlockNodeIndex value) { return Contains((IFrameBrowsingBlockNodeIndex)value); }
        int IList<IReadOnlyBrowsingBlockNodeIndex>.IndexOf(IReadOnlyBrowsingBlockNodeIndex value) { return IndexOf((IFrameBrowsingBlockNodeIndex)value); }
        IEnumerator<IReadOnlyBrowsingBlockNodeIndex> IEnumerable<IReadOnlyBrowsingBlockNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Writeable
        IWriteableBrowsingBlockNodeIndex IWriteableBrowsingBlockNodeIndexList.this[int index] { get { return this[index]; } set { this[index] = (IFrameBrowsingBlockNodeIndex)value; } }
        IWriteableBrowsingBlockNodeIndex IList<IWriteableBrowsingBlockNodeIndex>.this[int index] { get { return this[index]; } set { this[index] = (IFrameBrowsingBlockNodeIndex)value; } }
        IWriteableBrowsingBlockNodeIndex IReadOnlyList<IWriteableBrowsingBlockNodeIndex>.this[int index] { get { return this[index]; } }
        void ICollection<IWriteableBrowsingBlockNodeIndex>.Add(IWriteableBrowsingBlockNodeIndex item) { Add((IFrameBrowsingBlockNodeIndex)item); }
        void IList<IWriteableBrowsingBlockNodeIndex>.Insert(int index, IWriteableBrowsingBlockNodeIndex item) { Insert(index, (IFrameBrowsingBlockNodeIndex)item); }
        bool ICollection<IWriteableBrowsingBlockNodeIndex>.Remove(IWriteableBrowsingBlockNodeIndex item) { return Remove((IFrameBrowsingBlockNodeIndex)item); }
        void ICollection<IWriteableBrowsingBlockNodeIndex>.CopyTo(IWriteableBrowsingBlockNodeIndex[] array, int index) { CopyTo((IFrameBrowsingBlockNodeIndex[])array, index); }
        bool ICollection<IWriteableBrowsingBlockNodeIndex>.IsReadOnly { get { return ((ICollection<IFrameBrowsingBlockNodeIndex>)this).IsReadOnly; } }
        bool ICollection<IWriteableBrowsingBlockNodeIndex>.Contains(IWriteableBrowsingBlockNodeIndex value) { return Contains((IFrameBrowsingBlockNodeIndex)value); }
        int IList<IWriteableBrowsingBlockNodeIndex>.IndexOf(IWriteableBrowsingBlockNodeIndex value) { return IndexOf((IFrameBrowsingBlockNodeIndex)value); }
        IEnumerator<IWriteableBrowsingBlockNodeIndex> IWriteableBrowsingBlockNodeIndexList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IWriteableBrowsingBlockNodeIndex> IEnumerable<IWriteableBrowsingBlockNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        #endregion
    }
}
