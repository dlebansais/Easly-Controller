﻿namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class WriteableBlockStateReadOnlyList : ReadOnlyBlockStateReadOnlyList, IReadOnlyCollection<IWriteableBlockState>, IReadOnlyList<IWriteableBlockState>
    {
        /// <inheritdoc/>
        public WriteableBlockStateReadOnlyList(WriteableBlockStateList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new IWriteableBlockState this[int index] { get { return (IWriteableBlockState)base[index]; } }
        /// <inheritdoc/>
        public new IEnumerator<IWriteableBlockState> GetEnumerator() { var iterator = ((ReadOnlyCollection<IReadOnlyBlockState>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IWriteableBlockState)iterator.Current; } }

        #region IWriteableBlockState
        IEnumerator<IWriteableBlockState> IEnumerable<IWriteableBlockState>.GetEnumerator() { return GetEnumerator(); }

        IWriteableBlockState IReadOnlyList<IWriteableBlockState>.this[int index] { get { return this[index]; } }
        #endregion
    }
}
