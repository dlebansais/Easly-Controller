#pragma warning disable 1591

namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// List of IxxxBrowsingListNodeIndex
    /// </summary>
    public interface IFrameBrowsingListNodeIndexList : IWriteableBrowsingListNodeIndexList, IList<IFrameBrowsingListNodeIndex>, IReadOnlyList<IFrameBrowsingListNodeIndex>
    {
        new IFrameBrowsingListNodeIndex this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<IFrameBrowsingListNodeIndex> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxBrowsingListNodeIndex
    /// </summary>
    internal class FrameBrowsingListNodeIndexList : Collection<IFrameBrowsingListNodeIndex>, IFrameBrowsingListNodeIndexList
    {
        #region ReadOnly
        IReadOnlyBrowsingListNodeIndex IReadOnlyBrowsingListNodeIndexList.this[int index] { get { return this[index]; } set { this[index] = (IFrameBrowsingListNodeIndex)value; } }
        IReadOnlyBrowsingListNodeIndex IList<IReadOnlyBrowsingListNodeIndex>.this[int index] { get { return this[index]; } set { this[index] = (IFrameBrowsingListNodeIndex)value; } }
        int IList<IReadOnlyBrowsingListNodeIndex>.IndexOf(IReadOnlyBrowsingListNodeIndex value) { return IndexOf((IFrameBrowsingListNodeIndex)value); }
        void IList<IReadOnlyBrowsingListNodeIndex>.Insert(int index, IReadOnlyBrowsingListNodeIndex item) { Insert(index, (IFrameBrowsingListNodeIndex)item); }
        void ICollection<IReadOnlyBrowsingListNodeIndex>.Add(IReadOnlyBrowsingListNodeIndex item) { Add((IFrameBrowsingListNodeIndex)item); }
        bool ICollection<IReadOnlyBrowsingListNodeIndex>.Contains(IReadOnlyBrowsingListNodeIndex value) { return Contains((IFrameBrowsingListNodeIndex)value); }
        void ICollection<IReadOnlyBrowsingListNodeIndex>.CopyTo(IReadOnlyBrowsingListNodeIndex[] array, int index) { CopyTo((IFrameBrowsingListNodeIndex[])array, index); }
        bool ICollection<IReadOnlyBrowsingListNodeIndex>.IsReadOnly { get { return ((ICollection<IFrameBrowsingListNodeIndex>)this).IsReadOnly; } }
        bool ICollection<IReadOnlyBrowsingListNodeIndex>.Remove(IReadOnlyBrowsingListNodeIndex item) { return Remove((IFrameBrowsingListNodeIndex)item); }
        IEnumerator<IReadOnlyBrowsingListNodeIndex> IEnumerable<IReadOnlyBrowsingListNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        IReadOnlyBrowsingListNodeIndex IReadOnlyList<IReadOnlyBrowsingListNodeIndex>.this[int index] { get { return this[index]; } }
        #endregion

        #region Writeable
        IWriteableBrowsingListNodeIndex IWriteableBrowsingListNodeIndexList.this[int index] { get { return this[index]; } set { this[index] = (IFrameBrowsingListNodeIndex)value; } }
        IEnumerator<IWriteableBrowsingListNodeIndex> IWriteableBrowsingListNodeIndexList.GetEnumerator() { return GetEnumerator(); }
        IWriteableBrowsingListNodeIndex IList<IWriteableBrowsingListNodeIndex>.this[int index] { get { return this[index]; } set { this[index] = (IFrameBrowsingListNodeIndex)value; } }
        int IList<IWriteableBrowsingListNodeIndex>.IndexOf(IWriteableBrowsingListNodeIndex value) { return IndexOf((IFrameBrowsingListNodeIndex)value); }
        void IList<IWriteableBrowsingListNodeIndex>.Insert(int index, IWriteableBrowsingListNodeIndex item) { Insert(index, (IFrameBrowsingListNodeIndex)item); }
        void ICollection<IWriteableBrowsingListNodeIndex>.Add(IWriteableBrowsingListNodeIndex item) { Add((IFrameBrowsingListNodeIndex)item); }
        bool ICollection<IWriteableBrowsingListNodeIndex>.Contains(IWriteableBrowsingListNodeIndex value) { return Contains((IFrameBrowsingListNodeIndex)value); }
        void ICollection<IWriteableBrowsingListNodeIndex>.CopyTo(IWriteableBrowsingListNodeIndex[] array, int index) { CopyTo((IFrameBrowsingListNodeIndex[])array, index); }
        bool ICollection<IWriteableBrowsingListNodeIndex>.IsReadOnly { get { return ((ICollection<IFrameBrowsingListNodeIndex>)this).IsReadOnly; } }
        bool ICollection<IWriteableBrowsingListNodeIndex>.Remove(IWriteableBrowsingListNodeIndex item) { return Remove((IFrameBrowsingListNodeIndex)item); }
        IEnumerator<IWriteableBrowsingListNodeIndex> IEnumerable<IWriteableBrowsingListNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        IWriteableBrowsingListNodeIndex IReadOnlyList<IWriteableBrowsingListNodeIndex>.this[int index] { get { return this[index]; } }
        #endregion
    }
}
