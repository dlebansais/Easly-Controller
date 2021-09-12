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
        /// <inheritdoc/>
        public new IEnumerator<IWriteableNodeState> GetEnumerator() { var iterator = ((System.Collections.ObjectModel.ReadOnlyCollection<IReadOnlyNodeState>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IWriteableNodeState)iterator.Current; } }

        #region IWriteableNodeState
        IEnumerator<IWriteableNodeState> IEnumerable<IWriteableNodeState>.GetEnumerator() { return GetEnumerator(); }
        IWriteableNodeState IReadOnlyList<IWriteableNodeState>.this[int index] { get { return (IWriteableNodeState)this[index]; } }
        #endregion
    }
}
