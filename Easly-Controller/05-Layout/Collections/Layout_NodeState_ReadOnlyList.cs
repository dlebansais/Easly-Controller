namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;

    /// <inheritdoc/>
    public class LayoutNodeStateReadOnlyList : FocusNodeStateReadOnlyList, IReadOnlyCollection<ILayoutNodeState>, IReadOnlyList<ILayoutNodeState>
    {
        /// <inheritdoc/>
        public LayoutNodeStateReadOnlyList(LayoutNodeStateList list)
            : base(list)
        {
        }

        #region ILayoutNodeState
        IEnumerator<ILayoutNodeState> IEnumerable<ILayoutNodeState>.GetEnumerator() { return ((IList<ILayoutNodeState>)this).GetEnumerator(); }
        ILayoutNodeState IReadOnlyList<ILayoutNodeState>.this[int index] { get { return (ILayoutNodeState)this[index]; } }
        #endregion
    }
}
