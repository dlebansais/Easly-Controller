#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Read-only list of IxxxIndexCollection
    /// </summary>
    internal class WriteableIndexCollectionReadOnlyList : ReadOnlyIndexCollectionReadOnlyList, IReadOnlyCollection<IWriteableIndexCollection>, IReadOnlyList<IWriteableIndexCollection>
    {
        public WriteableIndexCollectionReadOnlyList(WriteableIndexCollectionList list)
            : base(list)
        {
        }

        #region IWriteableIndexCollection
        IEnumerator<IWriteableIndexCollection> IEnumerable<IWriteableIndexCollection>.GetEnumerator() { return new List<IWriteableIndexCollection>().GetEnumerator(); }
        IWriteableIndexCollection IReadOnlyList<IWriteableIndexCollection>.this[int index] { get { return (IWriteableIndexCollection)this[index]; } }
        #endregion
    }
}
