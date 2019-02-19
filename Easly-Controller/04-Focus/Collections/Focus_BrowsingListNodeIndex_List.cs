#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// List of IxxxBrowsingListNodeIndex
    /// </summary>
    public interface IFocusBrowsingListNodeIndexList : IFrameBrowsingListNodeIndexList, IList<IFocusBrowsingListNodeIndex>, IReadOnlyList<IFocusBrowsingListNodeIndex>
    {
        new IFocusBrowsingListNodeIndex this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<IFocusBrowsingListNodeIndex> GetEnumerator();
        new void Clear();
    }

    /// <summary>
    /// List of IxxxBrowsingListNodeIndex
    /// </summary>
    internal class FocusBrowsingListNodeIndexList : Collection<IFocusBrowsingListNodeIndex>, IFocusBrowsingListNodeIndexList
    {
        #region ReadOnly
        IReadOnlyBrowsingListNodeIndex IReadOnlyBrowsingListNodeIndexList.this[int index] { get { return this[index]; } set { this[index] = (IFocusBrowsingListNodeIndex)value; } }
        IReadOnlyBrowsingListNodeIndex IList<IReadOnlyBrowsingListNodeIndex>.this[int index] { get { return this[index]; } set { this[index] = (IFocusBrowsingListNodeIndex)value; } }
        int IList<IReadOnlyBrowsingListNodeIndex>.IndexOf(IReadOnlyBrowsingListNodeIndex value) { return IndexOf((IFocusBrowsingListNodeIndex)value); }
        void IList<IReadOnlyBrowsingListNodeIndex>.Insert(int index, IReadOnlyBrowsingListNodeIndex item) { Insert(index, (IFocusBrowsingListNodeIndex)item); }
        void ICollection<IReadOnlyBrowsingListNodeIndex>.Add(IReadOnlyBrowsingListNodeIndex item) { Add((IFocusBrowsingListNodeIndex)item); }
        bool ICollection<IReadOnlyBrowsingListNodeIndex>.Contains(IReadOnlyBrowsingListNodeIndex value) { return Contains((IFocusBrowsingListNodeIndex)value); }
        void ICollection<IReadOnlyBrowsingListNodeIndex>.CopyTo(IReadOnlyBrowsingListNodeIndex[] array, int index) { CopyTo((IFocusBrowsingListNodeIndex[])array, index); }
        bool ICollection<IReadOnlyBrowsingListNodeIndex>.IsReadOnly { get { return ((ICollection<IFocusBrowsingListNodeIndex>)this).IsReadOnly; } }
        bool ICollection<IReadOnlyBrowsingListNodeIndex>.Remove(IReadOnlyBrowsingListNodeIndex item) { return Remove((IFocusBrowsingListNodeIndex)item); }
        IEnumerator<IReadOnlyBrowsingListNodeIndex> IEnumerable<IReadOnlyBrowsingListNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        IReadOnlyBrowsingListNodeIndex IReadOnlyList<IReadOnlyBrowsingListNodeIndex>.this[int index] { get { return this[index]; } }
        #endregion

        #region Writeable
        IWriteableBrowsingListNodeIndex IWriteableBrowsingListNodeIndexList.this[int index] { get { return this[index]; } set { this[index] = (IFocusBrowsingListNodeIndex)value; } }
        IEnumerator<IWriteableBrowsingListNodeIndex> IWriteableBrowsingListNodeIndexList.GetEnumerator() { return GetEnumerator(); }
        IWriteableBrowsingListNodeIndex IList<IWriteableBrowsingListNodeIndex>.this[int index] { get { return this[index]; } set { this[index] = (IFocusBrowsingListNodeIndex)value; } }
        int IList<IWriteableBrowsingListNodeIndex>.IndexOf(IWriteableBrowsingListNodeIndex value) { return IndexOf((IFocusBrowsingListNodeIndex)value); }
        void IList<IWriteableBrowsingListNodeIndex>.Insert(int index, IWriteableBrowsingListNodeIndex item) { Insert(index, (IFocusBrowsingListNodeIndex)item); }
        void ICollection<IWriteableBrowsingListNodeIndex>.Add(IWriteableBrowsingListNodeIndex item) { Add((IFocusBrowsingListNodeIndex)item); }
        bool ICollection<IWriteableBrowsingListNodeIndex>.Contains(IWriteableBrowsingListNodeIndex value) { return Contains((IFocusBrowsingListNodeIndex)value); }
        void ICollection<IWriteableBrowsingListNodeIndex>.CopyTo(IWriteableBrowsingListNodeIndex[] array, int index) { CopyTo((IFocusBrowsingListNodeIndex[])array, index); }
        bool ICollection<IWriteableBrowsingListNodeIndex>.IsReadOnly { get { return ((ICollection<IFocusBrowsingListNodeIndex>)this).IsReadOnly; } }
        bool ICollection<IWriteableBrowsingListNodeIndex>.Remove(IWriteableBrowsingListNodeIndex item) { return Remove((IFocusBrowsingListNodeIndex)item); }
        IEnumerator<IWriteableBrowsingListNodeIndex> IEnumerable<IWriteableBrowsingListNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        IWriteableBrowsingListNodeIndex IReadOnlyList<IWriteableBrowsingListNodeIndex>.this[int index] { get { return this[index]; } }
        #endregion

        #region Frame
        IFrameBrowsingListNodeIndex IFrameBrowsingListNodeIndexList.this[int index] { get { return this[index]; } set { this[index] = (IFocusBrowsingListNodeIndex)value; } }
        IEnumerator<IFrameBrowsingListNodeIndex> IFrameBrowsingListNodeIndexList.GetEnumerator() { return GetEnumerator(); }
        IFrameBrowsingListNodeIndex IList<IFrameBrowsingListNodeIndex>.this[int index] { get { return this[index]; } set { this[index] = (IFocusBrowsingListNodeIndex)value; } }
        int IList<IFrameBrowsingListNodeIndex>.IndexOf(IFrameBrowsingListNodeIndex value) { return IndexOf((IFocusBrowsingListNodeIndex)value); }
        void IList<IFrameBrowsingListNodeIndex>.Insert(int index, IFrameBrowsingListNodeIndex item) { Insert(index, (IFocusBrowsingListNodeIndex)item); }
        void ICollection<IFrameBrowsingListNodeIndex>.Add(IFrameBrowsingListNodeIndex item) { Add((IFocusBrowsingListNodeIndex)item); }
        bool ICollection<IFrameBrowsingListNodeIndex>.Contains(IFrameBrowsingListNodeIndex value) { return Contains((IFocusBrowsingListNodeIndex)value); }
        void ICollection<IFrameBrowsingListNodeIndex>.CopyTo(IFrameBrowsingListNodeIndex[] array, int index) { CopyTo((IFocusBrowsingListNodeIndex[])array, index); }
        bool ICollection<IFrameBrowsingListNodeIndex>.IsReadOnly { get { return ((ICollection<IFocusBrowsingListNodeIndex>)this).IsReadOnly; } }
        bool ICollection<IFrameBrowsingListNodeIndex>.Remove(IFrameBrowsingListNodeIndex item) { return Remove((IFocusBrowsingListNodeIndex)item); }
        IEnumerator<IFrameBrowsingListNodeIndex> IEnumerable<IFrameBrowsingListNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        IFrameBrowsingListNodeIndex IReadOnlyList<IFrameBrowsingListNodeIndex>.this[int index] { get { return this[index]; } }
        #endregion
    }
}
