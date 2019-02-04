#pragma warning disable 1591

namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// List of IxxxIndexCollection
    /// </summary>
    internal interface IFrameIndexCollectionList : IWriteableIndexCollectionList, IList<IFrameIndexCollection>, IReadOnlyList<IFrameIndexCollection>
    {
        new int Count { get; }
        new IFrameIndexCollection this[int index] { get; set; }
        new IEnumerator<IFrameIndexCollection> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxIndexCollection
    /// </summary>
    internal class FrameIndexCollectionList : Collection<IFrameIndexCollection>, IFrameIndexCollectionList
    {
        #region ReadOnly
        public new IReadOnlyIndexCollection this[int index] { get { return base[index]; } set { base[index] = (IFrameIndexCollection)value; } }
        public void Add(IReadOnlyIndexCollection item) { base.Add((IFrameIndexCollection)item); }
        public void Insert(int index, IReadOnlyIndexCollection item) { base.Insert(index, (IFrameIndexCollection)item); }
        public bool Remove(IReadOnlyIndexCollection item) { return base.Remove((IFrameIndexCollection)item); }
        public void CopyTo(IReadOnlyIndexCollection[] array, int index) { base.CopyTo((IFrameIndexCollection[])array, index); }
        bool ICollection<IReadOnlyIndexCollection>.IsReadOnly { get { return ((ICollection<IFrameIndexCollection>)this).IsReadOnly; } }
        public bool Contains(IReadOnlyIndexCollection value) { return base.Contains((IFrameIndexCollection)value); }
        public int IndexOf(IReadOnlyIndexCollection value) { return base.IndexOf((IFrameIndexCollection)value); }
        IEnumerator<IReadOnlyIndexCollection> IEnumerable<IReadOnlyIndexCollection>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Writeable
        IWriteableIndexCollection IWriteableIndexCollectionList.this[int index] { get { return base[index]; } set { base[index] = (IFrameIndexCollection)value; } }
        IWriteableIndexCollection IList<IWriteableIndexCollection>.this[int index] { get { return base[index]; } set { base[index] = (IFrameIndexCollection)value; } }
        IWriteableIndexCollection IReadOnlyList<IWriteableIndexCollection>.this[int index] { get { return base[index]; } }
        public void Add(IWriteableIndexCollection item) { base.Add((IFrameIndexCollection)item); }
        public void Insert(int index, IWriteableIndexCollection item) { base.Insert(index, (IFrameIndexCollection)item); }
        public bool Remove(IWriteableIndexCollection item) { return base.Remove((IFrameIndexCollection)item); }
        public void CopyTo(IWriteableIndexCollection[] array, int index) { base.CopyTo((IFrameIndexCollection[])array, index); }
        bool ICollection<IWriteableIndexCollection>.IsReadOnly { get { return ((ICollection<IFrameIndexCollection>)this).IsReadOnly; } }
        public bool Contains(IWriteableIndexCollection value) { return base.Contains((IFrameIndexCollection)value); }
        public int IndexOf(IWriteableIndexCollection value) { return base.IndexOf((IFrameIndexCollection)value); }
        IEnumerator<IWriteableIndexCollection> IWriteableIndexCollectionList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IWriteableIndexCollection> IEnumerable<IWriteableIndexCollection>.GetEnumerator() { return GetEnumerator(); }
        #endregion
    }
}
