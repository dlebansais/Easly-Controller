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
        public void Add(IReadOnlyBrowsingListNodeIndex item) { base.Add((IFocusBrowsingListNodeIndex)item); }
        public void Insert(int index, IReadOnlyBrowsingListNodeIndex item) { base.Insert(index, (IFocusBrowsingListNodeIndex)item); }
        public bool Remove(IReadOnlyBrowsingListNodeIndex item) { return base.Remove((IFocusBrowsingListNodeIndex)item); }
        public void CopyTo(IReadOnlyBrowsingListNodeIndex[] array, int index) { base.CopyTo((IFocusBrowsingListNodeIndex[])array, index); }
        bool ICollection<IReadOnlyBrowsingListNodeIndex>.IsReadOnly { get { return ((ICollection<IFocusBrowsingListNodeIndex>)this).IsReadOnly; } }
        public bool Contains(IReadOnlyBrowsingListNodeIndex value) { return base.Contains((IFocusBrowsingListNodeIndex)value); }
        public int IndexOf(IReadOnlyBrowsingListNodeIndex value) { return base.IndexOf((IFocusBrowsingListNodeIndex)value); }
        IEnumerator<IReadOnlyBrowsingListNodeIndex> IEnumerable<IReadOnlyBrowsingListNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Writeable
        IWriteableBrowsingListNodeIndex IWriteableBrowsingListNodeIndexList.this[int index] { get { return this[index]; } set { this[index] = (IFocusBrowsingListNodeIndex)value; } }
        IWriteableBrowsingListNodeIndex IList<IWriteableBrowsingListNodeIndex>.this[int index] { get { return this[index]; } set { this[index] = (IFocusBrowsingListNodeIndex)value; } }
        IWriteableBrowsingListNodeIndex IReadOnlyList<IWriteableBrowsingListNodeIndex>.this[int index] { get { return this[index]; } }
        public void Add(IWriteableBrowsingListNodeIndex item) { base.Add((IFocusBrowsingListNodeIndex)item); }
        public void Insert(int index, IWriteableBrowsingListNodeIndex item) { base.Insert(index, (IFocusBrowsingListNodeIndex)item); }
        public bool Remove(IWriteableBrowsingListNodeIndex item) { return base.Remove((IFocusBrowsingListNodeIndex)item); }
        public void CopyTo(IWriteableBrowsingListNodeIndex[] array, int index) { base.CopyTo((IFocusBrowsingListNodeIndex[])array, index); }
        bool ICollection<IWriteableBrowsingListNodeIndex>.IsReadOnly { get { return ((ICollection<IFocusBrowsingListNodeIndex>)this).IsReadOnly; } }
        public bool Contains(IWriteableBrowsingListNodeIndex value) { return base.Contains((IFocusBrowsingListNodeIndex)value); }
        public int IndexOf(IWriteableBrowsingListNodeIndex value) { return base.IndexOf((IFocusBrowsingListNodeIndex)value); }
        IEnumerator<IWriteableBrowsingListNodeIndex> IWriteableBrowsingListNodeIndexList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IWriteableBrowsingListNodeIndex> IEnumerable<IWriteableBrowsingListNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Frame
        IFrameBrowsingListNodeIndex IFrameBrowsingListNodeIndexList.this[int index] { get { return this[index]; } set { this[index] = (IFocusBrowsingListNodeIndex)value; } }
        IFrameBrowsingListNodeIndex IList<IFrameBrowsingListNodeIndex>.this[int index] { get { return this[index]; } set { this[index] = (IFocusBrowsingListNodeIndex)value; } }
        IFrameBrowsingListNodeIndex IReadOnlyList<IFrameBrowsingListNodeIndex>.this[int index] { get { return this[index]; } }
        public void Add(IFrameBrowsingListNodeIndex item) { base.Add((IFocusBrowsingListNodeIndex)item); }
        public void Insert(int index, IFrameBrowsingListNodeIndex item) { base.Insert(index, (IFocusBrowsingListNodeIndex)item); }
        public bool Remove(IFrameBrowsingListNodeIndex item) { return base.Remove((IFocusBrowsingListNodeIndex)item); }
        public void CopyTo(IFrameBrowsingListNodeIndex[] array, int index) { base.CopyTo((IFocusBrowsingListNodeIndex[])array, index); }
        bool ICollection<IFrameBrowsingListNodeIndex>.IsReadOnly { get { return ((ICollection<IFocusBrowsingListNodeIndex>)this).IsReadOnly; } }
        public bool Contains(IFrameBrowsingListNodeIndex value) { return base.Contains((IFocusBrowsingListNodeIndex)value); }
        public int IndexOf(IFrameBrowsingListNodeIndex value) { return base.IndexOf((IFocusBrowsingListNodeIndex)value); }
        IEnumerator<IFrameBrowsingListNodeIndex> IFrameBrowsingListNodeIndexList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IFrameBrowsingListNodeIndex> IEnumerable<IFrameBrowsingListNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        #endregion
    }
}
