namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Focus;
    using EaslyController.ReadOnly;

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
        /// <inheritdoc/>
        public new IEnumerator<ILayoutBlockState> GetEnumerator() { var iterator = ((ReadOnlyCollection<IReadOnlyBlockState>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutBlockState)iterator.Current; } }

        #region ILayoutBlockState
        IEnumerator<ILayoutBlockState> IEnumerable<ILayoutBlockState>.GetEnumerator() { return GetEnumerator(); }
        ILayoutBlockState IReadOnlyList<ILayoutBlockState>.this[int index] { get { return (ILayoutBlockState)this[index]; } }
        #endregion
    }
}
