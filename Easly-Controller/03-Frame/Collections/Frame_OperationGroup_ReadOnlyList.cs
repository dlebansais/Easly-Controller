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

        #region FrameOperationGroup
        IEnumerator<FrameOperationGroup> IEnumerable<FrameOperationGroup>.GetEnumerator() { return ((IList<FrameOperationGroup>)this).GetEnumerator(); }
        FrameOperationGroup IReadOnlyList<FrameOperationGroup>.this[int index] { get { return (FrameOperationGroup)this[index]; } }
        #endregion
    }
}
