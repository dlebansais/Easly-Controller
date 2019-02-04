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
        public new IReadOnlyBrowsingBlockNodeIndex this[int index] { get { return base[index]; } set { base[index] = (IFrameBrowsingBlockNodeIndex)value; } }
        public void Add(IReadOnlyBrowsingBlockNodeIndex item) { base.Add((IFrameBrowsingBlockNodeIndex)item); }
        public void Insert(int index, IReadOnlyBrowsingBlockNodeIndex item) { base.Insert(index, (IFrameBrowsingBlockNodeIndex)item); }
        public bool Remove(IReadOnlyBrowsingBlockNodeIndex item) { return base.Remove((IFrameBrowsingBlockNodeIndex)item); }
        public void CopyTo(IReadOnlyBrowsingBlockNodeIndex[] array, int index) { base.CopyTo((IFrameBrowsingBlockNodeIndex[])array, index); }
        bool ICollection<IReadOnlyBrowsingBlockNodeIndex>.IsReadOnly { get { return ((ICollection<IFrameBrowsingBlockNodeIndex>)this).IsReadOnly; } }
        public bool Contains(IReadOnlyBrowsingBlockNodeIndex value) { return base.Contains((IFrameBrowsingBlockNodeIndex)value); }
        public int IndexOf(IReadOnlyBrowsingBlockNodeIndex value) { return base.IndexOf((IFrameBrowsingBlockNodeIndex)value); }
        IEnumerator<IReadOnlyBrowsingBlockNodeIndex> IEnumerable<IReadOnlyBrowsingBlockNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Writeable
        IWriteableBrowsingBlockNodeIndex IWriteableBrowsingBlockNodeIndexList.this[int index] { get { return base[index]; } set { base[index] = (IFrameBrowsingBlockNodeIndex)value; } }
        IWriteableBrowsingBlockNodeIndex IList<IWriteableBrowsingBlockNodeIndex>.this[int index] { get { return base[index]; } set { base[index] = (IFrameBrowsingBlockNodeIndex)value; } }
        IWriteableBrowsingBlockNodeIndex IReadOnlyList<IWriteableBrowsingBlockNodeIndex>.this[int index] { get { return base[index]; } }
        public void Add(IWriteableBrowsingBlockNodeIndex item) { base.Add((IFrameBrowsingBlockNodeIndex)item); }
        public void Insert(int index, IWriteableBrowsingBlockNodeIndex item) { base.Insert(index, (IFrameBrowsingBlockNodeIndex)item); }
        public bool Remove(IWriteableBrowsingBlockNodeIndex item) { return base.Remove((IFrameBrowsingBlockNodeIndex)item); }
        public void CopyTo(IWriteableBrowsingBlockNodeIndex[] array, int index) { base.CopyTo((IFrameBrowsingBlockNodeIndex[])array, index); }
        bool ICollection<IWriteableBrowsingBlockNodeIndex>.IsReadOnly { get { return ((ICollection<IFrameBrowsingBlockNodeIndex>)this).IsReadOnly; } }
        public bool Contains(IWriteableBrowsingBlockNodeIndex value) { return base.Contains((IFrameBrowsingBlockNodeIndex)value); }
        public int IndexOf(IWriteableBrowsingBlockNodeIndex value) { return base.IndexOf((IFrameBrowsingBlockNodeIndex)value); }
        IEnumerator<IWriteableBrowsingBlockNodeIndex> IWriteableBrowsingBlockNodeIndexList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IWriteableBrowsingBlockNodeIndex> IEnumerable<IWriteableBrowsingBlockNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        #endregion
    }
}