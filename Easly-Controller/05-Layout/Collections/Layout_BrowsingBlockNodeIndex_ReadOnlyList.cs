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

        /// <inheritdoc/>
        public new ILayoutBrowsingBlockNodeIndex this[int index] { get { return (ILayoutBrowsingBlockNodeIndex)base[index]; } }

        #region ILayoutBrowsingBlockNodeIndex
        IEnumerator<ILayoutBrowsingBlockNodeIndex> IEnumerable<ILayoutBrowsingBlockNodeIndex>.GetEnumerator() { System.Collections.IEnumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutBrowsingBlockNodeIndex)iterator.Current; } }
        ILayoutBrowsingBlockNodeIndex IReadOnlyList<ILayoutBrowsingBlockNodeIndex>.this[int index] { get { return (ILayoutBrowsingBlockNodeIndex)this[index]; } }
        #endregion
    }
}
