namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FrameBrowsingBlockNodeIndexReadOnlyList : WriteableBrowsingBlockNodeIndexReadOnlyList, IReadOnlyCollection<IFrameBrowsingBlockNodeIndex>, IReadOnlyList<IFrameBrowsingBlockNodeIndex>
    {
        /// <inheritdoc/>
        public FrameBrowsingBlockNodeIndexReadOnlyList(FrameBrowsingBlockNodeIndexList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new IFrameBrowsingBlockNodeIndex this[int index] { get { return (IFrameBrowsingBlockNodeIndex)base[index]; } }
        /// <inheritdoc/>
        public new IEnumerator<IFrameBrowsingBlockNodeIndex> GetEnumerator() { var iterator = ((ReadOnlyCollection<IReadOnlyBrowsingBlockNodeIndex>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IFrameBrowsingBlockNodeIndex)iterator.Current; } }

        #region IFrameBrowsingBlockNodeIndex
        IEnumerator<IFrameBrowsingBlockNodeIndex> IEnumerable<IFrameBrowsingBlockNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        IFrameBrowsingBlockNodeIndex IReadOnlyList<IFrameBrowsingBlockNodeIndex>.this[int index] { get { return (IFrameBrowsingBlockNodeIndex)this[index]; } }
        #endregion
    }
}
