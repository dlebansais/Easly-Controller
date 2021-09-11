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

        #region IFocusBlockState
        IEnumerator<IFocusBlockState> IEnumerable<IFocusBlockState>.GetEnumerator() { System.Collections.IEnumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (IFocusBlockState)iterator.Current; } }
        IFocusBlockState IReadOnlyList<IFocusBlockState>.this[int index] { get { return (IFocusBlockState)this[index]; } }
        #endregion
    }
}
