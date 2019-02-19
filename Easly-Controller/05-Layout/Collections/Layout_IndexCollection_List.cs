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
    /// List of IxxxIndexCollection
    /// </summary>
    internal interface ILayoutIndexCollectionList : IFocusIndexCollectionList, IList<ILayoutIndexCollection>, IReadOnlyList<ILayoutIndexCollection>
    {
        new ILayoutIndexCollection this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<ILayoutIndexCollection> GetEnumerator();
        new void Clear();
    }

    /// <summary>
    /// List of IxxxIndexCollection
    /// </summary>
    internal class LayoutIndexCollectionList : Collection<ILayoutIndexCollection>, ILayoutIndexCollectionList
    {
        #region ReadOnly
        IReadOnlyIndexCollection IReadOnlyIndexCollectionList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutIndexCollection)value; } }
        IReadOnlyIndexCollection IList<IReadOnlyIndexCollection>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutIndexCollection)value; } }
        int IList<IReadOnlyIndexCollection>.IndexOf(IReadOnlyIndexCollection value) { return IndexOf((ILayoutIndexCollection)value); }
        void IList<IReadOnlyIndexCollection>.Insert(int index, IReadOnlyIndexCollection item) { Insert(index, (ILayoutIndexCollection)item); }
        void ICollection<IReadOnlyIndexCollection>.Add(IReadOnlyIndexCollection item) { Add((ILayoutIndexCollection)item); }
        bool ICollection<IReadOnlyIndexCollection>.Contains(IReadOnlyIndexCollection value) { return Contains((ILayoutIndexCollection)value); }
        void ICollection<IReadOnlyIndexCollection>.CopyTo(IReadOnlyIndexCollection[] array, int index) { CopyTo((ILayoutIndexCollection[])array, index); }
        bool ICollection<IReadOnlyIndexCollection>.IsReadOnly { get { return ((ICollection<ILayoutIndexCollection>)this).IsReadOnly; } }
        bool ICollection<IReadOnlyIndexCollection>.Remove(IReadOnlyIndexCollection item) { return Remove((ILayoutIndexCollection)item); }
        IEnumerator<IReadOnlyIndexCollection> IEnumerable<IReadOnlyIndexCollection>.GetEnumerator() { return GetEnumerator(); }
        IReadOnlyIndexCollection IReadOnlyList<IReadOnlyIndexCollection>.this[int index] { get { return this[index]; } }
        #endregion

        #region Writeable
        IWriteableIndexCollection IWriteableIndexCollectionList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutIndexCollection)value; } }
        IEnumerator<IWriteableIndexCollection> IWriteableIndexCollectionList.GetEnumerator() { return GetEnumerator(); }
        IWriteableIndexCollection IList<IWriteableIndexCollection>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutIndexCollection)value; } }
        int IList<IWriteableIndexCollection>.IndexOf(IWriteableIndexCollection value) { return IndexOf((ILayoutIndexCollection)value); }
        void IList<IWriteableIndexCollection>.Insert(int index, IWriteableIndexCollection item) { Insert(index, (ILayoutIndexCollection)item); }
        void ICollection<IWriteableIndexCollection>.Add(IWriteableIndexCollection item) { Add((ILayoutIndexCollection)item); }
        bool ICollection<IWriteableIndexCollection>.Contains(IWriteableIndexCollection value) { return Contains((ILayoutIndexCollection)value); }
        void ICollection<IWriteableIndexCollection>.CopyTo(IWriteableIndexCollection[] array, int index) { CopyTo((ILayoutIndexCollection[])array, index); }
        bool ICollection<IWriteableIndexCollection>.IsReadOnly { get { return ((ICollection<ILayoutIndexCollection>)this).IsReadOnly; } }
        bool ICollection<IWriteableIndexCollection>.Remove(IWriteableIndexCollection item) { return Remove((ILayoutIndexCollection)item); }
        IEnumerator<IWriteableIndexCollection> IEnumerable<IWriteableIndexCollection>.GetEnumerator() { return GetEnumerator(); }
        IWriteableIndexCollection IReadOnlyList<IWriteableIndexCollection>.this[int index] { get { return this[index]; } }
        #endregion

        #region Frame
        IFrameIndexCollection IFrameIndexCollectionList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutIndexCollection)value; } }
        IEnumerator<IFrameIndexCollection> IFrameIndexCollectionList.GetEnumerator() { return GetEnumerator(); }
        IFrameIndexCollection IList<IFrameIndexCollection>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutIndexCollection)value; } }
        int IList<IFrameIndexCollection>.IndexOf(IFrameIndexCollection value) { return IndexOf((ILayoutIndexCollection)value); }
        void IList<IFrameIndexCollection>.Insert(int index, IFrameIndexCollection item) { Insert(index, (ILayoutIndexCollection)item); }
        void ICollection<IFrameIndexCollection>.Add(IFrameIndexCollection item) { Add((ILayoutIndexCollection)item); }
        bool ICollection<IFrameIndexCollection>.Contains(IFrameIndexCollection value) { return Contains((ILayoutIndexCollection)value); }
        void ICollection<IFrameIndexCollection>.CopyTo(IFrameIndexCollection[] array, int index) { CopyTo((ILayoutIndexCollection[])array, index); }
        bool ICollection<IFrameIndexCollection>.IsReadOnly { get { return ((ICollection<ILayoutIndexCollection>)this).IsReadOnly; } }
        bool ICollection<IFrameIndexCollection>.Remove(IFrameIndexCollection item) { return Remove((ILayoutIndexCollection)item); }
        IEnumerator<IFrameIndexCollection> IEnumerable<IFrameIndexCollection>.GetEnumerator() { return GetEnumerator(); }
        IFrameIndexCollection IReadOnlyList<IFrameIndexCollection>.this[int index] { get { return this[index]; } }
        #endregion

        #region Focus
        IFocusIndexCollection IFocusIndexCollectionList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutIndexCollection)value; } }
        IEnumerator<IFocusIndexCollection> IFocusIndexCollectionList.GetEnumerator() { return GetEnumerator(); }
        IFocusIndexCollection IList<IFocusIndexCollection>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutIndexCollection)value; } }
        int IList<IFocusIndexCollection>.IndexOf(IFocusIndexCollection value) { return IndexOf((ILayoutIndexCollection)value); }
        void IList<IFocusIndexCollection>.Insert(int index, IFocusIndexCollection item) { Insert(index, (ILayoutIndexCollection)item); }
        void ICollection<IFocusIndexCollection>.Add(IFocusIndexCollection item) { Add((ILayoutIndexCollection)item); }
        bool ICollection<IFocusIndexCollection>.Contains(IFocusIndexCollection value) { return Contains((ILayoutIndexCollection)value); }
        void ICollection<IFocusIndexCollection>.CopyTo(IFocusIndexCollection[] array, int index) { CopyTo((ILayoutIndexCollection[])array, index); }
        bool ICollection<IFocusIndexCollection>.IsReadOnly { get { return ((ICollection<ILayoutIndexCollection>)this).IsReadOnly; } }
        bool ICollection<IFocusIndexCollection>.Remove(IFocusIndexCollection item) { return Remove((ILayoutIndexCollection)item); }
        IEnumerator<IFocusIndexCollection> IEnumerable<IFocusIndexCollection>.GetEnumerator() { return GetEnumerator(); }
        IFocusIndexCollection IReadOnlyList<IFocusIndexCollection>.this[int index] { get { return this[index]; } }
        #endregion

        public virtual IReadOnlyIndexCollectionReadOnlyList ToReadOnly()
        {
            return new LayoutIndexCollectionReadOnlyList(this);
        }
    }
}
