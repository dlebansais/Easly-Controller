namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;

    /// <inheritdoc/>
    public class LayoutBrowsingBlockNodeIndexReadOnlyList : FocusBrowsingBlockNodeIndexReadOnlyList, IReadOnlyCollection<ILayoutBrowsingBlockNodeIndex>, IReadOnlyList<ILayoutBrowsingBlockNodeIndex>
    {
        /// <inheritdoc/>
        public LayoutBrowsingBlockNodeIndexReadOnlyList(LayoutBrowsingBlockNodeIndexList list)
            : base(list)
        {
        }

        #region ILayoutBrowsingBlockNodeIndex
        IEnumerator<ILayoutBrowsingBlockNodeIndex> IEnumerable<ILayoutBrowsingBlockNodeIndex>.GetEnumerator() { return ((IList<ILayoutBrowsingBlockNodeIndex>)this).GetEnumerator(); }
        ILayoutBrowsingBlockNodeIndex IReadOnlyList<ILayoutBrowsingBlockNodeIndex>.this[int index] { get { return (ILayoutBrowsingBlockNodeIndex)this[index]; } }
        #endregion
    }
}
