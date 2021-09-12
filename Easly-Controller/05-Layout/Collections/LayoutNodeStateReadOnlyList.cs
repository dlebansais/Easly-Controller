namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class LayoutNodeStateReadOnlyList : FocusNodeStateReadOnlyList, IReadOnlyCollection<ILayoutNodeState>, IReadOnlyList<ILayoutNodeState>
    {
        /// <inheritdoc/>
        public LayoutNodeStateReadOnlyList(LayoutNodeStateList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new ILayoutNodeState this[int index] { get { return (ILayoutNodeState)base[index]; } }
        /// <inheritdoc/>
        public new IEnumerator<ILayoutNodeState> GetEnumerator() { var iterator = ((System.Collections.ObjectModel.ReadOnlyCollection<IReadOnlyNodeState>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutNodeState)iterator.Current; } }

        #region ILayoutNodeState
        IEnumerator<ILayoutNodeState> IEnumerable<ILayoutNodeState>.GetEnumerator() { return GetEnumerator(); }
        ILayoutNodeState IReadOnlyList<ILayoutNodeState>.this[int index] { get { return (ILayoutNodeState)this[index]; } }
        #endregion
    }
}
