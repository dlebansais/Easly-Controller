namespace EaslyController.Frame
{
    using System.Collections.Generic;
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

        #region IFrameBrowsingBlockNodeIndex
        IEnumerator<IFrameBrowsingBlockNodeIndex> IEnumerable<IFrameBrowsingBlockNodeIndex>.GetEnumerator() { System.Collections.IEnumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (IFrameBrowsingBlockNodeIndex)iterator.Current; } }
        IFrameBrowsingBlockNodeIndex IReadOnlyList<IFrameBrowsingBlockNodeIndex>.this[int index] { get { return (IFrameBrowsingBlockNodeIndex)this[index]; } }
        #endregion
    }
}
