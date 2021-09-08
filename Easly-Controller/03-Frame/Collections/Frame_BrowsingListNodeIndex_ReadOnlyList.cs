namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FrameBrowsingListNodeIndexReadOnlyList : WriteableBrowsingListNodeIndexReadOnlyList, IReadOnlyCollection<IFrameBrowsingListNodeIndex>, IReadOnlyList<IFrameBrowsingListNodeIndex>
    {
        /// <inheritdoc/>
        public FrameBrowsingListNodeIndexReadOnlyList(FrameBrowsingListNodeIndexList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new IFrameBrowsingListNodeIndex this[int index] { get { return (IFrameBrowsingListNodeIndex)base[index]; } }

        #region IFrameBrowsingListNodeIndex
        IEnumerator<IFrameBrowsingListNodeIndex> IEnumerable<IFrameBrowsingListNodeIndex>.GetEnumerator() { return ((IList<IFrameBrowsingListNodeIndex>)this).GetEnumerator(); }
        IFrameBrowsingListNodeIndex IReadOnlyList<IFrameBrowsingListNodeIndex>.this[int index] { get { return (IFrameBrowsingListNodeIndex)this[index]; } }
        #endregion
    }
}
