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

        #region ILayoutInsertionChildNodeIndex
        IEnumerator<ILayoutInsertionChildNodeIndex> IEnumerable<ILayoutInsertionChildNodeIndex>.GetEnumerator() { System.Collections.IEnumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutInsertionChildNodeIndex)iterator.Current; } }
        ILayoutInsertionChildNodeIndex IReadOnlyList<ILayoutInsertionChildNodeIndex>.this[int index] { get { return (ILayoutInsertionChildNodeIndex)this[index]; } }
        #endregion
    }
}
