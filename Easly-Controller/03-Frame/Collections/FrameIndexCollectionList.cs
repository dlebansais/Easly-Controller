namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FrameIndexCollectionList : WriteableIndexCollectionList, ICollection<IFrameIndexCollection>, IEnumerable<IFrameIndexCollection>, IList<IFrameIndexCollection>, IReadOnlyCollection<IFrameIndexCollection>, IReadOnlyList<IFrameIndexCollection>
    {
        /// <inheritdoc/>
        public new IFrameIndexCollection this[int index] { get { return (IFrameIndexCollection)base[index]; } set { base[index] = value; } }

        #region IFrameIndexCollection
        void ICollection<IFrameIndexCollection>.Add(IFrameIndexCollection item) { Add(item); }
        bool ICollection<IFrameIndexCollection>.Contains(IFrameIndexCollection item) { return Contains(item); }
        void ICollection<IFrameIndexCollection>.CopyTo(IFrameIndexCollection[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<IFrameIndexCollection>.Remove(IFrameIndexCollection item) { return Remove(item); }
        bool ICollection<IFrameIndexCollection>.IsReadOnly { get { return ((ICollection<IWriteableIndexCollection>)this).IsReadOnly; } }
        IEnumerator<IFrameIndexCollection> IEnumerable<IFrameIndexCollection>.GetEnumerator() { Enumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (IFrameIndexCollection)iterator.Current; } }
        IFrameIndexCollection IList<IFrameIndexCollection>.this[int index] { get { return (IFrameIndexCollection)this[index]; } set { this[index] = value; } }
        int IList<IFrameIndexCollection>.IndexOf(IFrameIndexCollection item) { return IndexOf(item); }
        void IList<IFrameIndexCollection>.Insert(int index, IFrameIndexCollection item) { Insert(index, item); }
        IFrameIndexCollection IReadOnlyList<IFrameIndexCollection>.this[int index] { get { return (IFrameIndexCollection)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyIndexCollectionReadOnlyList ToReadOnly()
        {
            return new FrameIndexCollectionReadOnlyList(this);
        }
    }
}
