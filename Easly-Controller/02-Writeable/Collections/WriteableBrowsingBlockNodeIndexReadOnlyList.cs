namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class WriteableBrowsingBlockNodeIndexReadOnlyList : ReadOnlyBrowsingBlockNodeIndexReadOnlyList, IReadOnlyCollection<IWriteableBrowsingBlockNodeIndex>, IReadOnlyList<IWriteableBrowsingBlockNodeIndex>
    {
        /// <inheritdoc/>
        public WriteableBrowsingBlockNodeIndexReadOnlyList(WriteableBrowsingBlockNodeIndexList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new IWriteableBrowsingBlockNodeIndex this[int index] { get { return (IWriteableBrowsingBlockNodeIndex)base[index]; } }
        /// <inheritdoc/>
        public new IEnumerator<IWriteableBrowsingBlockNodeIndex> GetEnumerator() { var iterator = ((ReadOnlyCollection<IReadOnlyBrowsingBlockNodeIndex>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IWriteableBrowsingBlockNodeIndex)iterator.Current; } }

        #region IWriteableBrowsingBlockNodeIndex
        IEnumerator<IWriteableBrowsingBlockNodeIndex> IEnumerable<IWriteableBrowsingBlockNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        IWriteableBrowsingBlockNodeIndex IReadOnlyList<IWriteableBrowsingBlockNodeIndex>.this[int index] { get { return (IWriteableBrowsingBlockNodeIndex)this[index]; } }
        #endregion
    }
}
