#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Read-only list of IxxxBlockState
    /// </summary>
    public class WriteableBlockStateReadOnlyList : ReadOnlyBlockStateReadOnlyList, IReadOnlyCollection<IWriteableBlockState>, IReadOnlyList<IWriteableBlockState>
    {
        public WriteableBlockStateReadOnlyList(WriteableBlockStateList list)
            : base(list)
        {
        }

        #region IWriteableBlockState
        IEnumerator<IWriteableBlockState> IEnumerable<IWriteableBlockState>.GetEnumerator() { return new List<IWriteableBlockState>().GetEnumerator(); }
        IWriteableBlockState IReadOnlyList<IWriteableBlockState>.this[int index] { get { return (IWriteableBlockState)this[index]; } }
        #endregion
    }
}
