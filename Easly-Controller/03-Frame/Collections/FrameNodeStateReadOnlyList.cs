namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;
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
        /// <inheritdoc/>
        public new IEnumerator<IFrameNodeState> GetEnumerator() { var iterator = ((ReadOnlyCollection<IReadOnlyNodeState>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IFrameNodeState)iterator.Current; } }

        #region IFrameNodeState
        IEnumerator<IFrameNodeState> IEnumerable<IFrameNodeState>.GetEnumerator() { return GetEnumerator(); }
        IFrameNodeState IReadOnlyList<IFrameNodeState>.this[int index] { get { return (IFrameNodeState)this[index]; } }
        #endregion
    }
}
