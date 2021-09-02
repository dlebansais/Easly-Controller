namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class WriteableNodeStateReadOnlyList : ReadOnlyNodeStateReadOnlyList, IReadOnlyCollection<IWriteableNodeState>, IReadOnlyList<IWriteableNodeState>
    {
        /// <inheritdoc/>
        public WriteableNodeStateReadOnlyList(WriteableNodeStateList list)
            : base(list)
        {
        }

        #region IWriteableNodeState
        IEnumerator<IWriteableNodeState> IEnumerable<IWriteableNodeState>.GetEnumerator() { return ((IList<IWriteableNodeState>)this).GetEnumerator(); }
        IWriteableNodeState IReadOnlyList<IWriteableNodeState>.this[int index] { get { return (IWriteableNodeState)this[index]; } }
        #endregion
    }
}
