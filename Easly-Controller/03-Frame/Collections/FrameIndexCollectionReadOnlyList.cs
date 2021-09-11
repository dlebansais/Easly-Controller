namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FrameIndexCollectionReadOnlyList : WriteableIndexCollectionReadOnlyList, IReadOnlyCollection<IFrameIndexCollection>, IReadOnlyList<IFrameIndexCollection>
    {
        /// <inheritdoc/>
        public FrameIndexCollectionReadOnlyList(FrameIndexCollectionList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new IFrameIndexCollection this[int index] { get { return (IFrameIndexCollection)base[index]; } }

        #region IFrameIndexCollection
        IEnumerator<IFrameIndexCollection> IEnumerable<IFrameIndexCollection>.GetEnumerator() { System.Collections.IEnumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (IFrameIndexCollection)iterator.Current; } }
        IFrameIndexCollection IReadOnlyList<IFrameIndexCollection>.this[int index] { get { return (IFrameIndexCollection)this[index]; } }
        #endregion
    }
}
