namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FrameNodeStateReadOnlyList : WriteableNodeStateReadOnlyList, IReadOnlyCollection<IFrameNodeState>, IReadOnlyList<IFrameNodeState>
    {
        /// <inheritdoc/>
        public FrameNodeStateReadOnlyList(FrameNodeStateList list)
            : base(list)
        {
        }

        #region IFrameNodeState
        IEnumerator<IFrameNodeState> IEnumerable<IFrameNodeState>.GetEnumerator() { return ((IList<IFrameNodeState>)this).GetEnumerator(); }
        IFrameNodeState IReadOnlyList<IFrameNodeState>.this[int index] { get { return (IFrameNodeState)this[index]; } }
        #endregion
    }
}
