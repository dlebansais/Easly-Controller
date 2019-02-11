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
    /// List of IxxxBrowsingBlockNodeIndex
    /// </summary>
    public interface ILayoutBrowsingBlockNodeIndexList : IFocusBrowsingBlockNodeIndexList, IList<ILayoutBrowsingBlockNodeIndex>, IReadOnlyList<ILayoutBrowsingBlockNodeIndex>
    {
        new ILayoutBrowsingBlockNodeIndex this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<ILayoutBrowsingBlockNodeIndex> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxBrowsingBlockNodeIndex
    /// </summary>
    internal class LayoutBrowsingBlockNodeIndexList : Collection<ILayoutBrowsingBlockNodeIndex>, ILayoutBrowsingBlockNodeIndexList
    {
        #region ReadOnly
        IReadOnlyBrowsingBlockNodeIndex IReadOnlyBrowsingBlockNodeIndexList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutBrowsingBlockNodeIndex)value; } }
        IReadOnlyBrowsingBlockNodeIndex IList<IReadOnlyBrowsingBlockNodeIndex>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutBrowsingBlockNodeIndex)value; } }
        int IList<IReadOnlyBrowsingBlockNodeIndex>.IndexOf(IReadOnlyBrowsingBlockNodeIndex value) { return IndexOf((ILayoutBrowsingBlockNodeIndex)value); }
        void IList<IReadOnlyBrowsingBlockNodeIndex>.Insert(int index, IReadOnlyBrowsingBlockNodeIndex item) { Insert(index, (ILayoutBrowsingBlockNodeIndex)item); }
        void ICollection<IReadOnlyBrowsingBlockNodeIndex>.Add(IReadOnlyBrowsingBlockNodeIndex item) { Add((ILayoutBrowsingBlockNodeIndex)item); }
        bool ICollection<IReadOnlyBrowsingBlockNodeIndex>.Contains(IReadOnlyBrowsingBlockNodeIndex value) { return Contains((ILayoutBrowsingBlockNodeIndex)value); }
        void ICollection<IReadOnlyBrowsingBlockNodeIndex>.CopyTo(IReadOnlyBrowsingBlockNodeIndex[] array, int index) { CopyTo((ILayoutBrowsingBlockNodeIndex[])array, index); }
        bool ICollection<IReadOnlyBrowsingBlockNodeIndex>.IsReadOnly { get { return ((ICollection<ILayoutBrowsingBlockNodeIndex>)this).IsReadOnly; } }
        bool ICollection<IReadOnlyBrowsingBlockNodeIndex>.Remove(IReadOnlyBrowsingBlockNodeIndex item) { return Remove((ILayoutBrowsingBlockNodeIndex)item); }
        IEnumerator<IReadOnlyBrowsingBlockNodeIndex> IEnumerable<IReadOnlyBrowsingBlockNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        IReadOnlyBrowsingBlockNodeIndex IReadOnlyList<IReadOnlyBrowsingBlockNodeIndex>.this[int index] { get { return this[index]; } }
        #endregion

        #region Writeable
        IWriteableBrowsingBlockNodeIndex IWriteableBrowsingBlockNodeIndexList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutBrowsingBlockNodeIndex)value; } }
        IEnumerator<IWriteableBrowsingBlockNodeIndex> IWriteableBrowsingBlockNodeIndexList.GetEnumerator() { return GetEnumerator(); }
        IWriteableBrowsingBlockNodeIndex IList<IWriteableBrowsingBlockNodeIndex>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutBrowsingBlockNodeIndex)value; } }
        int IList<IWriteableBrowsingBlockNodeIndex>.IndexOf(IWriteableBrowsingBlockNodeIndex value) { return IndexOf((ILayoutBrowsingBlockNodeIndex)value); }
        void IList<IWriteableBrowsingBlockNodeIndex>.Insert(int index, IWriteableBrowsingBlockNodeIndex item) { Insert(index, (ILayoutBrowsingBlockNodeIndex)item); }
        void ICollection<IWriteableBrowsingBlockNodeIndex>.Add(IWriteableBrowsingBlockNodeIndex item) { Add((ILayoutBrowsingBlockNodeIndex)item); }
        bool ICollection<IWriteableBrowsingBlockNodeIndex>.Contains(IWriteableBrowsingBlockNodeIndex value) { return Contains((ILayoutBrowsingBlockNodeIndex)value); }
        void ICollection<IWriteableBrowsingBlockNodeIndex>.CopyTo(IWriteableBrowsingBlockNodeIndex[] array, int index) { CopyTo((ILayoutBrowsingBlockNodeIndex[])array, index); }
        bool ICollection<IWriteableBrowsingBlockNodeIndex>.IsReadOnly { get { return ((ICollection<ILayoutBrowsingBlockNodeIndex>)this).IsReadOnly; } }
        bool ICollection<IWriteableBrowsingBlockNodeIndex>.Remove(IWriteableBrowsingBlockNodeIndex item) { return Remove((ILayoutBrowsingBlockNodeIndex)item); }
        IEnumerator<IWriteableBrowsingBlockNodeIndex> IEnumerable<IWriteableBrowsingBlockNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        IWriteableBrowsingBlockNodeIndex IReadOnlyList<IWriteableBrowsingBlockNodeIndex>.this[int index] { get { return this[index]; } }
        #endregion

        #region Frame
        IFrameBrowsingBlockNodeIndex IFrameBrowsingBlockNodeIndexList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutBrowsingBlockNodeIndex)value; } }
        IEnumerator<IFrameBrowsingBlockNodeIndex> IFrameBrowsingBlockNodeIndexList.GetEnumerator() { return GetEnumerator(); }
        IFrameBrowsingBlockNodeIndex IList<IFrameBrowsingBlockNodeIndex>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutBrowsingBlockNodeIndex)value; } }
        int IList<IFrameBrowsingBlockNodeIndex>.IndexOf(IFrameBrowsingBlockNodeIndex value) { return IndexOf((ILayoutBrowsingBlockNodeIndex)value); }
        void IList<IFrameBrowsingBlockNodeIndex>.Insert(int index, IFrameBrowsingBlockNodeIndex item) { Insert(index, (ILayoutBrowsingBlockNodeIndex)item); }
        void ICollection<IFrameBrowsingBlockNodeIndex>.Add(IFrameBrowsingBlockNodeIndex item) { Add((ILayoutBrowsingBlockNodeIndex)item); }
        bool ICollection<IFrameBrowsingBlockNodeIndex>.Contains(IFrameBrowsingBlockNodeIndex value) { return Contains((ILayoutBrowsingBlockNodeIndex)value); }
        void ICollection<IFrameBrowsingBlockNodeIndex>.CopyTo(IFrameBrowsingBlockNodeIndex[] array, int index) { CopyTo((ILayoutBrowsingBlockNodeIndex[])array, index); }
        bool ICollection<IFrameBrowsingBlockNodeIndex>.IsReadOnly { get { return ((ICollection<ILayoutBrowsingBlockNodeIndex>)this).IsReadOnly; } }
        bool ICollection<IFrameBrowsingBlockNodeIndex>.Remove(IFrameBrowsingBlockNodeIndex item) { return Remove((ILayoutBrowsingBlockNodeIndex)item); }
        IEnumerator<IFrameBrowsingBlockNodeIndex> IEnumerable<IFrameBrowsingBlockNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        IFrameBrowsingBlockNodeIndex IReadOnlyList<IFrameBrowsingBlockNodeIndex>.this[int index] { get { return this[index]; } }
        #endregion

        #region Focus
        IFocusBrowsingBlockNodeIndex IFocusBrowsingBlockNodeIndexList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutBrowsingBlockNodeIndex)value; } }
        IEnumerator<IFocusBrowsingBlockNodeIndex> IFocusBrowsingBlockNodeIndexList.GetEnumerator() { return GetEnumerator(); }
        IFocusBrowsingBlockNodeIndex IList<IFocusBrowsingBlockNodeIndex>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutBrowsingBlockNodeIndex)value; } }
        int IList<IFocusBrowsingBlockNodeIndex>.IndexOf(IFocusBrowsingBlockNodeIndex value) { return IndexOf((ILayoutBrowsingBlockNodeIndex)value); }
        void IList<IFocusBrowsingBlockNodeIndex>.Insert(int index, IFocusBrowsingBlockNodeIndex item) { Insert(index, (ILayoutBrowsingBlockNodeIndex)item); }
        void ICollection<IFocusBrowsingBlockNodeIndex>.Add(IFocusBrowsingBlockNodeIndex item) { Add((ILayoutBrowsingBlockNodeIndex)item); }
        bool ICollection<IFocusBrowsingBlockNodeIndex>.Contains(IFocusBrowsingBlockNodeIndex value) { return Contains((ILayoutBrowsingBlockNodeIndex)value); }
        void ICollection<IFocusBrowsingBlockNodeIndex>.CopyTo(IFocusBrowsingBlockNodeIndex[] array, int index) { CopyTo((ILayoutBrowsingBlockNodeIndex[])array, index); }
        bool ICollection<IFocusBrowsingBlockNodeIndex>.IsReadOnly { get { return ((ICollection<ILayoutBrowsingBlockNodeIndex>)this).IsReadOnly; } }
        bool ICollection<IFocusBrowsingBlockNodeIndex>.Remove(IFocusBrowsingBlockNodeIndex item) { return Remove((ILayoutBrowsingBlockNodeIndex)item); }
        IEnumerator<IFocusBrowsingBlockNodeIndex> IEnumerable<IFocusBrowsingBlockNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        IFocusBrowsingBlockNodeIndex IReadOnlyList<IFocusBrowsingBlockNodeIndex>.this[int index] { get { return this[index]; } }
        #endregion
    }
}
