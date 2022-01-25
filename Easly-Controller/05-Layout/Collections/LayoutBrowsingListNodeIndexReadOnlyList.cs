namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Focus;
    using EaslyController.ReadOnly;

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
        /// <inheritdoc/>
        public new IEnumerator<ILayoutBrowsingListNodeIndex> GetEnumerator() { var iterator = ((ReadOnlyCollection<IReadOnlyBrowsingListNodeIndex>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutBrowsingListNodeIndex)iterator.Current; } }

        #region ILayoutBrowsingListNodeIndex
        IEnumerator<ILayoutBrowsingListNodeIndex> IEnumerable<ILayoutBrowsingListNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        ILayoutBrowsingListNodeIndex IReadOnlyList<ILayoutBrowsingListNodeIndex>.this[int index] { get { return (ILayoutBrowsingListNodeIndex)this[index]; } }
        #endregion
    }
}
