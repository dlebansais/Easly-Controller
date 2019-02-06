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
        new IFrameIndexCollection this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<IFrameIndexCollection> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxIndexCollection
    /// </summary>
    internal class FrameIndexCollectionList : Collection<IFrameIndexCollection>, IFrameIndexCollectionList
    {
        #region ReadOnly
        IReadOnlyIndexCollection IReadOnlyIndexCollectionList.this[int index] { get { return this[index]; } set { this[index] = (IFrameIndexCollection)value; } }
        IReadOnlyIndexCollection IList<IReadOnlyIndexCollection>.this[int index] { get { return this[index]; } set { this[index] = (IFrameIndexCollection)value; } }
        int IList<IReadOnlyIndexCollection>.IndexOf(IReadOnlyIndexCollection value) { return IndexOf((IFrameIndexCollection)value); }
        void IList<IReadOnlyIndexCollection>.Insert(int index, IReadOnlyIndexCollection item) { Insert(index, (IFrameIndexCollection)item); }
        void ICollection<IReadOnlyIndexCollection>.Add(IReadOnlyIndexCollection item) { Add((IFrameIndexCollection)item); }
        bool ICollection<IReadOnlyIndexCollection>.Contains(IReadOnlyIndexCollection value) { return Contains((IFrameIndexCollection)value); }
        void ICollection<IReadOnlyIndexCollection>.CopyTo(IReadOnlyIndexCollection[] array, int index) { CopyTo((IFrameIndexCollection[])array, index); }
        bool ICollection<IReadOnlyIndexCollection>.IsReadOnly { get { return ((ICollection<IFrameIndexCollection>)this).IsReadOnly; } }
        bool ICollection<IReadOnlyIndexCollection>.Remove(IReadOnlyIndexCollection item) { return Remove((IFrameIndexCollection)item); }
        IEnumerator<IReadOnlyIndexCollection> IEnumerable<IReadOnlyIndexCollection>.GetEnumerator() { return GetEnumerator(); }
        IReadOnlyIndexCollection IReadOnlyList<IReadOnlyIndexCollection>.this[int index] { get { return this[index]; } }
        #endregion

        #region Writeable
        IWriteableIndexCollection IWriteableIndexCollectionList.this[int index] { get { return this[index]; } set { this[index] = (IFrameIndexCollection)value; } }
        IEnumerator<IWriteableIndexCollection> IWriteableIndexCollectionList.GetEnumerator() { return GetEnumerator(); }
        IWriteableIndexCollection IList<IWriteableIndexCollection>.this[int index] { get { return this[index]; } set { this[index] = (IFrameIndexCollection)value; } }
        int IList<IWriteableIndexCollection>.IndexOf(IWriteableIndexCollection value) { return IndexOf((IFrameIndexCollection)value); }
        void IList<IWriteableIndexCollection>.Insert(int index, IWriteableIndexCollection item) { Insert(index, (IFrameIndexCollection)item); }
        void ICollection<IWriteableIndexCollection>.Add(IWriteableIndexCollection item) { Add((IFrameIndexCollection)item); }
        bool ICollection<IWriteableIndexCollection>.Contains(IWriteableIndexCollection value) { return Contains((IFrameIndexCollection)value); }
        void ICollection<IWriteableIndexCollection>.CopyTo(IWriteableIndexCollection[] array, int index) { CopyTo((IFrameIndexCollection[])array, index); }
        bool ICollection<IWriteableIndexCollection>.IsReadOnly { get { return ((ICollection<IFrameIndexCollection>)this).IsReadOnly; } }
        bool ICollection<IWriteableIndexCollection>.Remove(IWriteableIndexCollection item) { return Remove((IFrameIndexCollection)item); }
        IEnumerator<IWriteableIndexCollection> IEnumerable<IWriteableIndexCollection>.GetEnumerator() { return GetEnumerator(); }
        IWriteableIndexCollection IReadOnlyList<IWriteableIndexCollection>.this[int index] { get { return this[index]; } }
        #endregion

        public virtual IReadOnlyIndexCollectionReadOnlyList ToReadOnly()
        {
            return new FrameIndexCollectionReadOnlyList(this);
        }
    }
}
