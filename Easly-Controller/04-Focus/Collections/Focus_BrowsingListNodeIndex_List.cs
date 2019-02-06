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
        new int Count { get; }
        new IFocusBrowsingListNodeIndex this[int index] { get; set; }
        new IEnumerator<IFocusBrowsingListNodeIndex> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxBrowsingListNodeIndex
    /// </summary>
    internal class FocusBrowsingListNodeIndexList : Collection<IFocusBrowsingListNodeIndex>, IFocusBrowsingListNodeIndexList
    {
        #region ReadOnly
        IReadOnlyBrowsingListNodeIndex IReadOnlyBrowsingListNodeIndexList.this[int index] { get { return this[index]; } set { this[index] = (IFocusBrowsingListNodeIndex)value; } }
        IReadOnlyBrowsingListNodeIndex IList<IReadOnlyBrowsingListNodeIndex>.this[int index] { get { return this[index]; } set { this[index] = (IFocusBrowsingListNodeIndex)value; } }
        IReadOnlyBrowsingListNodeIndex IReadOnlyList<IReadOnlyBrowsingListNodeIndex>.this[int index] { get { return this[index]; } }
        void ICollection<IReadOnlyBrowsingListNodeIndex>.Add(IReadOnlyBrowsingListNodeIndex item) { Add((IFocusBrowsingListNodeIndex)item); }
        void IList<IReadOnlyBrowsingListNodeIndex>.Insert(int index, IReadOnlyBrowsingListNodeIndex item) { Insert(index, (IFocusBrowsingListNodeIndex)item); }
        bool ICollection<IReadOnlyBrowsingListNodeIndex>.Remove(IReadOnlyBrowsingListNodeIndex item) { return Remove((IFocusBrowsingListNodeIndex)item); }
        void ICollection<IReadOnlyBrowsingListNodeIndex>.CopyTo(IReadOnlyBrowsingListNodeIndex[] array, int index) { CopyTo((IFocusBrowsingListNodeIndex[])array, index); }
        bool ICollection<IReadOnlyBrowsingListNodeIndex>.IsReadOnly { get { return ((ICollection<IFocusBrowsingListNodeIndex>)this).IsReadOnly; } }
        bool ICollection<IReadOnlyBrowsingListNodeIndex>.Contains(IReadOnlyBrowsingListNodeIndex value) { return Contains((IFocusBrowsingListNodeIndex)value); }
        int IList<IReadOnlyBrowsingListNodeIndex>.IndexOf(IReadOnlyBrowsingListNodeIndex value) { return IndexOf((IFocusBrowsingListNodeIndex)value); }
        IEnumerator<IReadOnlyBrowsingListNodeIndex> IEnumerable<IReadOnlyBrowsingListNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Writeable
        IWriteableBrowsingListNodeIndex IWriteableBrowsingListNodeIndexList.this[int index] { get { return this[index]; } set { this[index] = (IFocusBrowsingListNodeIndex)value; } }
        IWriteableBrowsingListNodeIndex IList<IWriteableBrowsingListNodeIndex>.this[int index] { get { return this[index]; } set { this[index] = (IFocusBrowsingListNodeIndex)value; } }
        IWriteableBrowsingListNodeIndex IReadOnlyList<IWriteableBrowsingListNodeIndex>.this[int index] { get { return this[index]; } }
        void ICollection<IWriteableBrowsingListNodeIndex>.Add(IWriteableBrowsingListNodeIndex item) { Add((IFocusBrowsingListNodeIndex)item); }
        void IList<IWriteableBrowsingListNodeIndex>.Insert(int index, IWriteableBrowsingListNodeIndex item) { Insert(index, (IFocusBrowsingListNodeIndex)item); }
        bool ICollection<IWriteableBrowsingListNodeIndex>.Remove(IWriteableBrowsingListNodeIndex item) { return Remove((IFocusBrowsingListNodeIndex)item); }
        void ICollection<IWriteableBrowsingListNodeIndex>.CopyTo(IWriteableBrowsingListNodeIndex[] array, int index) { CopyTo((IFocusBrowsingListNodeIndex[])array, index); }
        bool ICollection<IWriteableBrowsingListNodeIndex>.IsReadOnly { get { return ((ICollection<IFocusBrowsingListNodeIndex>)this).IsReadOnly; } }
        bool ICollection<IWriteableBrowsingListNodeIndex>.Contains(IWriteableBrowsingListNodeIndex value) { return Contains((IFocusBrowsingListNodeIndex)value); }
        int IList<IWriteableBrowsingListNodeIndex>.IndexOf(IWriteableBrowsingListNodeIndex value) { return IndexOf((IFocusBrowsingListNodeIndex)value); }
        IEnumerator<IWriteableBrowsingListNodeIndex> IWriteableBrowsingListNodeIndexList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IWriteableBrowsingListNodeIndex> IEnumerable<IWriteableBrowsingListNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Frame
        IFrameBrowsingListNodeIndex IFrameBrowsingListNodeIndexList.this[int index] { get { return this[index]; } set { this[index] = (IFocusBrowsingListNodeIndex)value; } }
        IFrameBrowsingListNodeIndex IList<IFrameBrowsingListNodeIndex>.this[int index] { get { return this[index]; } set { this[index] = (IFocusBrowsingListNodeIndex)value; } }
        IFrameBrowsingListNodeIndex IReadOnlyList<IFrameBrowsingListNodeIndex>.this[int index] { get { return this[index]; } }
        void ICollection<IFrameBrowsingListNodeIndex>.Add(IFrameBrowsingListNodeIndex item) { Add((IFocusBrowsingListNodeIndex)item); }
        void IList<IFrameBrowsingListNodeIndex>.Insert(int index, IFrameBrowsingListNodeIndex item) { Insert(index, (IFocusBrowsingListNodeIndex)item); }
        bool ICollection<IFrameBrowsingListNodeIndex>.Remove(IFrameBrowsingListNodeIndex item) { return Remove((IFocusBrowsingListNodeIndex)item); }
        void ICollection<IFrameBrowsingListNodeIndex>.CopyTo(IFrameBrowsingListNodeIndex[] array, int index) { CopyTo((IFocusBrowsingListNodeIndex[])array, index); }
        bool ICollection<IFrameBrowsingListNodeIndex>.IsReadOnly { get { return ((ICollection<IFocusBrowsingListNodeIndex>)this).IsReadOnly; } }
        bool ICollection<IFrameBrowsingListNodeIndex>.Contains(IFrameBrowsingListNodeIndex value) { return Contains((IFocusBrowsingListNodeIndex)value); }
        int IList<IFrameBrowsingListNodeIndex>.IndexOf(IFrameBrowsingListNodeIndex value) { return IndexOf((IFocusBrowsingListNodeIndex)value); }
        IEnumerator<IFrameBrowsingListNodeIndex> IFrameBrowsingListNodeIndexList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IFrameBrowsingListNodeIndex> IEnumerable<IFrameBrowsingListNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        #endregion
    }
}
