namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;
    using EaslyController.ReadOnly;

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
        /// <inheritdoc/>
        public new IEnumerator<ILayoutBrowsingBlockNodeIndex> GetEnumerator() { var iterator = ((System.Collections.ObjectModel.ReadOnlyCollection<IReadOnlyBrowsingBlockNodeIndex>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutBrowsingBlockNodeIndex)iterator.Current; } }

        #region ILayoutBrowsingBlockNodeIndex
        IEnumerator<ILayoutBrowsingBlockNodeIndex> IEnumerable<ILayoutBrowsingBlockNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        ILayoutBrowsingBlockNodeIndex IReadOnlyList<ILayoutBrowsingBlockNodeIndex>.this[int index] { get { return (ILayoutBrowsingBlockNodeIndex)this[index]; } }
        #endregion
    }
}
