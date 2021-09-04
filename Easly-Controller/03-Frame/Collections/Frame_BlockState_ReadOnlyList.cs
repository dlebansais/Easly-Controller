﻿namespace EaslyController.Frame
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

        #region IFrameBlockState
        IEnumerator<IFrameBlockState> IEnumerable<IFrameBlockState>.GetEnumerator() { return ((IList<IFrameBlockState>)this).GetEnumerator(); }
        IFrameBlockState IReadOnlyList<IFrameBlockState>.this[int index] { get { return (IFrameBlockState)this[index]; } }
        #endregion
    }
}
