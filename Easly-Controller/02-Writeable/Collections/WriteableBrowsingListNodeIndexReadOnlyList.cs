namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class WriteableBrowsingListNodeIndexReadOnlyList : ReadOnlyBrowsingListNodeIndexReadOnlyList, IReadOnlyCollection<IWriteableBrowsingListNodeIndex>, IReadOnlyList<IWriteableBrowsingListNodeIndex>
    {
        /// <inheritdoc/>
        public WriteableBrowsingListNodeIndexReadOnlyList(WriteableBrowsingListNodeIndexList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new IWriteableBrowsingListNodeIndex this[int index] { get { return (IWriteableBrowsingListNodeIndex)base[index]; } }
        /// <inheritdoc/>
        public new IEnumerator<IWriteableBrowsingListNodeIndex> GetEnumerator() { var iterator = ((System.Collections.ObjectModel.ReadOnlyCollection<IReadOnlyBrowsingListNodeIndex>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IWriteableBrowsingListNodeIndex)iterator.Current; } }

        #region IWriteableBrowsingListNodeIndex
        IEnumerator<IWriteableBrowsingListNodeIndex> IEnumerable<IWriteableBrowsingListNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        IWriteableBrowsingListNodeIndex IReadOnlyList<IWriteableBrowsingListNodeIndex>.this[int index] { get { return (IWriteableBrowsingListNodeIndex)this[index]; } }
        #endregion
    }
}
