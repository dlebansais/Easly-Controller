namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class WriteableIndexCollectionReadOnlyList : ReadOnlyIndexCollectionReadOnlyList, IReadOnlyCollection<IWriteableIndexCollection>, IReadOnlyList<IWriteableIndexCollection>
    {
        /// <inheritdoc/>
        public WriteableIndexCollectionReadOnlyList(WriteableIndexCollectionList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new IWriteableIndexCollection this[int index] { get { return (IWriteableIndexCollection)base[index]; } }
        /// <inheritdoc/>
        public new IEnumerator<IWriteableIndexCollection> GetEnumerator() { var iterator = ((System.Collections.ObjectModel.ReadOnlyCollection<IReadOnlyIndexCollection>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IWriteableIndexCollection)iterator.Current; } }

        #region IWriteableIndexCollection
        IEnumerator<IWriteableIndexCollection> IEnumerable<IWriteableIndexCollection>.GetEnumerator() { return GetEnumerator(); }
        IWriteableIndexCollection IReadOnlyList<IWriteableIndexCollection>.this[int index] { get { return (IWriteableIndexCollection)this[index]; } }
        #endregion
    }
}
