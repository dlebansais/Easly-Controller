namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class FocusNodeStateReadOnlyList : FrameNodeStateReadOnlyList, IReadOnlyCollection<IFocusNodeState>, IReadOnlyList<IFocusNodeState>
    {
        /// <inheritdoc/>
        public FocusNodeStateReadOnlyList(FocusNodeStateList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new IFocusNodeState this[int index] { get { return (IFocusNodeState)base[index]; } }
        /// <inheritdoc/>
        public new IEnumerator<IFocusNodeState> GetEnumerator() { var iterator = ((System.Collections.ObjectModel.ReadOnlyCollection<IReadOnlyNodeState>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IFocusNodeState)iterator.Current; } }

        #region IFocusNodeState
        IEnumerator<IFocusNodeState> IEnumerable<IFocusNodeState>.GetEnumerator() { return GetEnumerator(); }
        IFocusNodeState IReadOnlyList<IFocusNodeState>.this[int index] { get { return (IFocusNodeState)this[index]; } }
        #endregion
    }
}
