namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FrameOperationGroupReadOnlyList : WriteableOperationGroupReadOnlyList, IReadOnlyCollection<FrameOperationGroup>, IReadOnlyList<FrameOperationGroup>
    {
        /// <inheritdoc/>
        public FrameOperationGroupReadOnlyList(FrameOperationGroupList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new FrameOperationGroup this[int index] { get { return (FrameOperationGroup)base[index]; } }
        /// <inheritdoc/>
        public new IEnumerator<FrameOperationGroup> GetEnumerator() { var iterator = ((ReadOnlyCollection<WriteableOperationGroup>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (FrameOperationGroup)iterator.Current; } }

        #region FrameOperationGroup
        IEnumerator<FrameOperationGroup> IEnumerable<FrameOperationGroup>.GetEnumerator() { return GetEnumerator(); }
        FrameOperationGroup IReadOnlyList<FrameOperationGroup>.this[int index] { get { return (FrameOperationGroup)this[index]; } }
        #endregion
    }
}
