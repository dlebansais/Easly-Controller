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

        /// <inheritdoc/>
        public new FrameOperation this[int index] { get { return (FrameOperation)base[index]; } }

        #region FrameOperation
        IEnumerator<FrameOperation> IEnumerable<FrameOperation>.GetEnumerator() { System.Collections.IEnumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (FrameOperation)iterator.Current; } }
        FrameOperation IReadOnlyList<FrameOperation>.this[int index] { get { return (FrameOperation)this[index]; } }
        #endregion
    }
}
