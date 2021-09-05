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

        #region IFocusNodeState
        IEnumerator<IFocusNodeState> IEnumerable<IFocusNodeState>.GetEnumerator() { return ((IList<IFocusNodeState>)this).GetEnumerator(); }
        IFocusNodeState IReadOnlyList<IFocusNodeState>.this[int index] { get { return (IFocusNodeState)this[index]; } }
        #endregion
    }
}
