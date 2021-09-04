namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FrameOperationReadOnlyList : WriteableOperationReadOnlyList, IReadOnlyCollection<FrameOperation>, IReadOnlyList<FrameOperation>
    {
        /// <inheritdoc/>
        public FrameOperationReadOnlyList(FrameOperationList list)
            : base(list)
        {
        }

        #region FrameOperation
        IEnumerator<FrameOperation> IEnumerable<FrameOperation>.GetEnumerator() { return ((IList<FrameOperation>)this).GetEnumerator(); }
        FrameOperation IReadOnlyList<FrameOperation>.this[int index] { get { return (FrameOperation)this[index]; } }
        #endregion
    }
}
