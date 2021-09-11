namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;

    /// <inheritdoc/>
    public class LayoutBrowsingListNodeIndexReadOnlyList : FocusBrowsingListNodeIndexReadOnlyList, IReadOnlyCollection<ILayoutBrowsingListNodeIndex>, IReadOnlyList<ILayoutBrowsingListNodeIndex>
    {
        /// <inheritdoc/>
        public LayoutBrowsingListNodeIndexReadOnlyList(LayoutBrowsingListNodeIndexList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new ILayoutBrowsingListNodeIndex this[int index] { get { return (ILayoutBrowsingListNodeIndex)base[index]; } }

        #region ILayoutBrowsingListNodeIndex
        IEnumerator<ILayoutBrowsingListNodeIndex> IEnumerable<ILayoutBrowsingListNodeIndex>.GetEnumerator() { System.Collections.IEnumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutBrowsingListNodeIndex)iterator.Current; } }
        ILayoutBrowsingListNodeIndex IReadOnlyList<ILayoutBrowsingListNodeIndex>.this[int index] { get { return (ILayoutBrowsingListNodeIndex)this[index]; } }
        #endregion
    }
}
