namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;

    /// <inheritdoc/>
    public class LayoutInsertionChildNodeIndexReadOnlyList : FocusInsertionChildNodeIndexReadOnlyList, IReadOnlyCollection<ILayoutInsertionChildNodeIndex>, IReadOnlyList<ILayoutInsertionChildNodeIndex>
    {
        /// <inheritdoc/>
        public LayoutInsertionChildNodeIndexReadOnlyList(LayoutInsertionChildNodeIndexList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new ILayoutInsertionChildNodeIndex this[int index] { get { return (ILayoutInsertionChildNodeIndex)base[index]; } }
        /// <inheritdoc/>
        public new IEnumerator<ILayoutInsertionChildNodeIndex> GetEnumerator() { var iterator = ((System.Collections.ObjectModel.ReadOnlyCollection<IFocusInsertionChildNodeIndex>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutInsertionChildNodeIndex)iterator.Current; } }

        #region ILayoutInsertionChildNodeIndex
        IEnumerator<ILayoutInsertionChildNodeIndex> IEnumerable<ILayoutInsertionChildNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        ILayoutInsertionChildNodeIndex IReadOnlyList<ILayoutInsertionChildNodeIndex>.this[int index] { get { return (ILayoutInsertionChildNodeIndex)this[index]; } }
        #endregion
    }
}
