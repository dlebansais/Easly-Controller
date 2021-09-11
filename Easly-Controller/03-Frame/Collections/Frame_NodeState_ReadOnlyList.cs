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

        /// <inheritdoc/>
        public new IFrameNodeState this[int index] { get { return (IFrameNodeState)base[index]; } }

        #region IFrameNodeState
        IEnumerator<IFrameNodeState> IEnumerable<IFrameNodeState>.GetEnumerator() { System.Collections.IEnumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (IFrameNodeState)iterator.Current; } }
        IFrameNodeState IReadOnlyList<IFrameNodeState>.this[int index] { get { return (IFrameNodeState)this[index]; } }
        #endregion
    }
}
