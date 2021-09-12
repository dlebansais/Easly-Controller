namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class FocusBlockStateReadOnlyList : FrameBlockStateReadOnlyList, IReadOnlyCollection<IFocusBlockState>, IReadOnlyList<IFocusBlockState>
    {
        /// <inheritdoc/>
        public FocusBlockStateReadOnlyList(FocusBlockStateList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new IFocusBlockState this[int index] { get { return (IFocusBlockState)base[index]; } }
        /// <inheritdoc/>
        public new IEnumerator<IFocusBlockState> GetEnumerator() { var iterator = ((System.Collections.ObjectModel.ReadOnlyCollection<IReadOnlyBlockState>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IFocusBlockState)iterator.Current; } }

        #region IFocusBlockState
        IEnumerator<IFocusBlockState> IEnumerable<IFocusBlockState>.GetEnumerator() { return GetEnumerator(); }
        IFocusBlockState IReadOnlyList<IFocusBlockState>.this[int index] { get { return (IFocusBlockState)this[index]; } }
        #endregion
    }
}
