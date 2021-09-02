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

        #region IWriteableBrowsingBlockNodeIndex
        IEnumerator<IWriteableBrowsingBlockNodeIndex> IEnumerable<IWriteableBrowsingBlockNodeIndex>.GetEnumerator() { return ((IList<IWriteableBrowsingBlockNodeIndex>)this).GetEnumerator(); }
        IWriteableBrowsingBlockNodeIndex IReadOnlyList<IWriteableBrowsingBlockNodeIndex>.this[int index] { get { return (IWriteableBrowsingBlockNodeIndex)this[index]; } }
        #endregion
    }
}
