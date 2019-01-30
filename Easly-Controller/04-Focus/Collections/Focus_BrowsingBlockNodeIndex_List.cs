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
    public class FocusBrowsingBlockNodeIndexList : Collection<IFocusBrowsingBlockNodeIndex>, IFocusBrowsingBlockNodeIndexList
    {
        #region ReadOnly
        public new IReadOnlyBrowsingBlockNodeIndex this[int index] { get { return base[index]; } set { base[index] = (IFocusBrowsingBlockNodeIndex)value; } }
        public void Add(IReadOnlyBrowsingBlockNodeIndex item) { base.Add((IFocusBrowsingBlockNodeIndex)item); }
        public void Insert(int index, IReadOnlyBrowsingBlockNodeIndex item) { base.Insert(index, (IFocusBrowsingBlockNodeIndex)item); }
        public bool Remove(IReadOnlyBrowsingBlockNodeIndex item) { return base.Remove((IFocusBrowsingBlockNodeIndex)item); }
        public void CopyTo(IReadOnlyBrowsingBlockNodeIndex[] array, int index) { base.CopyTo((IFocusBrowsingBlockNodeIndex[])array, index); }
        bool ICollection<IReadOnlyBrowsingBlockNodeIndex>.IsReadOnly { get { return ((ICollection<IFocusBrowsingBlockNodeIndex>)this).IsReadOnly; } }
        public bool Contains(IReadOnlyBrowsingBlockNodeIndex value) { return base.Contains((IFocusBrowsingBlockNodeIndex)value); }
        public int IndexOf(IReadOnlyBrowsingBlockNodeIndex value) { return base.IndexOf((IFocusBrowsingBlockNodeIndex)value); }
        public new IEnumerator<IReadOnlyBrowsingBlockNodeIndex> GetEnumerator() { return base.GetEnumerator(); }
        #endregion

        #region Writeable
        IWriteableBrowsingBlockNodeIndex IWriteableBrowsingBlockNodeIndexList.this[int index] { get { return base[index]; } set { base[index] = (IFocusBrowsingBlockNodeIndex)value; } }
        IWriteableBrowsingBlockNodeIndex IList<IWriteableBrowsingBlockNodeIndex>.this[int index] { get { return base[index]; } set { base[index] = (IFocusBrowsingBlockNodeIndex)value; } }
        IWriteableBrowsingBlockNodeIndex IReadOnlyList<IWriteableBrowsingBlockNodeIndex>.this[int index] { get { return base[index]; } }
        public void Add(IWriteableBrowsingBlockNodeIndex item) { base.Add((IFocusBrowsingBlockNodeIndex)item); }
        public void Insert(int index, IWriteableBrowsingBlockNodeIndex item) { base.Insert(index, (IFocusBrowsingBlockNodeIndex)item); }
        public bool Remove(IWriteableBrowsingBlockNodeIndex item) { return base.Remove((IFocusBrowsingBlockNodeIndex)item); }
        public void CopyTo(IWriteableBrowsingBlockNodeIndex[] array, int index) { base.CopyTo((IFocusBrowsingBlockNodeIndex[])array, index); }
        bool ICollection<IWriteableBrowsingBlockNodeIndex>.IsReadOnly { get { return ((ICollection<IFocusBrowsingBlockNodeIndex>)this).IsReadOnly; } }
        public bool Contains(IWriteableBrowsingBlockNodeIndex value) { return base.Contains((IFocusBrowsingBlockNodeIndex)value); }
        public int IndexOf(IWriteableBrowsingBlockNodeIndex value) { return base.IndexOf((IFocusBrowsingBlockNodeIndex)value); }
        IEnumerator<IWriteableBrowsingBlockNodeIndex> IWriteableBrowsingBlockNodeIndexList.GetEnumerator() { return base.GetEnumerator(); }
        IEnumerator<IWriteableBrowsingBlockNodeIndex> IEnumerable<IWriteableBrowsingBlockNodeIndex>.GetEnumerator() { return base.GetEnumerator(); }
        #endregion

        #region Frame
        IFrameBrowsingBlockNodeIndex IFrameBrowsingBlockNodeIndexList.this[int index] { get { return base[index]; } set { base[index] = (IFocusBrowsingBlockNodeIndex)value; } }
        IFrameBrowsingBlockNodeIndex IList<IFrameBrowsingBlockNodeIndex>.this[int index] { get { return base[index]; } set { base[index] = (IFocusBrowsingBlockNodeIndex)value; } }
        IFrameBrowsingBlockNodeIndex IReadOnlyList<IFrameBrowsingBlockNodeIndex>.this[int index] { get { return base[index]; } }
        public void Add(IFrameBrowsingBlockNodeIndex item) { base.Add((IFocusBrowsingBlockNodeIndex)item); }
        public void Insert(int index, IFrameBrowsingBlockNodeIndex item) { base.Insert(index, (IFocusBrowsingBlockNodeIndex)item); }
        public bool Remove(IFrameBrowsingBlockNodeIndex item) { return base.Remove((IFocusBrowsingBlockNodeIndex)item); }
        public void CopyTo(IFrameBrowsingBlockNodeIndex[] array, int index) { base.CopyTo((IFocusBrowsingBlockNodeIndex[])array, index); }
        bool ICollection<IFrameBrowsingBlockNodeIndex>.IsReadOnly { get { return ((ICollection<IFocusBrowsingBlockNodeIndex>)this).IsReadOnly; } }
        public bool Contains(IFrameBrowsingBlockNodeIndex value) { return base.Contains((IFocusBrowsingBlockNodeIndex)value); }
        public int IndexOf(IFrameBrowsingBlockNodeIndex value) { return base.IndexOf((IFocusBrowsingBlockNodeIndex)value); }
        IEnumerator<IFrameBrowsingBlockNodeIndex> IFrameBrowsingBlockNodeIndexList.GetEnumerator() { return base.GetEnumerator(); }
        IEnumerator<IFrameBrowsingBlockNodeIndex> IEnumerable<IFrameBrowsingBlockNodeIndex>.GetEnumerator() { return base.GetEnumerator(); }
        #endregion
    }
}