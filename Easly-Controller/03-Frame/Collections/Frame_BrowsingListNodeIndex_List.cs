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
        new int Count { get; }
        new IFrameBrowsingListNodeIndex this[int index] { get; set; }
        new IEnumerator<IFrameBrowsingListNodeIndex> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxBrowsingListNodeIndex
    /// </summary>
    internal class FrameBrowsingListNodeIndexList : Collection<IFrameBrowsingListNodeIndex>, IFrameBrowsingListNodeIndexList
    {
        #region ReadOnly
        public new IReadOnlyBrowsingListNodeIndex this[int index] { get { return base[index]; } set { base[index] = (IFrameBrowsingListNodeIndex)value; } }
        public void Add(IReadOnlyBrowsingListNodeIndex item) { base.Add((IFrameBrowsingListNodeIndex)item); }
        public void Insert(int index, IReadOnlyBrowsingListNodeIndex item) { base.Insert(index, (IFrameBrowsingListNodeIndex)item); }
        public bool Remove(IReadOnlyBrowsingListNodeIndex item) { return base.Remove((IFrameBrowsingListNodeIndex)item); }
        public void CopyTo(IReadOnlyBrowsingListNodeIndex[] array, int index) { base.CopyTo((IFrameBrowsingListNodeIndex[])array, index); }
        bool ICollection<IReadOnlyBrowsingListNodeIndex>.IsReadOnly { get { return ((ICollection<IFrameBrowsingListNodeIndex>)this).IsReadOnly; } }
        public bool Contains(IReadOnlyBrowsingListNodeIndex value) { return base.Contains((IFrameBrowsingListNodeIndex)value); }
        public int IndexOf(IReadOnlyBrowsingListNodeIndex value) { return base.IndexOf((IFrameBrowsingListNodeIndex)value); }
        public new IEnumerator<IReadOnlyBrowsingListNodeIndex> GetEnumerator() { return base.GetEnumerator(); }
        #endregion

        #region Writeable
        IWriteableBrowsingListNodeIndex IWriteableBrowsingListNodeIndexList.this[int index] { get { return base[index]; } set { base[index] = (IFrameBrowsingListNodeIndex)value; } }
        IWriteableBrowsingListNodeIndex IList<IWriteableBrowsingListNodeIndex>.this[int index] { get { return base[index]; } set { base[index] = (IFrameBrowsingListNodeIndex)value; } }
        IWriteableBrowsingListNodeIndex IReadOnlyList<IWriteableBrowsingListNodeIndex>.this[int index] { get { return base[index]; } }
        public void Add(IWriteableBrowsingListNodeIndex item) { base.Add((IFrameBrowsingListNodeIndex)item); }
        public void Insert(int index, IWriteableBrowsingListNodeIndex item) { base.Insert(index, (IFrameBrowsingListNodeIndex)item); }
        public bool Remove(IWriteableBrowsingListNodeIndex item) { return base.Remove((IFrameBrowsingListNodeIndex)item); }
        public void CopyTo(IWriteableBrowsingListNodeIndex[] array, int index) { base.CopyTo((IFrameBrowsingListNodeIndex[])array, index); }
        bool ICollection<IWriteableBrowsingListNodeIndex>.IsReadOnly { get { return ((ICollection<IFrameBrowsingListNodeIndex>)this).IsReadOnly; } }
        public bool Contains(IWriteableBrowsingListNodeIndex value) { return base.Contains((IFrameBrowsingListNodeIndex)value); }
        public int IndexOf(IWriteableBrowsingListNodeIndex value) { return base.IndexOf((IFrameBrowsingListNodeIndex)value); }
        IEnumerator<IWriteableBrowsingListNodeIndex> IWriteableBrowsingListNodeIndexList.GetEnumerator() { return base.GetEnumerator(); }
        IEnumerator<IWriteableBrowsingListNodeIndex> IEnumerable<IWriteableBrowsingListNodeIndex>.GetEnumerator() { return base.GetEnumerator(); }
        #endregion
    }
}
