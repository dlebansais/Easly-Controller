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

        #region ILayoutInsertionChildNodeIndex
        IEnumerator<ILayoutInsertionChildNodeIndex> IEnumerable<ILayoutInsertionChildNodeIndex>.GetEnumerator() { return ((IList<ILayoutInsertionChildNodeIndex>)this).GetEnumerator(); }
        ILayoutInsertionChildNodeIndex IReadOnlyList<ILayoutInsertionChildNodeIndex>.this[int index] { get { return (ILayoutInsertionChildNodeIndex)this[index]; } }
        #endregion
    }
}
