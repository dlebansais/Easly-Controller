namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;

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

        #region IFocusBlockState
        IEnumerator<IFocusBlockState> IEnumerable<IFocusBlockState>.GetEnumerator() { return ((IList<IFocusBlockState>)this).GetEnumerator(); }
        IFocusBlockState IReadOnlyList<IFocusBlockState>.this[int index] { get { return (IFocusBlockState)this[index]; } }
        #endregion
    }
}
