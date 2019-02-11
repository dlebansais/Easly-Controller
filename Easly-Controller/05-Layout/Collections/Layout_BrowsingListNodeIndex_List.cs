#pragma warning disable 1591

namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Focus;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// List of IxxxBrowsingListNodeIndex
    /// </summary>
    public interface ILayoutBrowsingListNodeIndexList : IFocusBrowsingListNodeIndexList, IList<ILayoutBrowsingListNodeIndex>, IReadOnlyList<ILayoutBrowsingListNodeIndex>
    {
        new ILayoutBrowsingListNodeIndex this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<ILayoutBrowsingListNodeIndex> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxBrowsingListNodeIndex
    /// </summary>
    internal class LayoutBrowsingListNodeIndexList : Collection<ILayoutBrowsingListNodeIndex>, ILayoutBrowsingListNodeIndexList
    {
        #region ReadOnly
        IReadOnlyBrowsingListNodeIndex IReadOnlyBrowsingListNodeIndexList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutBrowsingListNodeIndex)value; } }
        IReadOnlyBrowsingListNodeIndex IList<IReadOnlyBrowsingListNodeIndex>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutBrowsingListNodeIndex)value; } }
        int IList<IReadOnlyBrowsingListNodeIndex>.IndexOf(IReadOnlyBrowsingListNodeIndex value) { return IndexOf((ILayoutBrowsingListNodeIndex)value); }
        void IList<IReadOnlyBrowsingListNodeIndex>.Insert(int index, IReadOnlyBrowsingListNodeIndex item) { Insert(index, (ILayoutBrowsingListNodeIndex)item); }
        void ICollection<IReadOnlyBrowsingListNodeIndex>.Add(IReadOnlyBrowsingListNodeIndex item) { Add((ILayoutBrowsingListNodeIndex)item); }
        bool ICollection<IReadOnlyBrowsingListNodeIndex>.Contains(IReadOnlyBrowsingListNodeIndex value) { return Contains((ILayoutBrowsingListNodeIndex)value); }
        void ICollection<IReadOnlyBrowsingListNodeIndex>.CopyTo(IReadOnlyBrowsingListNodeIndex[] array, int index) { CopyTo((ILayoutBrowsingListNodeIndex[])array, index); }
        bool ICollection<IReadOnlyBrowsingListNodeIndex>.IsReadOnly { get { return ((ICollection<ILayoutBrowsingListNodeIndex>)this).IsReadOnly; } }
        bool ICollection<IReadOnlyBrowsingListNodeIndex>.Remove(IReadOnlyBrowsingListNodeIndex item) { return Remove((ILayoutBrowsingListNodeIndex)item); }
        IEnumerator<IReadOnlyBrowsingListNodeIndex> IEnumerable<IReadOnlyBrowsingListNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        IReadOnlyBrowsingListNodeIndex IReadOnlyList<IReadOnlyBrowsingListNodeIndex>.this[int index] { get { return this[index]; } }
        #endregion

        #region Writeable
        IWriteableBrowsingListNodeIndex IWriteableBrowsingListNodeIndexList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutBrowsingListNodeIndex)value; } }
        IEnumerator<IWriteableBrowsingListNodeIndex> IWriteableBrowsingListNodeIndexList.GetEnumerator() { return GetEnumerator(); }
        IWriteableBrowsingListNodeIndex IList<IWriteableBrowsingListNodeIndex>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutBrowsingListNodeIndex)value; } }
        int IList<IWriteableBrowsingListNodeIndex>.IndexOf(IWriteableBrowsingListNodeIndex value) { return IndexOf((ILayoutBrowsingListNodeIndex)value); }
        void IList<IWriteableBrowsingListNodeIndex>.Insert(int index, IWriteableBrowsingListNodeIndex item) { Insert(index, (ILayoutBrowsingListNodeIndex)item); }
        void ICollection<IWriteableBrowsingListNodeIndex>.Add(IWriteableBrowsingListNodeIndex item) { Add((ILayoutBrowsingListNodeIndex)item); }
        bool ICollection<IWriteableBrowsingListNodeIndex>.Contains(IWriteableBrowsingListNodeIndex value) { return Contains((ILayoutBrowsingListNodeIndex)value); }
        void ICollection<IWriteableBrowsingListNodeIndex>.CopyTo(IWriteableBrowsingListNodeIndex[] array, int index) { CopyTo((ILayoutBrowsingListNodeIndex[])array, index); }
        bool ICollection<IWriteableBrowsingListNodeIndex>.IsReadOnly { get { return ((ICollection<ILayoutBrowsingListNodeIndex>)this).IsReadOnly; } }
        bool ICollection<IWriteableBrowsingListNodeIndex>.Remove(IWriteableBrowsingListNodeIndex item) { return Remove((ILayoutBrowsingListNodeIndex)item); }
        IEnumerator<IWriteableBrowsingListNodeIndex> IEnumerable<IWriteableBrowsingListNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        IWriteableBrowsingListNodeIndex IReadOnlyList<IWriteableBrowsingListNodeIndex>.this[int index] { get { return this[index]; } }
        #endregion

        #region Frame
        IFrameBrowsingListNodeIndex IFrameBrowsingListNodeIndexList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutBrowsingListNodeIndex)value; } }
        IEnumerator<IFrameBrowsingListNodeIndex> IFrameBrowsingListNodeIndexList.GetEnumerator() { return GetEnumerator(); }
        IFrameBrowsingListNodeIndex IList<IFrameBrowsingListNodeIndex>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutBrowsingListNodeIndex)value; } }
        int IList<IFrameBrowsingListNodeIndex>.IndexOf(IFrameBrowsingListNodeIndex value) { return IndexOf((ILayoutBrowsingListNodeIndex)value); }
        void IList<IFrameBrowsingListNodeIndex>.Insert(int index, IFrameBrowsingListNodeIndex item) { Insert(index, (ILayoutBrowsingListNodeIndex)item); }
        void ICollection<IFrameBrowsingListNodeIndex>.Add(IFrameBrowsingListNodeIndex item) { Add((ILayoutBrowsingListNodeIndex)item); }
        bool ICollection<IFrameBrowsingListNodeIndex>.Contains(IFrameBrowsingListNodeIndex value) { return Contains((ILayoutBrowsingListNodeIndex)value); }
        void ICollection<IFrameBrowsingListNodeIndex>.CopyTo(IFrameBrowsingListNodeIndex[] array, int index) { CopyTo((ILayoutBrowsingListNodeIndex[])array, index); }
        bool ICollection<IFrameBrowsingListNodeIndex>.IsReadOnly { get { return ((ICollection<ILayoutBrowsingListNodeIndex>)this).IsReadOnly; } }
        bool ICollection<IFrameBrowsingListNodeIndex>.Remove(IFrameBrowsingListNodeIndex item) { return Remove((ILayoutBrowsingListNodeIndex)item); }
        IEnumerator<IFrameBrowsingListNodeIndex> IEnumerable<IFrameBrowsingListNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        IFrameBrowsingListNodeIndex IReadOnlyList<IFrameBrowsingListNodeIndex>.this[int index] { get { return this[index]; } }
        #endregion

        #region Focus
        IFocusBrowsingListNodeIndex IFocusBrowsingListNodeIndexList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutBrowsingListNodeIndex)value; } }
        IEnumerator<IFocusBrowsingListNodeIndex> IFocusBrowsingListNodeIndexList.GetEnumerator() { return GetEnumerator(); }
        IFocusBrowsingListNodeIndex IList<IFocusBrowsingListNodeIndex>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutBrowsingListNodeIndex)value; } }
        int IList<IFocusBrowsingListNodeIndex>.IndexOf(IFocusBrowsingListNodeIndex value) { return IndexOf((ILayoutBrowsingListNodeIndex)value); }
        void IList<IFocusBrowsingListNodeIndex>.Insert(int index, IFocusBrowsingListNodeIndex item) { Insert(index, (ILayoutBrowsingListNodeIndex)item); }
        void ICollection<IFocusBrowsingListNodeIndex>.Add(IFocusBrowsingListNodeIndex item) { Add((ILayoutBrowsingListNodeIndex)item); }
        bool ICollection<IFocusBrowsingListNodeIndex>.Contains(IFocusBrowsingListNodeIndex value) { return Contains((ILayoutBrowsingListNodeIndex)value); }
        void ICollection<IFocusBrowsingListNodeIndex>.CopyTo(IFocusBrowsingListNodeIndex[] array, int index) { CopyTo((ILayoutBrowsingListNodeIndex[])array, index); }
        bool ICollection<IFocusBrowsingListNodeIndex>.IsReadOnly { get { return ((ICollection<ILayoutBrowsingListNodeIndex>)this).IsReadOnly; } }
        bool ICollection<IFocusBrowsingListNodeIndex>.Remove(IFocusBrowsingListNodeIndex item) { return Remove((ILayoutBrowsingListNodeIndex)item); }
        IEnumerator<IFocusBrowsingListNodeIndex> IEnumerable<IFocusBrowsingListNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        IFocusBrowsingListNodeIndex IReadOnlyList<IFocusBrowsingListNodeIndex>.this[int index] { get { return this[index]; } }
        #endregion
    }
}
