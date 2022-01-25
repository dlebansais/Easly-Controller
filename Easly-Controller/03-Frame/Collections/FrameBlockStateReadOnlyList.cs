namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;
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
        /// <inheritdoc/>
        public new IEnumerator<IFrameBlockState> GetEnumerator() { var iterator = ((ReadOnlyCollection<IReadOnlyBlockState>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IFrameBlockState)iterator.Current; } }

        #region IFrameBlockState
        IEnumerator<IFrameBlockState> IEnumerable<IFrameBlockState>.GetEnumerator() { return GetEnumerator(); }
        IFrameBlockState IReadOnlyList<IFrameBlockState>.this[int index] { get { return (IFrameBlockState)this[index]; } }
        #endregion
    }
}
