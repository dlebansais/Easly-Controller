﻿namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;

    /// <inheritdoc/>
    public class LayoutBlockStateReadOnlyList : FocusBlockStateReadOnlyList, IReadOnlyCollection<ILayoutBlockState>, IReadOnlyList<ILayoutBlockState>
    {
        /// <inheritdoc/>
        public LayoutBlockStateReadOnlyList(LayoutBlockStateList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new ILayoutBlockState this[int index] { get { return (ILayoutBlockState)base[index]; } }

        #region ILayoutBlockState
        IEnumerator<ILayoutBlockState> IEnumerable<ILayoutBlockState>.GetEnumerator() { System.Collections.IEnumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutBlockState)iterator.Current; } }
        ILayoutBlockState IReadOnlyList<ILayoutBlockState>.this[int index] { get { return (ILayoutBlockState)this[index]; } }
        #endregion
    }
}
