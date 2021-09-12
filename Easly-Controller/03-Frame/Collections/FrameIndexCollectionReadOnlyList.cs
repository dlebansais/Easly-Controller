namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;
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
        /// <inheritdoc/>
        public new IEnumerator<IFrameIndexCollection> GetEnumerator() { var iterator = ((System.Collections.ObjectModel.ReadOnlyCollection<IReadOnlyIndexCollection>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IFrameIndexCollection)iterator.Current; } }

        #region IFrameIndexCollection
        IEnumerator<IFrameIndexCollection> IEnumerable<IFrameIndexCollection>.GetEnumerator() { return GetEnumerator(); }
        IFrameIndexCollection IReadOnlyList<IFrameIndexCollection>.this[int index] { get { return (IFrameIndexCollection)this[index]; } }
        #endregion
    }
}
