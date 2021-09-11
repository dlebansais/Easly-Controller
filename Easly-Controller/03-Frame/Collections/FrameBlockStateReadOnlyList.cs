namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FrameBlockStateReadOnlyList : WriteableBlockStateReadOnlyList, IReadOnlyCollection<IFrameBlockState>, IReadOnlyList<IFrameBlockState>
    {
        /// <inheritdoc/>
        public FrameBlockStateReadOnlyList(FrameBlockStateList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new IFrameBlockState this[int index] { get { return (IFrameBlockState)base[index]; } }

        #region IFrameBlockState
        IEnumerator<IFrameBlockState> IEnumerable<IFrameBlockState>.GetEnumerator() { System.Collections.IEnumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (IFrameBlockState)iterator.Current; } }
        IFrameBlockState IReadOnlyList<IFrameBlockState>.this[int index] { get { return (IFrameBlockState)this[index]; } }
        #endregion
    }
}
