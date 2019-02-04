#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// List of IxxxIndexCollection
    /// </summary>
    internal interface IFocusIndexCollectionList : IFrameIndexCollectionList, IList<IFocusIndexCollection>, IReadOnlyList<IFocusIndexCollection>
    {
        new int Count { get; }
        new IFocusIndexCollection this[int index] { get; set; }
        new IEnumerator<IFocusIndexCollection> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxIndexCollection
    /// </summary>
    internal class FocusIndexCollectionList : Collection<IFocusIndexCollection>, IFocusIndexCollectionList
    {
        #region ReadOnly
        IReadOnlyIndexCollection IReadOnlyIndexCollectionList.this[int index] { get { return this[index]; } set { this[index] = (IFocusIndexCollection)value; } }
        IReadOnlyIndexCollection IList<IReadOnlyIndexCollection>.this[int index] { get { return this[index]; } set { this[index] = (IFocusIndexCollection)value; } }
        IReadOnlyIndexCollection IReadOnlyList<IReadOnlyIndexCollection>.this[int index] { get { return this[index]; } }
        public void Add(IReadOnlyIndexCollection item) { base.Add((IFocusIndexCollection)item); }
        public void Insert(int index, IReadOnlyIndexCollection item) { base.Insert(index, (IFocusIndexCollection)item); }
        public bool Remove(IReadOnlyIndexCollection item) { return base.Remove((IFocusIndexCollection)item); }
        public void CopyTo(IReadOnlyIndexCollection[] array, int index) { base.CopyTo((IFocusIndexCollection[])array, index); }
        bool ICollection<IReadOnlyIndexCollection>.IsReadOnly { get { return ((ICollection<IFocusIndexCollection>)this).IsReadOnly; } }
        public bool Contains(IReadOnlyIndexCollection value) { return base.Contains((IFocusIndexCollection)value); }
        public int IndexOf(IReadOnlyIndexCollection value) { return base.IndexOf((IFocusIndexCollection)value); }
        IEnumerator<IReadOnlyIndexCollection> IEnumerable<IReadOnlyIndexCollection>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Writeable
        IWriteableIndexCollection IWriteableIndexCollectionList.this[int index] { get { return this[index]; } set { this[index] = (IFocusIndexCollection)value; } }
        IWriteableIndexCollection IList<IWriteableIndexCollection>.this[int index] { get { return this[index]; } set { this[index] = (IFocusIndexCollection)value; } }
        IWriteableIndexCollection IReadOnlyList<IWriteableIndexCollection>.this[int index] { get { return this[index]; } }
        public void Add(IWriteableIndexCollection item) { base.Add((IFocusIndexCollection)item); }
        public void Insert(int index, IWriteableIndexCollection item) { base.Insert(index, (IFocusIndexCollection)item); }
        public bool Remove(IWriteableIndexCollection item) { return base.Remove((IFocusIndexCollection)item); }
        public void CopyTo(IWriteableIndexCollection[] array, int index) { base.CopyTo((IFocusIndexCollection[])array, index); }
        bool ICollection<IWriteableIndexCollection>.IsReadOnly { get { return ((ICollection<IFocusIndexCollection>)this).IsReadOnly; } }
        public bool Contains(IWriteableIndexCollection value) { return base.Contains((IFocusIndexCollection)value); }
        public int IndexOf(IWriteableIndexCollection value) { return base.IndexOf((IFocusIndexCollection)value); }
        IEnumerator<IWriteableIndexCollection> IWriteableIndexCollectionList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IWriteableIndexCollection> IEnumerable<IWriteableIndexCollection>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Frame
        IFrameIndexCollection IFrameIndexCollectionList.this[int index] { get { return this[index]; } set { this[index] = (IFocusIndexCollection)value; } }
        IFrameIndexCollection IList<IFrameIndexCollection>.this[int index] { get { return this[index]; } set { this[index] = (IFocusIndexCollection)value; } }
        IFrameIndexCollection IReadOnlyList<IFrameIndexCollection>.this[int index] { get { return this[index]; } }
        public void Add(IFrameIndexCollection item) { base.Add((IFocusIndexCollection)item); }
        public void Insert(int index, IFrameIndexCollection item) { base.Insert(index, (IFocusIndexCollection)item); }
        public bool Remove(IFrameIndexCollection item) { return base.Remove((IFocusIndexCollection)item); }
        public void CopyTo(IFrameIndexCollection[] array, int index) { base.CopyTo((IFocusIndexCollection[])array, index); }
        bool ICollection<IFrameIndexCollection>.IsReadOnly { get { return ((ICollection<IFocusIndexCollection>)this).IsReadOnly; } }
        public bool Contains(IFrameIndexCollection value) { return base.Contains((IFocusIndexCollection)value); }
        public int IndexOf(IFrameIndexCollection value) { return base.IndexOf((IFocusIndexCollection)value); }
        IEnumerator<IFrameIndexCollection> IFrameIndexCollectionList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IFrameIndexCollection> IEnumerable<IFrameIndexCollection>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        public virtual IReadOnlyIndexCollectionReadOnlyList ToReadOnly()
        {
            return new FocusIndexCollectionReadOnlyList(this);
        }
    }
}
