#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Read-only list of IxxxNodeState
    /// </summary>
    internal class WriteableNodeStateReadOnlyList : ReadOnlyNodeStateReadOnlyList, IReadOnlyCollection<IWriteableNodeState>, IReadOnlyList<IWriteableNodeState>
    {
        public WriteableNodeStateReadOnlyList(WriteableNodeStateList list)
            : base(list)
        {
        }

        #region IWriteableNodeState
        IEnumerator<IWriteableNodeState> IEnumerable<IWriteableNodeState>.GetEnumerator() { return new List<IWriteableNodeState>().GetEnumerator(); }
        IWriteableNodeState IReadOnlyList<IWriteableNodeState>.this[int index] { get { return (IWriteableNodeState)this[index]; } }
        #endregion
    }
}
