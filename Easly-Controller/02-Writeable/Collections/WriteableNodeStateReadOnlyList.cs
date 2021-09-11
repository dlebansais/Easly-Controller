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

        /// <inheritdoc/>
        public new IWriteableNodeState this[int index] { get { return (IWriteableNodeState)base[index]; } }

        #region IWriteableNodeState
        IEnumerator<IWriteableNodeState> IEnumerable<IWriteableNodeState>.GetEnumerator() { System.Collections.IEnumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (IWriteableNodeState)iterator.Current; } }
        IWriteableNodeState IReadOnlyList<IWriteableNodeState>.this[int index] { get { return (IWriteableNodeState)this[index]; } }
        #endregion
    }
}
