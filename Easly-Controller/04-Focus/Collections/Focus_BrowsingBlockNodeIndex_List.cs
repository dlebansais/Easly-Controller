#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// List of IxxxBrowsingBlockNodeIndex
    /// </summary>
    public interface IFocusBrowsingBlockNodeIndexList : IFrameBrowsingBlockNodeIndexList, IList<IFocusBrowsingBlockNodeIndex>, IReadOnlyList<IFocusBrowsingBlockNodeIndex>
    {
        new int Count { get; }
        new IFocusBrowsingBlockNodeIndex this[int index] { get; set; }
        new IEnumerator<IFocusBrowsingBlockNodeIndex> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxBrowsingBlockNodeIndex
    /// </summary>
    internal class FocusBrowsingBlockNodeIndexList : Collection<IFocusBrowsingBlockNodeIndex>, IFocusBrowsingBlockNodeIndexList
    {
        #region ReadOnly
        IReadOnlyBrowsingBlockNodeIndex IReadOnlyBrowsingBlockNodeIndexList.this[int index] { get { return this[index]; } set { this[index] = (IFocusBrowsingBlockNodeIndex)value; } }
        IReadOnlyBrowsingBlockNodeIndex IList<IReadOnlyBrowsingBlockNodeIndex>.this[int index] { get { return this[index]; } set { this[index] = (IFocusBrowsingBlockNodeIndex)value; } }
        IReadOnlyBrowsingBlockNodeIndex IReadOnlyList<IReadOnlyBrowsingBlockNodeIndex>.this[int index] { get { return this[index]; } }
        void ICollection<IReadOnlyBrowsingBlockNodeIndex>.Add(IReadOnlyBrowsingBlockNodeIndex item) { Add((IFocusBrowsingBlockNodeIndex)item); }
        void IList<IReadOnlyBrowsingBlockNodeIndex>.Insert(int index, IReadOnlyBrowsingBlockNodeIndex item) { Insert(index, (IFocusBrowsingBlockNodeIndex)item); }
        bool ICollection<IReadOnlyBrowsingBlockNodeIndex>.Remove(IReadOnlyBrowsingBlockNodeIndex item) { return Remove((IFocusBrowsingBlockNodeIndex)item); }
        void ICollection<IReadOnlyBrowsingBlockNodeIndex>.CopyTo(IReadOnlyBrowsingBlockNodeIndex[] array, int index) { CopyTo((IFocusBrowsingBlockNodeIndex[])array, index); }
        bool ICollection<IReadOnlyBrowsingBlockNodeIndex>.IsReadOnly { get { return ((ICollection<IFocusBrowsingBlockNodeIndex>)this).IsReadOnly; } }
        bool ICollection<IReadOnlyBrowsingBlockNodeIndex>.Contains(IReadOnlyBrowsingBlockNodeIndex value) { return Contains((IFocusBrowsingBlockNodeIndex)value); }
        int IList<IReadOnlyBrowsingBlockNodeIndex>.IndexOf(IReadOnlyBrowsingBlockNodeIndex value) { return IndexOf((IFocusBrowsingBlockNodeIndex)value); }
        IEnumerator<IReadOnlyBrowsingBlockNodeIndex> IEnumerable<IReadOnlyBrowsingBlockNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Writeable
        IWriteableBrowsingBlockNodeIndex IWriteableBrowsingBlockNodeIndexList.this[int index] { get { return this[index]; } set { this[index] = (IFocusBrowsingBlockNodeIndex)value; } }
        IWriteableBrowsingBlockNodeIndex IList<IWriteableBrowsingBlockNodeIndex>.this[int index] { get { return this[index]; } set { this[index] = (IFocusBrowsingBlockNodeIndex)value; } }
        IWriteableBrowsingBlockNodeIndex IReadOnlyList<IWriteableBrowsingBlockNodeIndex>.this[int index] { get { return this[index]; } }
        void ICollection<IWriteableBrowsingBlockNodeIndex>.Add(IWriteableBrowsingBlockNodeIndex item) { Add((IFocusBrowsingBlockNodeIndex)item); }
        void IList<IWriteableBrowsingBlockNodeIndex>.Insert(int index, IWriteableBrowsingBlockNodeIndex item) { Insert(index, (IFocusBrowsingBlockNodeIndex)item); }
        bool ICollection<IWriteableBrowsingBlockNodeIndex>.Remove(IWriteableBrowsingBlockNodeIndex item) { return Remove((IFocusBrowsingBlockNodeIndex)item); }
        void ICollection<IWriteableBrowsingBlockNodeIndex>.CopyTo(IWriteableBrowsingBlockNodeIndex[] array, int index) { CopyTo((IFocusBrowsingBlockNodeIndex[])array, index); }
        bool ICollection<IWriteableBrowsingBlockNodeIndex>.IsReadOnly { get { return ((ICollection<IFocusBrowsingBlockNodeIndex>)this).IsReadOnly; } }
        bool ICollection<IWriteableBrowsingBlockNodeIndex>.Contains(IWriteableBrowsingBlockNodeIndex value) { return Contains((IFocusBrowsingBlockNodeIndex)value); }
        int IList<IWriteableBrowsingBlockNodeIndex>.IndexOf(IWriteableBrowsingBlockNodeIndex value) { return IndexOf((IFocusBrowsingBlockNodeIndex)value); }
        IEnumerator<IWriteableBrowsingBlockNodeIndex> IWriteableBrowsingBlockNodeIndexList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IWriteableBrowsingBlockNodeIndex> IEnumerable<IWriteableBrowsingBlockNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Frame
        IFrameBrowsingBlockNodeIndex IFrameBrowsingBlockNodeIndexList.this[int index] { get { return this[index]; } set { this[index] = (IFocusBrowsingBlockNodeIndex)value; } }
        IFrameBrowsingBlockNodeIndex IList<IFrameBrowsingBlockNodeIndex>.this[int index] { get { return this[index]; } set { this[index] = (IFocusBrowsingBlockNodeIndex)value; } }
        IFrameBrowsingBlockNodeIndex IReadOnlyList<IFrameBrowsingBlockNodeIndex>.this[int index] { get { return this[index]; } }
        void ICollection<IFrameBrowsingBlockNodeIndex>.Add(IFrameBrowsingBlockNodeIndex item) { Add((IFocusBrowsingBlockNodeIndex)item); }
        void IList<IFrameBrowsingBlockNodeIndex>.Insert(int index, IFrameBrowsingBlockNodeIndex item) { Insert(index, (IFocusBrowsingBlockNodeIndex)item); }
        bool ICollection<IFrameBrowsingBlockNodeIndex>.Remove(IFrameBrowsingBlockNodeIndex item) { return Remove((IFocusBrowsingBlockNodeIndex)item); }
        void ICollection<IFrameBrowsingBlockNodeIndex>.CopyTo(IFrameBrowsingBlockNodeIndex[] array, int index) { CopyTo((IFocusBrowsingBlockNodeIndex[])array, index); }
        bool ICollection<IFrameBrowsingBlockNodeIndex>.IsReadOnly { get { return ((ICollection<IFocusBrowsingBlockNodeIndex>)this).IsReadOnly; } }
        bool ICollection<IFrameBrowsingBlockNodeIndex>.Contains(IFrameBrowsingBlockNodeIndex value) { return Contains((IFocusBrowsingBlockNodeIndex)value); }
        int IList<IFrameBrowsingBlockNodeIndex>.IndexOf(IFrameBrowsingBlockNodeIndex value) { return IndexOf((IFocusBrowsingBlockNodeIndex)value); }
        IEnumerator<IFrameBrowsingBlockNodeIndex> IFrameBrowsingBlockNodeIndexList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IFrameBrowsingBlockNodeIndex> IEnumerable<IFrameBrowsingBlockNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        #endregion
    }
}
