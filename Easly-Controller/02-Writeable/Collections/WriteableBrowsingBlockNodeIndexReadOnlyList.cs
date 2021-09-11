namespace EaslyController.Writeable
{
    using System.Collections.Generic;
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

        #region IWriteableBrowsingBlockNodeIndex
        IEnumerator<IWriteableBrowsingBlockNodeIndex> IEnumerable<IWriteableBrowsingBlockNodeIndex>.GetEnumerator() { System.Collections.IEnumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (IWriteableBrowsingBlockNodeIndex)iterator.Current; } }
        IWriteableBrowsingBlockNodeIndex IReadOnlyList<IWriteableBrowsingBlockNodeIndex>.this[int index] { get { return (IWriteableBrowsingBlockNodeIndex)this[index]; } }
        #endregion
    }
}
