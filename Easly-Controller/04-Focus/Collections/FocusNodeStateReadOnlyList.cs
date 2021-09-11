namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;

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

        #region IFocusNodeState
        IEnumerator<IFocusNodeState> IEnumerable<IFocusNodeState>.GetEnumerator() { System.Collections.IEnumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (IFocusNodeState)iterator.Current; } }
        IFocusNodeState IReadOnlyList<IFocusNodeState>.this[int index] { get { return (IFocusNodeState)this[index]; } }
        #endregion
    }
}
