namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class FocusBrowsingBlockNodeIndexReadOnlyList : FrameBrowsingBlockNodeIndexReadOnlyList, IReadOnlyCollection<IFocusBrowsingBlockNodeIndex>, IReadOnlyList<IFocusBrowsingBlockNodeIndex>
    {
        /// <inheritdoc/>
        public FocusBrowsingBlockNodeIndexReadOnlyList(FocusBrowsingBlockNodeIndexList list)
            : base(list)
        {
        }

        #region IFocusBrowsingBlockNodeIndex
        IEnumerator<IFocusBrowsingBlockNodeIndex> IEnumerable<IFocusBrowsingBlockNodeIndex>.GetEnumerator() { return ((IList<IFocusBrowsingBlockNodeIndex>)this).GetEnumerator(); }
        IFocusBrowsingBlockNodeIndex IReadOnlyList<IFocusBrowsingBlockNodeIndex>.this[int index] { get { return (IFocusBrowsingBlockNodeIndex)this[index]; } }
        #endregion
    }
}
