namespace EaslyController.Frame
{
    using System.Collections.Generic;
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

        #region FrameOperationGroup
        IEnumerator<FrameOperationGroup> IEnumerable<FrameOperationGroup>.GetEnumerator() { System.Collections.IEnumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (FrameOperationGroup)iterator.Current; } }
        FrameOperationGroup IReadOnlyList<FrameOperationGroup>.this[int index] { get { return (FrameOperationGroup)this[index]; } }
        #endregion
    }
}
