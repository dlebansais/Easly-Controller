namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class FocusBrowsingListNodeIndexReadOnlyList : FrameBrowsingListNodeIndexReadOnlyList, IReadOnlyCollection<IFocusBrowsingListNodeIndex>, IReadOnlyList<IFocusBrowsingListNodeIndex>
    {
        /// <inheritdoc/>
        public FocusBrowsingListNodeIndexReadOnlyList(FocusBrowsingListNodeIndexList list)
            : base(list)
        {
        }

        #region IFocusBrowsingListNodeIndex
        IEnumerator<IFocusBrowsingListNodeIndex> IEnumerable<IFocusBrowsingListNodeIndex>.GetEnumerator() { return ((IList<IFocusBrowsingListNodeIndex>)this).GetEnumerator(); }
        IFocusBrowsingListNodeIndex IReadOnlyList<IFocusBrowsingListNodeIndex>.this[int index] { get { return (IFocusBrowsingListNodeIndex)this[index]; } }
        #endregion
    }
}
