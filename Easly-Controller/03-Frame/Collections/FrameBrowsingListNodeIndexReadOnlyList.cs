namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;
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
        /// <inheritdoc/>
        public new IEnumerator<IFrameBrowsingListNodeIndex> GetEnumerator() { var iterator = ((ReadOnlyCollection<IReadOnlyBrowsingListNodeIndex>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IFrameBrowsingListNodeIndex)iterator.Current; } }

        #region IFrameBrowsingListNodeIndex
        IEnumerator<IFrameBrowsingListNodeIndex> IEnumerable<IFrameBrowsingListNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        IFrameBrowsingListNodeIndex IReadOnlyList<IFrameBrowsingListNodeIndex>.this[int index] { get { return (IFrameBrowsingListNodeIndex)this[index]; } }
        #endregion
    }
}
