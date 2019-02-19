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
        new IFocusIndexCollection this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<IFocusIndexCollection> GetEnumerator();
        new void Clear();
    }

    /// <summary>
    /// List of IxxxIndexCollection
    /// </summary>
    internal class FocusIndexCollectionList : Collection<IFocusIndexCollection>, IFocusIndexCollectionList
    {
        #region ReadOnly
        IReadOnlyIndexCollection IReadOnlyIndexCollectionList.this[int index] { get { return this[index]; } set { this[index] = (IFocusIndexCollection)value; } }
        IReadOnlyIndexCollection IList<IReadOnlyIndexCollection>.this[int index] { get { return this[index]; } set { this[index] = (IFocusIndexCollection)value; } }
        int IList<IReadOnlyIndexCollection>.IndexOf(IReadOnlyIndexCollection value) { return IndexOf((IFocusIndexCollection)value); }
        void IList<IReadOnlyIndexCollection>.Insert(int index, IReadOnlyIndexCollection item) { Insert(index, (IFocusIndexCollection)item); }
        void ICollection<IReadOnlyIndexCollection>.Add(IReadOnlyIndexCollection item) { Add((IFocusIndexCollection)item); }
        bool ICollection<IReadOnlyIndexCollection>.Contains(IReadOnlyIndexCollection value) { return Contains((IFocusIndexCollection)value); }
        void ICollection<IReadOnlyIndexCollection>.CopyTo(IReadOnlyIndexCollection[] array, int index) { CopyTo((IFocusIndexCollection[])array, index); }
        bool ICollection<IReadOnlyIndexCollection>.IsReadOnly { get { return ((ICollection<IFocusIndexCollection>)this).IsReadOnly; } }
        bool ICollection<IReadOnlyIndexCollection>.Remove(IReadOnlyIndexCollection item) { return Remove((IFocusIndexCollection)item); }
        IEnumerator<IReadOnlyIndexCollection> IEnumerable<IReadOnlyIndexCollection>.GetEnumerator() { return GetEnumerator(); }
        IReadOnlyIndexCollection IReadOnlyList<IReadOnlyIndexCollection>.this[int index] { get { return this[index]; } }
        #endregion

        #region Writeable
        IWriteableIndexCollection IWriteableIndexCollectionList.this[int index] { get { return this[index]; } set { this[index] = (IFocusIndexCollection)value; } }
        IEnumerator<IWriteableIndexCollection> IWriteableIndexCollectionList.GetEnumerator() { return GetEnumerator(); }
        IWriteableIndexCollection IList<IWriteableIndexCollection>.this[int index] { get { return this[index]; } set { this[index] = (IFocusIndexCollection)value; } }
        int IList<IWriteableIndexCollection>.IndexOf(IWriteableIndexCollection value) { return IndexOf((IFocusIndexCollection)value); }
        void IList<IWriteableIndexCollection>.Insert(int index, IWriteableIndexCollection item) { Insert(index, (IFocusIndexCollection)item); }
        void ICollection<IWriteableIndexCollection>.Add(IWriteableIndexCollection item) { Add((IFocusIndexCollection)item); }
        bool ICollection<IWriteableIndexCollection>.Contains(IWriteableIndexCollection value) { return Contains((IFocusIndexCollection)value); }
        void ICollection<IWriteableIndexCollection>.CopyTo(IWriteableIndexCollection[] array, int index) { CopyTo((IFocusIndexCollection[])array, index); }
        bool ICollection<IWriteableIndexCollection>.IsReadOnly { get { return ((ICollection<IFocusIndexCollection>)this).IsReadOnly; } }
        bool ICollection<IWriteableIndexCollection>.Remove(IWriteableIndexCollection item) { return Remove((IFocusIndexCollection)item); }
        IEnumerator<IWriteableIndexCollection> IEnumerable<IWriteableIndexCollection>.GetEnumerator() { return GetEnumerator(); }
        IWriteableIndexCollection IReadOnlyList<IWriteableIndexCollection>.this[int index] { get { return this[index]; } }
        #endregion

        #region Frame
        IFrameIndexCollection IFrameIndexCollectionList.this[int index] { get { return this[index]; } set { this[index] = (IFocusIndexCollection)value; } }
        IEnumerator<IFrameIndexCollection> IFrameIndexCollectionList.GetEnumerator() { return GetEnumerator(); }
        IFrameIndexCollection IList<IFrameIndexCollection>.this[int index] { get { return this[index]; } set { this[index] = (IFocusIndexCollection)value; } }
        int IList<IFrameIndexCollection>.IndexOf(IFrameIndexCollection value) { return IndexOf((IFocusIndexCollection)value); }
        void IList<IFrameIndexCollection>.Insert(int index, IFrameIndexCollection item) { Insert(index, (IFocusIndexCollection)item); }
        void ICollection<IFrameIndexCollection>.Add(IFrameIndexCollection item) { Add((IFocusIndexCollection)item); }
        bool ICollection<IFrameIndexCollection>.Contains(IFrameIndexCollection value) { return Contains((IFocusIndexCollection)value); }
        void ICollection<IFrameIndexCollection>.CopyTo(IFrameIndexCollection[] array, int index) { CopyTo((IFocusIndexCollection[])array, index); }
        bool ICollection<IFrameIndexCollection>.IsReadOnly { get { return ((ICollection<IFocusIndexCollection>)this).IsReadOnly; } }
        bool ICollection<IFrameIndexCollection>.Remove(IFrameIndexCollection item) { return Remove((IFocusIndexCollection)item); }
        IEnumerator<IFrameIndexCollection> IEnumerable<IFrameIndexCollection>.GetEnumerator() { return GetEnumerator(); }
        IFrameIndexCollection IReadOnlyList<IFrameIndexCollection>.this[int index] { get { return this[index]; } }
        #endregion

        public virtual IReadOnlyIndexCollectionReadOnlyList ToReadOnly()
        {
            return new FocusIndexCollectionReadOnlyList(this);
        }
    }
}
